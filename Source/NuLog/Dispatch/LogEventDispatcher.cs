/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/7/2014
 * Updated: 11/13/2014
 * Changes: Removed MEF functionality for static meta dat aproviders and added it to the configuration.
 *   This allows developers to only activate the static meta data providers they whish
 *   to be active, as opposed to getting all of the ones in scope.
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Configuration;
using NuLog.MetaData;
using NuLog.Targets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace NuLog.Dispatch
{
    /// <summary>
    /// The standard dispatcher for the framework.
    /// Responsible for making sure that the log events make it
    /// to the intended targets based on the configuration, the
    /// "Man behind the curtain"
    /// Also responsible for managing the targets.
    /// 
    /// while the logging framework is designed to be quite simple,
    /// the dispatcher is the most complex 
    /// </summary>
    public class LogEventDispatcher : IConfigObserver
    {
        #region Constants
        // Messages
        private const string FailedToLogMessage = "Failed to log the message \"{0}\" because an exception occured: \"{1}\"";
        private const string ExceptionInNuLogMessage = "Failure in NuLog \"{0}\"";
        private const string TypeNotFoundMessage = "Type not found for \"{0}\"";

        // Functional Values
        private const string TraceConfigCategory = "config";
        /// <summary>
        /// The default timeout for shutting down the framework
        /// </summary>
        public const int DefaultShutdownTimeout = 30000; //30 seconds
        #endregion

        #region Members and Constructors

        // This lock is used for synchronization of tasks that need
        //  to be performed as such (such as reconfiguration of the framework)
        internal static readonly object LoggingLock = new object();

        // The operational logging configuration
        public LoggingConfig LoggingConfig { get; private set; }

        // The keepers are responsible for keeping track of the
        //  different pieces needed for dispatching log events
        //  They handle the inner-workings of the different subjects,
        //  Tags, Targets and Rules
        public TagKeeper TagKeeper { get; private set; }
        public TargetKeeper TargetKeeper { get; set; }
        public RuleKeeper RuleKeeper { get; private set; }

        // These queues are used to improve the performance of "fire-and-forget"
        //  The dispatcher will queue the actions and log events allowing
        //  the thread to return quickly, while a background thread works
        //  to process the queued actions and log events
        internal ConcurrentQueue<Action> ActionQueue { get; set; }
        internal ConcurrentQueue<LogEvent> LogQueue { get; set; }

        // This is used to cache the results of a given set of tags.  Once
        //  a collection of targets is determined for a flattened set of tags,
        //  it is stored here for a faster lookup, until the configuration
        //  changes, at which point this cache will be cleared
        private IDictionary<string, ICollection<TargetBase>> TargetCache { get; set; }

        // Internal variables used for the worker thread that processes
        //  the log event and action queues.  Helps with controlling
        //  the startup and shutdown of the thread
        internal Thread _queueWorkerThread;
        internal bool DoShutdownThread { get; set; }
        internal bool IsThreadShutdown { get; set; }

        // Status variables used to communicate shutdown status
        //  with the internal worker thread and external implementing applications
        protected bool IsShuttingDown
        {
            get
            {
                return DoShutdownThread;
            }
        }
        protected bool IsShutdown
        {
            get
            {
                return IsThreadShutdown;
            }
        }

        // The exception handler to pass exceptions to.  This is optional and is used for debugging purposes
        //  providing a way for implementing applications to have visibility into exceptions that
        //  occur inside of the logging framework
        private Action<Exception, string> ExceptionHandler { get; set; }

        // The static metadata providers are used to automatically append static
        //  information to the log events, such as machine name or environment
        private IList<IMetaDataProvider> StaticMetaDataProviders { get; set; }
        private bool StaticMetaDataProvidersLoaded { get; set; }

        /// <summary>
        /// Builds the dispatcher using the provided configuration and optional exception handler
        /// </summary>
        /// <param name="initialConfig">The initial configuration to use to build the dispatcher</param>
        /// <param name="exceptionHandler">The exception handler to use for the dispatcher</param>
        public LogEventDispatcher(LoggingConfig initialConfig, Action<Exception, string> exceptionHandler = null)
        {
            ExceptionHandler = exceptionHandler;

            TagKeeper = new TagKeeper();
            TargetKeeper = new TargetKeeper(this);
            RuleKeeper = new RuleKeeper(TagKeeper);
            TargetCache = new Dictionary<string, ICollection<TargetBase>>();

            ActionQueue = new ConcurrentQueue<Action>();
            LogQueue = new ConcurrentQueue<LogEvent>();

            // Wire up the configuration
            NewConfig(initialConfig);
        }

        #endregion

        #region Startup and Shutdown

        /// <summary>
        /// Signals to the dispatcher that it is to shutdown
        /// </summary>
        /// <param name="timeout">The amount of time in milliseconds that the dispatcher should limit itself to in shutting down</param>
        /// <returns>Whether or not the dispatcher successfully shut down in the time allocated</returns>
        public bool Shutdown(int timeout = DefaultShutdownTimeout)
        {
            lock (LoggingLock)
            {
                bool threadResult = ShutdownThread(timeout);

                TargetKeeper.Shutdown();

                return threadResult;
            }
        }

        // Starts up the worker thread if it is not already started
        private void StartupThread()
        {
            DoShutdownThread = false;

            if (_queueWorkerThread == null || _queueWorkerThread.IsAlive == false)
            {
                _queueWorkerThread = new Thread(new ThreadStart(this.QueueWorkerThread))
                {
                    IsBackground = true,
                    Priority = ThreadPriority.Lowest,
                    Name = "NuLog event dispatcher queue thread"
                };
                _queueWorkerThread.Start();

                IsThreadShutdown = false;
            }
        }

        // Shuts down the worker thread if it is started
        private bool ShutdownThread(int timeout = DefaultShutdownTimeout, Stopwatch stopwatch = null)
        {
            bool result = false;
            if (_queueWorkerThread != null && _queueWorkerThread.IsAlive)
            {
                // Signal to the thread to shutdown
                Trace.WriteLine("Shutting down dispatcher, waiting for all log evbents to flush");
                DoShutdownThread = true;

                // Make sure we are tracking the time shutting down
                if (stopwatch == null)
                {
                    stopwatch = new Stopwatch();
                    stopwatch.Start();
                }

                // Wait for the thread to shutdown, or the timeout to elapse
                while (_queueWorkerThread.IsAlive && stopwatch.ElapsedMilliseconds <= timeout)
                {
                    if (IsThreadShutdown)
                    {
                        result = true;
                        break;
                    }
                    Thread.Yield();
                }
            }
            else
            {
                result = true;
            }

            return result;
        }

        #endregion

        #region Logging

        /// <summary>
        /// Sends a log event to the dispatcher to be dispatched to the targets
        /// </summary>
        /// <param name="logEvent">The log event to dispatch</param>
        public void Log(LogEvent logEvent)
        {
            // Default to asynchronous logging, unless overridden in the configuration
            if (!LoggingConfig.Synchronous)
            {
                // Enqueue the log event for the worker thread to handle
                LogQueue.Enqueue(logEvent);
            }
            else
            {
                // If we are set to synchronous logging in the config, log the event now
                LogNow(logEvent);
            }
        }

        /// <summary>
        /// Sends an action to the dispatcher to be executed to log some item
        /// </summary>
        /// <param name="action">The action to process</param>
        public void Log(Action action)
        {
            // default to asynchronous handling, unless overriden in the configuration
            if (!LoggingConfig.Synchronous)
            {
                // Enqueue the action for the worker thread to handle
                ActionQueue.Enqueue(action);                
            }
            else
            {
                // If we are overriden to synchronous logging, process the action now
                action();
            }
        }

        // This is the method body of the worker thread
        private void QueueWorkerThread()
        {
            Action action;
            LogEvent logEvent;

            // Keep running while until the shutdown thread flag has been set
            while (!DoShutdownThread)
            {
                // Process all of the actions
                while (ActionQueue.IsEmpty == false)
                {
                    if (ActionQueue.TryDequeue(out action))
                    {
                        action();
                    }
                }

                // Then process all of the log events
                while (LogQueue.IsEmpty == false)
                {
                    if (LogQueue.TryDequeue(out logEvent))
                    {
                        // Update the log event's meta data with any static meta data providers
                        ExecuteStaticMetaDataProviders(logEvent);

                        // Determine which targets the log event needs to be dispatched to
                        var targets = GetTargetsForTags(logEvent.Tags);

                        // Dispatch the log event to the targets
                        if (targets != null)
                            foreach (var target in targets)
                                target.Enqueue(logEvent);
                    }
                }

                // Be kind, please rewind
                Thread.Yield();
            }

            // Signal that the thread has broken out of its execution loop
            IsThreadShutdown = true;
        }

        /// <summary>
        /// Logs the log event in this thread, before returning control to the logging application (synchronously)
        /// </summary>
        /// <param name="logEvent">The log event to log</param>
        public void LogNow(LogEvent logEvent)
        {
            // Update the log event's meta data with any static meta data providers
            ExecuteStaticMetaDataProviders(logEvent);

            // Determine what targets the log event is to go to
            var targets = GetTargetsForTags(logEvent.Tags);
            if (targets == null) return;

            // Dispatch the log event to the targets
            foreach (var target in targets)
                try
                {
                    target.Log(logEvent);
                }
                catch (Exception e)
                {
                    HandleException(e, logEvent);
                }
        }

        #endregion

        #region Configuration

        /// <summary>
        /// Notifies this dispatcher of a new configuration
        /// </summary>
        /// <param name="loggingConfig">The new configuration</param>
        public void NotifyNewConfig(LoggingConfig loggingConfig)
        {
            NewConfig(loggingConfig);
        }

        /// <summary>
        /// Reconfigures the dispatcher with the new logging config
        /// </summary>
        /// <param name="loggingConfig">The new logging config for the dispatcher</param>
        public void NewConfig(LoggingConfig loggingConfig)
        {
            // Store the new config
            LoggingConfig = loggingConfig;

            // Start or stop the thread depending on the configuration
            ConfigureThread();

            // Load Our Static Meta Data Providers
            LoadStaticMetaDataProviders();

            // Notify our dependants of the new config
            lock (LoggingLock)
            {
                // Notify the different keepers of the new config
                TagKeeper.NotifyNewConfig(loggingConfig);
                TargetKeeper.NotifyNewConfig(loggingConfig);
                RuleKeeper.NotifyNewConfig(loggingConfig);

                // And clear the target cache
                TargetCache.Clear();
            }
        }

        // Decides whether to startup the thread or to stop it based on the passed "synchronous" flag
        private void ConfigureThread()
        {
            if (!LoggingConfig.Synchronous)
            {
                // Startup the worker thread if we are not configured to be synchronous
                StartupThread();
            }
            else
            {
                // Shutdown the worker thread if we are configured to be synchronous
                ShutdownThread();
            }
        }

        #endregion

        #region Meta Data Providers

        // Loads the static meta data providers
        private void LoadStaticMetaDataProviders()
        {
            StaticMetaDataProviders = new List<IMetaDataProvider>();
            // Iterate over the list of static meta data providers and use
            //  reflection to instantiate them
            if (LoggingConfig.StaticMetaDataProviders != null && LoggingConfig.StaticMetaDataProviders.Count > 0)
            {
                Type providerType;
                ConstructorInfo constructorInfo;
                IMetaDataProvider metaDataProvider;

                foreach (string providerFullName in LoggingConfig.StaticMetaDataProviders)
                {
                    try
                    {
                        // Pull the type and and constructor for the provider, by name
                        providerType = Type.GetType(providerFullName);
                        if (providerType != null)
                        {
                            constructorInfo = providerType.GetConstructor(new Type[] { });
                            metaDataProvider = (IMetaDataProvider)constructorInfo.Invoke(null);
                            StaticMetaDataProviders.Add(metaDataProvider);
                        }
                        else
                        {
                            throw new LoggingException(String.Format(TypeNotFoundMessage, providerFullName));
                        }
                    }
                    catch (Exception exception)
                    {
                        // Handle the failure
                        HandleException(exception);
                    }
                }
            }
        }

        // Executes the meta data providers against the log event
        private void ExecuteStaticMetaDataProviders(LogEvent logEvent)
        {
            if (StaticMetaDataProviders != null && StaticMetaDataProviders.Count > 0)
            {
                foreach (var staticMetaDataProvider in StaticMetaDataProviders)
                {
                    try
                    {
                        // Get the meta data from the provider and merge it
                        //  into the log event's meta data
                        var metaData = staticMetaDataProvider.ProvideMetaData();
                        if (metaData != null)
                            if (logEvent.MetaData != null)
                            {
                                foreach (var key in metaData.Keys)
                                    logEvent.MetaData[key] = metaData[key];
                            }
                            else
                            {
                                logEvent.MetaData = metaData;
                            }
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(String.Format("Failure executing static metadata provider {0}: {1}:\r\n{2}", staticMetaDataProvider.GetType().FullName, ex.Message, ex.StackTrace), TraceConfigCategory);
                    }
                }
            }
        }

        #endregion

        #region Helpers

        // An internal method for determining which targets a log event is to be dispatched to
        //  based on the tags assigned to the log event and the rules in the configuration
        private ICollection<TargetBase> GetTargetsForTags(ICollection<string> tags)
        {
            // I am not sure if we need to lock here, but because I am in doubt, I will lock here to be safe
            //  I plan to think this out later, but this isn't a huge performance concern at the moment
            //  because we are caching the results
            var targetNamesKey = FlattenStrings(tags);
            lock (LoggingLock)
            {
                // Check the cache to see if we already have figured this out
                if (TargetCache.ContainsKey(targetNamesKey))
                {
                    return TargetCache[targetNamesKey];
                }
                else
                {
                    // We haven't figured this out yet, figure it out, cache it and return the results
                    var targetNames = RuleKeeper.GetTargetsForTags(tags);
                    var targets = TargetKeeper.GetTargets(targetNames);
                    TargetCache[targetNamesKey] = targets;
                    return targets;
                }
            }
        }

        // Internal function for handing exceptions thrown within the scope of the dispatcher
        public void HandleException(Exception e, LogEvent logEventInfo = null)
        {
            // Format the exception
            string message = logEventInfo != null
                ? String.Format(FailedToLogMessage, logEventInfo.Message, e)
                : String.Format(ExceptionInNuLogMessage, e);

            // Pass the exception to the exception handler, if we have one, otherwise write it out to trace
            if (ExceptionHandler != null)
                ExceptionHandler.Invoke(e, message);
            else
                Trace.WriteLine(message);
        }

        // Helper function for flattening strings into a single string
        private static string FlattenStrings(ICollection<string> stringCollection)
        {
            return String.Join(",", stringCollection.ToArray());
        }

        #endregion
    }
}
