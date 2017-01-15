/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration.Targets;
using NuLog.Dispatch;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace NuLog.Targets
{
    /// <summary>
    /// The base abstract target which defines the core features and functionality common to all targets
    /// </summary>
    public abstract class TargetBase
    {
        #region Constants / Statics

        private const string DefaultName = "TargetBase";
        private const bool DefaultSynchronousSetting = false;
        public const int DefaultShutdownTimeout = 30000; // 30 seconds

        /// <summary>
        /// The interval of time that the log queue is checked for new messages and flushed.
        /// </summary>
        public static int LogQueueTimerInterval = 200;

        #endregion Constants / Statics

        #region Members, Constructors, Initialization and Shutdown

        /// <summary>
        /// The name of the target.  Used to associate this target to the rules.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Whether or not this target should handle log events synchronously
        /// </summary>
        public bool Synchronous { get; protected set; }

        // The dispatcher associated to this target
        protected LogEventDispatcher Dispatcher { get; private set; }

        // The target configuration used to build this target
        protected TargetConfig BaseTargetConfig { get; private set; }

        // A queue of log events to be processed asynchronously
        protected ConcurrentQueue<LogEvent> LogQueue { get; set; }

        // The timer for working the log queue
        private Timer _timer;

        // The lock for the timer
        private readonly object _timerLock;

        /// <summary>
        /// Indicates whether or not the log queue is active, or if it's been shut down.
        /// </summary>
        protected bool IsLogQueueActive { get; private set; }

        /// <summary>
        /// Builds a basic and empty target base
        /// </summary>
        public TargetBase()
        {
            _timerLock = new object();

            InitializeInstance(null);
        }

        /// <summary>
        /// Builds a basic target base, with the associated dispatcher
        /// </summary>
        /// <param name="dispatcher">The dispatcher that owns this target</param>
        public TargetBase(LogEventDispatcher dispatcher)
        {
            _timerLock = new object();

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

            ConfigureLogQueueTimer(Synchronous);
        }

        /// <summary>
        /// Hook to notify this target of a new configuration
        /// </summary>
        /// <param name="targetConfig">The new configuration for this target</param>
        public virtual void NotifyNewConfig(TargetConfig targetConfig)
        {
            Initialize(targetConfig);
        }

        #endregion Members, Constructors, Initialization and Shutdown

        #region Internal Workings

        /// <summary>
        /// Enqueues the log event for asynchronous logging, unless the synchronous override flag is set, in which event, the log event is logged now
        /// </summary>
        /// <param name="logEvent">The log event to log</param>
        public void Enqueue(LogEvent logEvent)
        {
            // Make sure we are not shutdown, and check to see if this needs to be logged synchronously.  Log accordingly.
            if (IsLogQueueActive == false)
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
            // Shutdown the timer that watches the queue
            ShutdownLogQueueTimer();

            // Flush the queue
            QueueWorkerThread(this);

            // Perform any other shutdown operations
            return Shutdown();
        }

        #endregion Internal Workings

        #region Logging and Shutdown (Abstract and Virtual)

        /* These functions are setup to be overridden by implementing targets.
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
        /// the control back to the system more courteously.
        /// </summary>
        protected virtual void ProcessLogQueue()
        {
            // Iterate over the queue, removing and logging each log event
            LogEvent logEvent;
            while (LogQueue.IsEmpty == false)
            {
                if (LogQueue.TryDequeue(out logEvent))
                {
                    try
                    {
                        Log(logEvent);
                    }
                    catch (Exception e)
                    {
                        if (Dispatcher != null)
                            Dispatcher.HandleException(e, logEvent);
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

        #endregion Logging and Shutdown (Abstract and Virtual)

        #region Log Queue Timer

        private void ConfigureLogQueueTimer(bool synchronous)
        {
            if (synchronous)
            {
                ShutdownLogQueueTimer();
            }
            else
            {
                StartupLogQueueTimer();
            }
        }

        private void StartupLogQueueTimer()
        {
            lock (_timerLock)
            {
                if (_timer == null)
                {
                    _timer = new Timer(QueueWorkerThread, this, TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(LogQueueTimerInterval));
                    IsLogQueueActive = true;
                }
            }
        }

        private void ShutdownLogQueueTimer()
        {
            lock (_timerLock)
            {
                IsLogQueueActive = false;

                if (_timer != null)
                {
                    _timer.Dispose();
                    _timer = null;
                }
            }
        }

        private static void QueueWorkerThread(object targetInstance)
        {
            var target = targetInstance as TargetBase;
            target.ProcessLogQueue();
        }

        #endregion Log Queue Timer
    }
}