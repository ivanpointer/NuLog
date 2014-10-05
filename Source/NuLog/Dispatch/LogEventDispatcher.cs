using NuLog.Configuration;
using NuLog.MetaData;
using NuLog.Targets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NuLog.Dispatch
{
    public class LogEventDispatcher : IConfigObserver
    {
        public const int DefaultShutdownTimeout = 30000; //30 seconds
        private const string FailedToLogMessage = "Failed to log the message \"{0}\" because an exception occured: \"{1}\"";

        private static readonly object LoggingLock = new object();

        public TagKeeper TagKeeper { get; private set; }
        private TargetKeeper TargetKeeper { get; set; }
        public RuleKeeper RuleKeeper { get; private set; }

        public TargetBase RootTarget { get; set; }

        public bool Debug { get; set; }

        private IDictionary<string, ICollection<TargetBase>> TargetDictionary { get; set; }

        internal ConcurrentQueue<Action> ActionQueue { get; set; }
        internal ConcurrentQueue<LogEvent> LogQueue { get; set; }

        internal Thread _queueWorkerThread;
        internal bool _doShutdownThread;
        internal bool _isThreadShutdown;
        protected bool DoShutdown
        {
            get
            {
                return _doShutdownThread;
            }
        }
        public bool IsShutdown
        {
            get
            {
                return _isThreadShutdown;
            }
        }

        private Action<Exception, string> ExceptionHandler { get; set; }

        [ImportMany(typeof(IMetaDataProvider))]
        private IEnumerable<IMetaDataProvider> StaticMetaDataProviders { get; set; }
        private bool StaticMetaDataProvidersLoaded { get; set; }

        public LogEventDispatcher(LoggingConfig initialConfig, Action<Exception, string> exceptionHandler = null)
        {
            ExceptionHandler = exceptionHandler;

            TagKeeper = new TagKeeper();
            TargetKeeper = new TargetKeeper();
            RuleKeeper = new RuleKeeper(TagKeeper);
            TargetDictionary = new Dictionary<string, ICollection<TargetBase>>();

            ActionQueue = new ConcurrentQueue<Action>();
            LogQueue = new ConcurrentQueue<LogEvent>();

            _doShutdownThread = false;
            _isThreadShutdown = false;

            _queueWorkerThread = new Thread(new ThreadStart(this.QueueWorkerThread))
            {
                IsBackground = true
            };

            _queueWorkerThread.Start();

            InitializeStaticMetaDataProviders();

            NewConfig(initialConfig);
        }

        public void NotifyNewConfig(LoggingConfig loggingConfig)
        {
            NewConfig(loggingConfig);
        }

        public bool Shutdown(int timeout = DefaultShutdownTimeout)
        {
            lock (LoggingLock)
            {
                bool result = false;
                if (_queueWorkerThread != null && _queueWorkerThread.IsAlive)
                {
                    _doShutdownThread = true;

                    Trace.WriteLine("Shutting down dispatcher, waiting for all log evbents to flush");

                    var sw = new Stopwatch();
                    sw.Start();
                    while (_queueWorkerThread.IsAlive && sw.ElapsedMilliseconds <= timeout)
                    {
                        if (IsShutdown)
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

                TargetKeeper.Shutdown();

                return result;
            }
        }

        public void Log(LogEvent logEvent)
        {
            LogQueue.Enqueue(logEvent);
        }

        public void Enqueue(Action action)
        {
            ActionQueue.Enqueue(action);
        }

        private void QueueWorkerThread()
        {
            Action action;
            LogEvent logEvent;

            while (!DoShutdown)
            {
                while (ActionQueue.IsEmpty == false)
                {
                    if (ActionQueue.TryDequeue(out action))
                    {
                        action();
                    }
                }

                while (LogQueue.IsEmpty == false)
                {
                    if (LogQueue.TryDequeue(out logEvent))
                    {
                        ExecuteStaticMetaDataProviders(logEvent);

                        var targets = GetTargetsForTags(logEvent.Tags);

                        if (targets != null)
                            foreach (var target in targets)
                                target.Enqueue(logEvent);
                    }
                }

                Thread.Yield();
            }

            _isThreadShutdown = true;
        }

        public void LogNow(LogEvent logEvent)
        {
            ExecuteStaticMetaDataProviders(logEvent);

            var targets = GetTargetsForTags(logEvent.Tags);

            if (targets == null) return;

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

        private void NewConfig(LoggingConfig loggingConfig)
        {
            Debug = loggingConfig.Debug;

            lock (LoggingLock)
            {
                TagKeeper.NotifyNewConfig(loggingConfig);
                TargetKeeper.NotifyNewConfig(loggingConfig);
                RuleKeeper.NotifyNewConfig(loggingConfig);
                TargetDictionary.Clear();
            }
        }

        private ICollection<TargetBase> GetTargetsForTags(ICollection<string> tags)
        {
            // This was refactored using ReSharper.  I don't like how unclean the code looks now,
            //   but my larger concern here is performance.  I am trying to get this framework
            //   working faster than log4net
            lock (LoggingLock)
            {
                var targetNames = RuleKeeper.GetTargetsForTags(tags);
                if (targetNames == null || targetNames.Count <= 0) return null;

                var targetNamesKey = FlattenTargetNames(tags);

                if (TargetDictionary.ContainsKey(targetNamesKey)) return TargetDictionary[targetNamesKey];

                TargetDictionary[targetNamesKey] = TargetKeeper.GetTargets(targetNames);

                return TargetDictionary[targetNamesKey];
            }
        }

        private static string FlattenTargetNames(ICollection<string> targetNames)
        {
            return String.Join(",", targetNames.ToArray());
        }

        #region Helpers

        public void HandleException(Exception e, LogEvent logEventInfo = null)
        {
            string message = String.Format(FailedToLogMessage, logEventInfo != null ? logEventInfo.Message : "[null]", e);

            if (ExceptionHandler != null)
                ExceptionHandler.Invoke(e, message);
            else
                Trace.WriteLine(message);
        }

        private void InitializeStaticMetaDataProviders()
        {
            bool loaded = false;
            try
            {
                var aggregateCatalogue = new AggregateCatalog();
                aggregateCatalogue.Catalogs.Add(new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));

                var container = new CompositionContainer(aggregateCatalogue);
                container.ComposeParts(this);

                loaded = true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(String.Format("Failure loading static metadata providers: {0}:\r\n{1}", ex.Message, ex.StackTrace), Constants.TraceLogger.ConfigCategory);
            }

            StaticMetaDataProvidersLoaded = loaded && StaticMetaDataProviders != null && StaticMetaDataProviders.Any();
        }

        private void ExecuteStaticMetaDataProviders(LogEvent logEvent)
        {
            if (StaticMetaDataProvidersLoaded)
            {
                foreach (var staticMetaDataProvider in StaticMetaDataProviders)
                {
                    try
                    {
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
                        Trace.WriteLine(String.Format("Failure executing static metadata provider {0}: {1}:\r\n{2}", staticMetaDataProvider.GetType().FullName, ex.Message, ex.StackTrace), Constants.TraceLogger.ConfigCategory);
                    }
                }
            }
        }

        #endregion

    }
}
