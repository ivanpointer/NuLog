/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/8/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using NuLog.Configuration.Targets;
using NuLog.Dispatch;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NuLog.Targets
{
    /// <summary>
    /// The base abstract target which defines the core features and functionality common to all targets
    /// </summary>
    public abstract class TargetBase
    {

        #region Constants

        private const string DefaultName = "TargetBase";
        private const bool DefaultSynchronousSetting = false;
        public const int DefaultShutdownTimeout = 30000; // 30 seconds

        #endregion

        #region Members, Constructors, Initialization and Shutdown

        /// <summary>
        /// The name of the target.  Used to associate this target to the rules.
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// Whether or not this target should handle log events synchronously
        /// </summary>
        public bool Synchronous { get; protected set; }

        // Used to determine the internal status of the target
        protected bool DoShutdown
        {
            get
            {
                return DoShutdownThread;
            }
        }
        public bool IsShutdown
        {
            get
            {
                return IsThreadShutdown;
            }
        }

        // The dispatcher associated to this target
        protected LogEventDispatcher Dispatcher { get; private set; }
        
        // The target configuration used to build this target
        protected TargetConfig BaseTargetConfig { get; private set; }

        // A queue of log events to be processed asynchronously
        protected ConcurrentQueue<LogEvent> LogQueue { get; set; }

        // Worker thread for handling queued log events
        internal Thread _queueWorkerThread;
        protected bool DoShutdownThread { get; set; }
        protected bool IsThreadShutdown { get; set; }

        /// <summary>
        /// Builds a basic and empty target base
        /// </summary>
        public TargetBase()
        {
            InitializeInstance(null);
        }

        /// <summary>
        /// Buils a basic target base, with the assocaited dispatcher
        /// </summary>
        /// <param name="dispatcher">The dispatcher that owns this target</param>
        public TargetBase(LogEventDispatcher dispatcher)
        {
            InitializeInstance(dispatcher);
        }

        /// <summary>
        /// Initializes this instance to the provided dispatcher
        /// </summary>
        /// <param name="dispatcher">The dispatcher that owns this target</param>
        private void InitializeInstance(LogEventDispatcher dispatcher)
        {
            Dispatcher = dispatcher;
            LogQueue = new ConcurrentQueue<LogEvent>();

            DoShutdownThread = false;
            IsThreadShutdown = false;
        }

        /// <summary>
        /// Initializes the target using the provided information
        /// </summary>
        /// <param name="targetConfig">The configuration to use for this target</param>
        /// <param name="dispatcher">The dispatcher this target belongs to</param>
        /// <param name="synchronous">An override of the synchronous flag in the target config</param>
        public virtual void Initialize(TargetConfig targetConfig, LogEventDispatcher dispatcher = null, bool? synchronous = null)
        {
            // Wire up our pieces
            BaseTargetConfig = targetConfig;
            Dispatcher = dispatcher;

            // use the passed configuration if we have it, otherwise, use default values
            if (targetConfig != null)
            {
                Name = targetConfig.Name;
                Synchronous = synchronous.HasValue
                    ? synchronous.Value
                    : targetConfig.Synchronous.HasValue
                        ? targetConfig.Synchronous.Value
                        : DefaultSynchronousSetting;
            }
            else
            {
                Name = DefaultName;
                Synchronous = synchronous.HasValue
                    ? synchronous.Value
                    : DefaultSynchronousSetting;
            }

            // Wire up the worker thread (will start a thread if synchronous is false, will stop it otherwise)
            ConfigureWorkerThread(Synchronous);
        }

        /// <summary>
        /// Hook to notify this target of a new configuration
        /// </summary>
        /// <param name="targetConfig">The new configuration for this target</param>
        public virtual void NotifyNewConfig(TargetConfig targetConfig)
        {
            Initialize(targetConfig);
        }

        #endregion

        #region Internal Workings

        /// <summary>
        /// Enqueues the log event for asynchronous logging, unless the synchronous override flag is set, in which event, the log event is logged now
        /// </summary>
        /// <param name="logEvent">The log event to log</param>
        public void Enqueue(LogEvent logEvent)
        {
            // Make sure we are not shutdown, and check to see if this needs to be logged synchronously.  Log accordingly.
            if (DoShutdown)
                throw new InvalidOperationException("Cannot enqueue new log event when shutting down target");

            if (!Synchronous)
                LogQueue.Enqueue(logEvent);
            else
                Log(logEvent);
        }

        // The internal function for shutting down the target.  This is here to handle the internal worker thread
        //  So that overrides to <see cref="Shutdown" /> do not have to concern themselves with the worker thread
        internal bool ShutdownInternal(int timeout = DefaultShutdownTimeout)
        {
            bool shutdownThreadResult = ShutdownThread(timeout);

            return Shutdown() && shutdownThreadResult;
        }

        #endregion

        #region Logging and Shutdown (Abstract and Virtual)
        /* These functions are setup to be overriden by implementing targets.
         * They provide everything needed for logging activities */

        /// <summary>
        /// Logs the given log event now
        /// </summary>
        /// <param name="logEvent">The log event to log</param>
        public abstract void Log(LogEvent logEvent);

        /// <summary>
        /// Processes the log queue of events.  This is used for asynchronous logging.  The worker thread calls this function
        /// to handle queued log events.  The dispatcher is provided for access to the other parts of the framework that
        /// may be needed for logging purposes, for example, the tag keeper.
        /// 
        /// Override this function to provide advanced asynchronous logging logic.  By default, this function simply calls
        /// <see cref="Log" />.  Additional performance gains can be had, especially when using an external resource such
        /// as a file, database or service, by opening the handle/connection once and using it for each of the log events,
        /// as opposed to opening and closing the handle/connection for each log event.  Special consideration should be
        /// taken for "playing nice", if you expect that processing the log events in this queue may be expensive in terms
        /// of CPU and cycles, consider processing a limited number of log events from the queue before breaking, releasing
        /// the control back to the system more curteously.
        /// 
        /// TODO: Consider removing the connection of the dispatcher in favor of the TagKeeper, reducing the coupling needed
        ///   and complexity.
        /// </summary>
        /// <param name="logQueue">The queue of log events to log</param>
        /// <param name="dispatcher">The dispatcher that owns this target</param>
        protected virtual void ProcessLogQueue(ConcurrentQueue<LogEvent> logQueue, LogEventDispatcher dispatcher)
        {
            // Iterate over the queue, removing and logging each log event
            LogEvent logEvent;
            while (logQueue.IsEmpty == false)
            {
                if (logQueue.TryDequeue(out logEvent))
                {
                    try
                    {
                        Log(logEvent);
                    }
                    catch (Exception e)
                    {
                        if (dispatcher != null)
                            dispatcher.HandleException(e, logEvent);
                        else
                            throw e;
                    }
                }
            }
        }

        /// <summary>
        /// Override this to perform any house-keeping tasks needed for shutting down the target
        /// </summary>
        /// <returns>A bool indicating the success of shutting down the thread</returns>
        public virtual bool Shutdown()
        {
            return true;
        }

        #endregion

        #region Worker Thread

        // Decides whether to startup or shutdown the internal worker thread based on whether or not the target
        //  is configured to operate asynchronously or synchronously
        private void ConfigureWorkerThread(bool synchronous)
        {
            if (synchronous)
            {
                ShutdownThread();
            }
            else
            {
                StartupThread();
            }
        }

        // Starts up the worker thread if it is not already started
        private void StartupThread()
        {
            if (_queueWorkerThread == null || _queueWorkerThread.IsAlive == false)
            {
                _queueWorkerThread = new Thread(new ThreadStart(QueueWorkerThread))
                {
                    IsBackground = true,
                    Name = String.Format("{0} Queue Worker Thread", Name)
                };
                _queueWorkerThread.Start();
            }
        }

        // Shuts down the worker thread if it is started
        //  This is done by setting a flag indicating to the thread that it should shutdown
        //  This function then waits for a given time (timeout) for the thread to indicate that it is shutdown
        //   and returns a bool indicating whether the thread indicated that it is shutdown, within the time
        //   alotted.  If timeout is set to 0, this will wait indefinately for the thread to signal shutdown.
        //   I highly suggest against this, as it can result in a stalled application.  Instead, set a higher
        //   timeout to allow enough time for the thread to shutdown.
        private bool ShutdownThread(int timeout = DefaultShutdownTimeout, Stopwatch stopwatch = null)
        {
            if (_queueWorkerThread != null && _queueWorkerThread.IsAlive)
            {
                Trace.WriteLine(String.Format("Shutting down target \"{0}\", waiting for log queue to flush", Name));

                // Signal to the thread that it should shut down
                DoShutdownThread = true;

                // Start the stopwatch
                if (stopwatch == null)
                {
                    stopwatch = new Stopwatch();
                    stopwatch.Start();
                }

                // Wait for the thread to shutdown, or the timeout to elapse
                while (_queueWorkerThread.IsAlive && (timeout == 0 || stopwatch.ElapsedMilliseconds <= timeout))
                    if (IsShutdown)
                        return true;

                // If we got here, we failed to shutdown the thread, abort it
                _queueWorkerThread.Abort();
            }
            else
            {
                // The thread is not started, consider this a success
                return true;
            }

            // We failed to exit successfully, report as such
            return false;
        }

        // The "body" of the worker thread
        //  Simply watches the queue for log events and calls <see cref="ProcessLogQueue" /> if there are log events to process
        //  Yields after each check to "play nice"
        protected void QueueWorkerThread()
        {
            //  After the shutdown flag is set, we break out of the loop and signal that we are shutdown

            while (!DoShutdown)
            {
                if (LogQueue.IsEmpty == false)
                    ProcessLogQueue(LogQueue, Dispatcher);

                Thread.Yield();
            }

            IsThreadShutdown = true;
        }

        #endregion

    }
}
