/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Configuration;
using NuLog.MetaData;
using NuLog.Targets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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
        private const string FailedToLogMessage = "Failed to log the message \"{0}\" because an exception occurred: \"{1}\"";

        private const string ExceptionInNuLogMessage = "Failure in NuLog \"{0}\"";
        private const string TypeNotFoundMessage = "Type not found for \"{0}\"";

        // Functional Values
        private const string TraceConfigCategory = "config";

        #endregion Constants

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

        // The timer used to execute processing of the logging queue, and the lock to protect it
        internal Timer _timer;

        // The exception handler to pass exceptions to.  This is optional and is used for debugging purposes
        //  providing a way for implementing applications to have visibility into exceptions that
        //  occur inside of the logging framework
        private Action<Exception, string> ExceptionHandler { get; set; }

        // The static meta data providers are used to automatically append static
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

        #endregion Members and Constructors

        #region Startup and Shutdown

        /// <summary>
        /// Signals to the dispatcher that it is to shutdown.
        /// </summary>
        /// <returns>Whether or not the dispatcher shut down cleanly.</returns>
        public bool Shutdown()
        {
            return ShutdownThread();
        }

        // Starts up the worker thread if it is not already started
        private void StartupThread()
        {
            lock (LoggingLock)
            {
                if (_timer == null)
                    _timer = new Timer(QueueWorkerThread, this, TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(500));
            }
        }

        // Shuts down the worker thread if it is started
        private bool ShutdownThread()
        {
            // Protect the timer
            lock (LoggingLock)
            {
                // Signal to the thread to shutdown
                Trace.WriteLine("Shutting down dispatcher, waiting for all log events to flush");

                // Call the worker body until the queues are empty
                while (ActionQueue.IsEmpty == false || LogQueue.IsEmpty == false)
                    QueueWorkerThread(this);

                if (_timer != null)
                {
                    // Wait for the thread to shutdown, or the timeout to elapse
                    _timer.Dispose();
                    _timer = null;
                }
            }

            // No conditionals
            return true;
        }

        #endregion Startup and Shutdown

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
            // default to asynchronous handling, unless overridden in the configuration
            if (!LoggingConfig.Synchronous)
            {
                // Enqueue the action for the worker thread to handle
                ActionQueue.Enqueue(action);
            }
            else
            {
                // If we are overridden to synchronous logging, process the action now
                action();
            }
        }

        // This is the method body of the worker thread
        private static void QueueWorkerThread(object dispatcherInstance)
        {
            Action action;
            LogEvent logEvent;
            LogEventDispatcher dispatcher = dispatcherInstance as LogEventDispatcher;

            // Only lock and loop if there is anything to process
            if (dispatcher.ActionQueue.IsEmpty == false || dispatcher.LogQueue.IsEmpty == false)
            {
                // Process all of the actions
                while (dispatcher.ActionQueue.IsEmpty == false)
                {
                    if (dispatcher.ActionQueue.TryDequeue(out action))
                    {
                        action();
                    }
                }

                // Then process all of the log events
                while (dispatcher.LogQueue.IsEmpty == false)
                {
                    if (dispatcher.LogQueue.TryDequeue(out logEvent))
                    {
                        // Update the log event's meta data with any static meta data providers
                        dispatcher.ExecuteStaticMetaDataProviders(logEvent);

                        // Determine which targets the log event needs to be dispatched to
                        var targets = dispatcher.GetTargetsForTags(logEvent.Tags);

                        // Dispatch the log event to the targets
                        if (targets != null)
                            foreach (var target in targets)
                                target.Enqueue(logEvent);
                    }
                }
            }
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

        #endregion Logging

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

            // Notify our dependents of the new config
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

        #endregion Configuration

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
                        // Pull the type and constructor for the provider, by name
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
                                    logEvent.MetaData[key] = MergeMetaData(logEvent.MetaData, metaData, key);
                            }
                            else
                            {
                                logEvent.MetaData = metaData;
                            }
                    }
                    catch (Exception ex)
                    {
                        if (logEvent == null || logEvent.Silent == false)
                            Trace.WriteLine(String.Format("Failure executing static meta data provider {0}: {1}:\r\n{2}", staticMetaDataProvider.GetType().FullName, ex.Message, ex.StackTrace), TraceConfigCategory);
                    }
                }
            }
        }

        // Merges the two values, favoring the primary if secondary is null
        private static object MergeMetaData(IDictionary<string, object> primary, IDictionary<string, object> secondary, string key)
        {
            var primaryValue = primary.ContainsKey(key)
                ? primary[key]
                : null;

            var secondaryValue = secondary[key];

            return secondaryValue == null
                ? primaryValue
                : secondaryValue;
        }

        #endregion Meta Data Providers

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
                if (logEventInfo == null || logEventInfo.Silent == false)
                Trace.WriteLine(message);
        }

        // Helper function for flattening strings into a single string
        private static string FlattenStrings(ICollection<string> stringCollection)
        {
            return String.Join(",", stringCollection.ToArray());
        }

        #endregion Helpers
    }
}