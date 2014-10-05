using NuLog.Configuration;
using NuLog.Dispatch;
using NuLog.Logger;
using NuLog.MetaData;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NuLog
{
    public class LoggerFactory
    {
        private static readonly object _factoryLock = new object();
        private static readonly Lazy<LoggerFactory> Instance = new Lazy<LoggerFactory>(() =>
        { return new LoggerFactory(); });

        [ImportMany(typeof(ILoggingConfigExtender))]
        private IEnumerable<ILoggingConfigExtender> ConfigExtenders { get; set; }

        private LoggingConfig LoggingConfig { get; set; }
        private LogEventDispatcher LogEventDispatcher { get; set; }
        private bool Initialized { get; set; }
        private IDictionary<string, LoggerBase> NamedLoggers { get; set; }

        private Action<Exception, string> ExceptionHandler { get; set; }

        private LoggerFactory()
        {
            NamedLoggers = new Dictionary<string, LoggerBase>();
        }

        public static void Initialize(ILoggingConfigBuilder configBuilder, LogEventDispatcher dispatcher = null, Action<Exception, string> exceptionHandler = null, bool force = true)
        {
            try
            {
                var config = configBuilder.Build();
                Initialize(config, dispatcher, exceptionHandler, force);
            }
            catch
            {
                Trace.WriteLine(String.Format(Constants.TraceLogger.FailedConfigBuilderMessage, configBuilder == null ? "NULL" : configBuilder.GetType().FullName));
                Initialize(config: null, dispatcher: dispatcher, force: force);
            }
        }

        public static void Initialize(LoggingConfig config = null, LogEventDispatcher dispatcher = null, Action<Exception, string> exceptionHandler = null, bool force = true)
        {
            lock (_factoryLock)
            {
                if (force || !Instance.Value.Initialized)
                {
                    if (Instance.Value.Initialized)
                        Instance.Value.LoggingConfig.Shutdown();

                    Instance.Value.ExceptionHandler = exceptionHandler;

                    try
                    {
                        Instance.Value.LoggingConfig = config != null
                        ? config
                        : new LoggingConfig();
                    }
                    catch (Exception e)
                    {
                        // Report the exception
                        Trace.WriteLine(Constants.TraceLogger.ConfigurationFailedUsingDefaultsMessage);
                        Trace.WriteLine(String.Format("Failure loading config: {0}:\r\n{1}", e.Message, e.StackTrace), Constants.TraceLogger.ConfigCategory);

                        // Load the default config to get us off the ground!!
                        Instance.Value.LoggingConfig = new LoggingConfig(loadConfig: false);
                    }

                    // Update the config
                    Instance.Value.ExecuteConfigExtenders();

                    // Setup the dispatcher
                    Instance.Value.LogEventDispatcher = dispatcher != null
                        ? dispatcher
                        : new LogEventDispatcher(Instance.Value.LoggingConfig, Instance.Value.ExceptionHandler);
                    Instance.Value.LoggingConfig.RegisterObserver(Instance.Value.LogEventDispatcher);

                    // Notify of a new config
                    Instance.Value.LoggingConfig.NotifyObservers();

                    Instance.Value.Initialized = true;
                }
            }
        }

        public static void Initialize(string configFile, LogEventDispatcher dispatcher = null, Action<Exception, string> exceptionHandler = null, bool force = true)
        {
            lock (_factoryLock)
            {
                if (force || !Instance.Value.Initialized)
                {
                    if (Instance.Value.Initialized)
                        Instance.Value.LoggingConfig.Shutdown();

                    try
                    {
                        Instance.Value.LoggingConfig = new LoggingConfig(configFile);
                    }
                    catch (Exception e)
                    {
                        // Report the exception
                        Trace.WriteLine(e, Constants.TraceLogger.ConfigCategory);

                        // Load the default config to get us off the ground!!
                        Instance.Value.LoggingConfig = new LoggingConfig();
                    }

                    if (Instance.Value.LogEventDispatcher == null)
                    {
                        Instance.Value.LogEventDispatcher = new LogEventDispatcher(Instance.Value.LoggingConfig, Instance.Value.ExceptionHandler);
                    } else {
                        Instance.Value.LogEventDispatcher.NewConfig(Instance.Value.LoggingConfig);
                    }

                    Instance.Value.LoggingConfig.RegisterObserver(Instance.Value.LogEventDispatcher);

                    Instance.Value.Initialized = true;
                }
            }
        }

        public static LoggerBase GetLogger(string loggerName = null, params string[] defaultTags)
        {
            Initialize(force: false);

            var stackFrame = new StackFrame(1);
            string declaringFullName = stackFrame.GetMethod().DeclaringType.FullName;
            if (String.IsNullOrEmpty(loggerName))
                loggerName = declaringFullName;

            lock (_factoryLock)
            {
                if (Instance.Value.NamedLoggers.ContainsKey(loggerName) == false)
                {
                    var tags = new List<string>();
                    foreach (var defaultTag in defaultTags)
                        if (!tags.Contains(defaultTag))
                            tags.Add(defaultTag);

                    if (tags.Contains(declaringFullName) == false)
                        tags.Add(declaringFullName);

                    if (tags.Contains(loggerName) == false)
                        tags.Add(loggerName);

                    var logger = new DefaultLogger(Instance.Value.LogEventDispatcher, tags);
                    Instance.Value.NamedLoggers[loggerName] = logger;
                    return logger;
                }
                else
                {
                    return Instance.Value.NamedLoggers[loggerName];
                }
            }
        }

        public static LoggerBase GetLogger(IMetaDataProvider metaDataProvider, params string[] defaultTags)
        {
            Initialize(force: false);

            var stackFrame = new StackFrame(1);
            string declaringFullName = stackFrame.GetMethod().DeclaringType.FullName;

            var tags = new List<string>();
            foreach (var defaultTag in defaultTags)
                if (!tags.Contains(defaultTag))
                    tags.Add(defaultTag);

            if (tags.Contains(declaringFullName) == false)
                tags.Add(declaringFullName);

            return new DefaultLogger(Instance.Value.LogEventDispatcher, tags)
            {
                MetaDataProvider = metaDataProvider
            };
        }

        public static void Shutdown()
        {
            lock (_factoryLock)
            {
                if (Instance.Value != null && Instance.Value.Initialized)
                {
                    if (Instance.Value.LogEventDispatcher != null)
                    {
                        Instance.Value.LogEventDispatcher.Shutdown();
                    }
                }
            }
        }

        #region Helpers

        private void ExecuteConfigExtenders()
        {
            bool loaded = false;
            try
            {
                AggregateCatalog aggregateCatalogue = new AggregateCatalog();
                aggregateCatalogue.Catalogs.Add(new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));

                CompositionContainer container = new CompositionContainer(aggregateCatalogue);
                container.ComposeParts(this);

                loaded = true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(String.Format("Failure loading configuration extenders: {1}:\r\n{2}", ex.Message, ex.StackTrace), Constants.TraceLogger.ConfigCategory);
            }

            if (loaded && Instance.Value.ConfigExtenders != null)
                foreach (var configExtender in Instance.Value.ConfigExtenders)
                {
                    try
                    {
                        configExtender.UpdateConfig(Instance.Value.LoggingConfig);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(String.Format("Failure executing config extender {0}: {1}:\r\n{2}", configExtender.GetType().FullName, ex.Message, ex.StackTrace), Constants.TraceLogger.ConfigCategory);
                    }
                }
        }

        #endregion

    }
}
