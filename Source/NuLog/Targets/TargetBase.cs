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
    public abstract class TargetBase
    {
        private const string DefaultName = "TargetBase";
        private const bool DefaultSynchronousSetting = false;
        public const int DefaultShutdownTimeout = 10000; //10 seconds

        public string Name { get; protected set; }
        public bool Synchronous { get; protected set; }
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

        protected LogEventDispatcher Dispatcher { get; private set; }
        protected TargetConfig BaseTargetConfig { get; private set; }

        protected ConcurrentQueue<LogEvent> LogQueue { get; set; }

        internal Thread _queueWorkerThread;
        protected bool DoShutdownThread { get; set; }
        protected bool IsThreadShutdown { get; set; }

        public TargetBase()
        {
            InitializeInstance(null);
        }

        public TargetBase(LogEventDispatcher dispatcher)
        {
            InitializeInstance(dispatcher);
        }

        private void InitializeInstance(LogEventDispatcher dispatcher)
        {
            Dispatcher = dispatcher;
            LogQueue = new ConcurrentQueue<LogEvent>();

            DoShutdownThread = false;
            IsThreadShutdown = false;

            _queueWorkerThread = new Thread(new ThreadStart(QueueWorkerThread))
            {
                IsBackground = true
            };

            _queueWorkerThread.Start();
        }

        public void Enqueue(LogEvent logEvent)
        {
            if (DoShutdown)
                throw new InvalidOperationException("Cannot enqueue new log event when shutting down target");

            if (!Synchronous)
                LogQueue.Enqueue(logEvent);
            else
                Log(logEvent);
        }

        protected void QueueWorkerThread()
        {
            while (!DoShutdown)
            {
                if (LogQueue.IsEmpty == false)
                    ProcessLogQueue(LogQueue, Dispatcher);

                Thread.Yield();
            }

            IsThreadShutdown = true;
        }

        protected virtual void ProcessLogQueue(ConcurrentQueue<LogEvent> logQueue, LogEventDispatcher dispatcher)
        {
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

        public virtual void Initialize(TargetConfig targetConfig, bool? synchronous = null)
        {
            BaseTargetConfig = targetConfig;

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

            _queueWorkerThread.Name = String.Format("{0} Queue Worker Thread", Name);
        }

        public virtual void NotifyNewConfig(TargetConfig targetConfig)
        {
            Initialize(targetConfig);
        }

        public abstract void Log(LogEvent logEvent);

        public virtual bool Shutdown(int timeout = DefaultShutdownTimeout)
        {
            if (_queueWorkerThread != null && _queueWorkerThread.IsAlive)
            {
                DoShutdownThread = true;

                Trace.WriteLine(String.Format("Shutting down target \"{0}\", waiting for log queue to flush", Name));

                var sw = new Stopwatch();
                sw.Start();
                while (_queueWorkerThread.IsAlive && sw.ElapsedMilliseconds <= timeout)
                    if (IsShutdown)
                        return true;
            }
            else
            {
                return true;
            }

            return false;
        }
    }
}
