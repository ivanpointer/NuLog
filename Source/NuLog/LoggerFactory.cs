/*
 * Author: Ivan Pointer (ivan@pointerplace.us)
 * Date: 10/5/2014
 * Updated 11/16/2014: Ivan Pointer: Added the ability to set a default configuration builder using MEF
 * Updated 11/13/2014: Ivan Pointer: Added the ability to load configuration extenders explicitly, wihtout using MEF
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Configuration;
using NuLog.Dispatch;
using NuLog.Extenders;
using NuLog.Loggers;
using NuLog.MetaData;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NuLog
{
    /// <summary>
    /// The factory for building and providing all of the pieces for the NuLogging framework.
    /// The factory ties the four other major pieces of the framework together:
    ///   configuration, the dispatcher, the targets and loggers
    /// </summary>
    public class LoggerFactory
    {
        #region Constants

        // Messages
        private const string FailedConfigBuilderMessage = "Failed to configure using the provided config builder {0}, cause: {1}";
        private const string ConfigurationFailedUsingDefaultsMessage = "Configuration failed, using default configuration";
        private const string ConfigLoadFailureMessage = "Failure loading config: {0}:\r\n{1}";
        private const string ShutdownDispatcherFailureMessage = "Failed to shutdown the existing dispatcher due to error {0}";
        private const string ConfigExtendersLoadFailedMessage = "Failure loading configuration extenders: {1}:\r\n{2}";
        private const string TypeNotFoundMessage = "Type not found for \"{0}\"";
        private const string FailedToLogMessage = "Failed to log the message \"{0}\" because an exception occured: \"{1}\"";
        private const string ExceptionInNuLogMessage = "Failure in NuLog \"{0}\"";
        private const string LoadMEFFailureMessage = "Failed to load MEF components, cause: {0}";
        private const string ExtenderTypeNullMessage = "Extender type is null or empty";
        private const string ExtenderTypeNotFoundMessage = "Extender for type \"{0}\" not found";

        // Functional Values
        private const string TraceConfigCategory = "config";
        private const string NullString = "NULL";
        private const int DefaultExtenderShutdownTimeout = 5000;

        #endregion

        #region Members and Constructors

        // Used for general locking on the factory level
        private static readonly object _factoryLock = new object();
        // Setting up the lazy singleton pattern
        private static readonly Lazy<LoggerFactory> Instance = new Lazy<LoggerFactory>(() =>
        { return new LoggerFactory(); });

        /// <summary>
        /// The framework can be extended with "ConfigExtenders".  These are used to,
        /// well, extend the configuration.
        /// </summary>
        [ImportMany(typeof(ILoggingConfigExtender))]
        private IEnumerable<ILoggingConfigExtender> MEFConfigExtenders { get; set; }
        private IList<ILoggingConfigExtender> ConfigExtenders { get; set; }

        /// <summary>
        /// The extenders for extending the logging framework
        /// </summary>
        private IList<IExtender> Extenders { get; set; }

        /// <summary>
        /// The default configuration builder for the factory.
        /// It is intended to be loaded using MEF.  See the article
        /// https://github.com/ivanpointer/NuLog/wiki/7.1-Creating-a-Custom-Configuration-Builder
        /// for more information
        /// </summary>
        [Import(typeof(ILoggingConfigBuilder), AllowDefault = true)]
        private ILoggingConfigBuilder DefaultConfigBuilder { get; set; }

        /// <summary>
        /// The single primary configuration used for all of the components
        /// of the framework
        /// </summary>
        private LoggingConfig LoggingConfig { get; set; }

        /// <summary>
        /// The log event dispatcher, or "dispatcher".
        /// The dispatcher is responsible for managing the targets,
        /// and for dispatching log events from the loggers to
        /// the various targets.
        /// </summary>
        private LogEventDispatcher LogEventDispatcher { get; set; }

        /// <summary>
        /// Whether or not the factory is initialized.  This is used
        /// to prevent loading the configuration multiple times,
        /// unless it is strictly intentional (I.E. a force tag on
        /// the Initialize function)
        /// </summary>
        private bool Initialized { get; set; }

        /// <summary>
        /// A cache of named loggers.  Loggers by name are only
        /// initialized once, allowing for developers to call the
        /// factory for a logger instance multiple times without
        /// provisioning more than a single logger multiple times
        /// </summary>
        private IDictionary<string, LoggerBase> NamedLoggers { get; set; }

        /// <summary>
        /// Used for debugging purposes, an exception handler is used
        /// to intercept exceptions occuring inside of the logging framework.
        /// If not supplied, exceptions within the framework are sent
        /// to Trace.
        /// </summary>
        private Action<Exception, string> ExceptionHandler { get; set; }

        /// <summary>
        /// The default constructor for the factory.  Is marked private
        /// for the singleton pattern.
        /// </summary>
        private LoggerFactory()
        {
            NamedLoggers = new Dictionary<string, LoggerBase>();
        }

        #endregion

        #region Initialization Methods

        /// <summary>
        /// Initializes the logging framework using a provided ILoggingConfigBuilder
        /// </summary>
        /// <param name="configBuilder">The config builder to use to initialize the logging framework</param>
        /// <param name="dispatcher">An alternate dispatcher to use for the logging framework</param>
        /// <param name="exceptionHandler">An exception handler to use for debugging purposes</param>
        /// <param name="force">Whether or not to force the re-configuration if the framework has alredy been intiaizlied</param>
        public static void Initialize(ILoggingConfigBuilder configBuilder, LogEventDispatcher dispatcher = null, Action<Exception, string> exceptionHandler = null, bool force = true)
        {
            LoggingConfig config = null;

            // Try to load the config from the given builder
            try
            {
                config = configBuilder.Build();
            }
            catch (Exception cause)
            {
                // Report the exception
                string message = String.Format(FailedConfigBuilderMessage, configBuilder == null ? NullString : configBuilder.GetType().FullName, cause);
                if (exceptionHandler != null)
                {
                    exceptionHandler(cause, message);
                }
                else
                {
                    Trace.WriteLine(message, TraceConfigCategory);
                }
            }

            Initialize(config, dispatcher, exceptionHandler, force);
        }

        /// <summary>
        /// Initializes the logging framework using a file
        /// </summary>
        /// <param name="configFile">The path to the JSON configuration file to use</param>
        /// <param name="dispatcher">An alternate dispatcher to use for the logging framework</param>
        /// <param name="exceptionHandler">An exception handler to use for debugging purposes</param>
        /// <param name="force">Whether or not to force the re-configuration if the framework has alredy been intiaizlied</param>
        public static void Initialize(string configFile, LogEventDispatcher dispatcher = null, Action<Exception, string> exceptionHandler = null, bool force = true)
        {
            LoggingConfig config = null;

            // Try to load the config from the given file
            try
            {
                config = new LoggingConfig(configFile);
            }
            catch (Exception cause)
            {
                // Report the exception
                if (exceptionHandler != null)
                {
                    exceptionHandler(cause, ConfigurationFailedUsingDefaultsMessage);
                }
                else
                {
                    Trace.WriteLine(cause, TraceConfigCategory);
                }
            }

            Initialize(config, dispatcher, exceptionHandler, force);
        }

        /// <summary>
        /// Initializes the logging framework using a provided LoggingConfig
        /// </summary>
        /// <param name="config">The LoggingConfig to use to initialize the logging framework</param>
        /// <param name="dispatcher">An alternate dispatcher to use for the logging framework</param>
        /// <param name="exceptionHandler">An exception handler to use for debugging purposes</param>
        /// <param name="force">Whether or not to force the re-configuration if the framework has alredy been intiaizlied</param>
        public static void Initialize(LoggingConfig config = null, LogEventDispatcher dispatcher = null, Action<Exception, string> exceptionHandler = null, bool force = true)
        {
            lock (_factoryLock)
            {
                var instance = Instance.Value;

                // We only want to configure if we haven't yet, or if it is marked to override (force) the initialization
                if (force || !instance.Initialized)
                {
                    // Shutdown existing config if it exists
                    if (instance.Initialized)
                        instance.LoggingConfig.Shutdown();

                    // Wire up the exception handler
                    instance.ExceptionHandler = exceptionHandler;

                    // Setup the logging config
                    instance.InitializeConfig(config);

                    // Load and initialize the extenders, allowing them to update the config
                    //  before we start
                    instance.InitializeExtenders();

                    // Setup the dispatcher
                    instance.InitializeDispatcher(dispatcher);

                    // Start the extenders
                    instance.StartExtenders(instance.LogEventDispatcher);
                    
                    // Mark us as initialized
                    instance.Initialized = true;
                }
            }
        }

        /// <summary>
        /// Load in the MEF extension components
        /// </summary>
        private void InitializeMEF()
        {
            try
            {
                AggregateCatalog aggregateCatalogue = new AggregateCatalog();
                aggregateCatalogue.Catalogs.Add(new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));

                CompositionContainer container = new CompositionContainer(aggregateCatalogue);
                container.ComposeParts(this);
            }
            catch (Exception cause)
            {
                Trace.WriteLine(String.Format(LoadMEFFailureMessage, cause), TraceConfigCategory);
            }
        }

        /// <summary>
        /// An internal function for initializing the logging config for the factory
        /// </summary>
        /// <param name="config">The config to use for initialization. If none(null) provided, the default logging config will be loaded (from NuLog.json)</param>
        private void InitializeConfig(LoggingConfig config = null)
        {
            // The configuration needs to be shutdown because it has a list of observers
            //  and may have a file-watcher implemented that needs to be disabled/shutdown
            if (Initialized)
                LoggingConfig.Shutdown();

            // Make sure that the MEF extensions are loaded in
            InitializeMEF();

            try
            {
                // Load using the supplied config,
                //  or the default config from the default config builder
                //  or the default config as defined by LoggingConfig
                LoggingConfig = config != null
                    ? config
                    : DefaultConfigBuilder != null
                        ? DefaultConfigBuilder.Build()
                        : new LoggingConfig();
            }
            catch (Exception cause)
            {
                // Report the exception
                Trace.WriteLine(ConfigurationFailedUsingDefaultsMessage);
                Trace.WriteLine(String.Format(ConfigLoadFailureMessage, cause.Message, cause.StackTrace), TraceConfigCategory);

                // Load the default config to get us off the ground!!
                LoggingConfig = new LoggingConfig(loadConfig: false);
            }

            // Load the config extenders
            LoadConfigExtenders();

            // Execute the config extenders against the configuration
            ExecuteConfigExtenders();
        }

        /// <summary>
        /// Loads the extenders and updates the config
        /// </summary>
        private void InitializeExtenders()
        {
            LoadAndInitializeExtenders();

            ExecuteExtendersUpdateConfig();
        }

        // Loads the extenders
        private void LoadAndInitializeExtenders()
        {
            // First, shutdown any extenders we may already have
            ShutdownExtenders();

            Extenders = new List<IExtender>();

            // Check to see if we have any extenders configured
            if (LoggingConfig.Extenders != null && LoggingConfig.Extenders.Count > 0)
            {
                Type extenderType;
                ConstructorInfo constructorInfo;
                IExtender extender;

                // Iterate over each config and try to build its instance
                foreach (var extenderConfig in LoggingConfig.Extenders)
                {
                    try
                    {
                        // Lookup the extender's type
                        if (String.IsNullOrEmpty(extenderConfig.Type))
                            throw new LoggingException(ExtenderTypeNullMessage);

                        extenderType = Type.GetType(extenderConfig.Type);

                        if (extenderType == null)
                            throw new LoggingException(String.Format(ExtenderTypeNotFoundMessage, extenderConfig.Type));

                        // Build the extender
                        constructorInfo = extenderType.GetConstructor(new Type[] { });
                        extender = (IExtender)constructorInfo.Invoke(null);
                        extender.Initialize(extenderConfig, LoggingConfig);
                        Extenders.Add(extender);
                    }
                    catch (Exception exception)
                    {
                        // Handle the failure
                        HandleException(exception);
                    }
                }
            }
        }

        // Update the configuration using the extenders
        private void ExecuteExtendersUpdateConfig()
        {
            foreach (var extender in Extenders)
            {
                try
                {
                    extender.UpdateConfig(LoggingConfig);
                }
                catch (Exception cause)
                {
                    // Handle the failure
                    HandleException(cause);
                }
            }
        }

        // Initialize the extenders
        private void StartExtenders(LogEventDispatcher dispatcher)
        {
            foreach (var extender in Extenders)
            {
                try
                {
                    extender.Startup(dispatcher);
                }
                catch (Exception cause)
                {
                    // Handle the failure
                    HandleException(cause);
                }
            }
        }

        // Shutdown the extenders
        private void ShutdownExtenders()
        {
            if(Extenders != null)
                foreach(var extender in Extenders)
                {
                    try
                    {
                        extender.Shutdown(DefaultExtenderShutdownTimeout);
                    }
                    catch (Exception cause)
                    {
                        // Handle the failure
                        HandleException(cause);
                    }
                }
        }

        /// <summary>
        /// An internal function for initializing the dispatcher
        /// </summary>
        /// <param name="dispatcher">An alternate dispatcher to use.  If none(null) provided, will use the default dispatcher</param>
        private void InitializeDispatcher(LogEventDispatcher dispatcher = null)
        {
            // If we've been provided an alternate dispatcher, we laready have one
            //  and we aren't already using the new alternate
            if (dispatcher != null && LogEventDispatcher != null && LogEventDispatcher != dispatcher)
            {
                try
                {
                    // Shut down our current dispatcher
                    LogEventDispatcher.Shutdown();
                }
                catch (Exception e)
                {
                    // We failed to shutdown the existing dispatcher,  log the error and continue
                    Trace.WriteLine(String.Format(ShutdownDispatcherFailureMessage, e), TraceConfigCategory);
                }
            }

            // If we've been provided with an alternate dispatcher
            if (dispatcher != null)
            {
                // Wire it up
                LogEventDispatcher = dispatcher;
                LogEventDispatcher.NotifyNewConfig(LoggingConfig);
                LoggingConfig.RegisterObserver(LogEventDispatcher);
            }
            else
            {
                // If we don't yet have a dispatcher
                if (LogEventDispatcher == null)
                {
                    // Wire up a new default one
                    LogEventDispatcher = new LogEventDispatcher(LoggingConfig, ExceptionHandler);
                    LoggingConfig.RegisterObserver(LogEventDispatcher);
                }
                else
                {
                    // Notify of new config if we already have a dispatcher
                    LogEventDispatcher.NotifyNewConfig(LoggingConfig);
                }
            }
        }

        // Loads in the configuration extenders from MEF and from the set configuration
        private void LoadConfigExtenders()
        {
            ConfigExtenders = new List<ILoggingConfigExtender>();

            // Load MEF Extenders
            try
            {
                if(MEFConfigExtenders != null)
                    foreach (var extender in MEFConfigExtenders)
                        ConfigExtenders.Add(extender);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(String.Format(ConfigExtendersLoadFailedMessage, ex.Message, ex.StackTrace), TraceConfigCategory);
            }

            // Iterate over the list of static meta data providers and use
            //  reflection to instantiate them
            if (LoggingConfig.ConfigurationExtenders != null && LoggingConfig.ConfigurationExtenders.Count > 0)
            {
                Type extenderType;
                ConstructorInfo constructorInfo;
                ILoggingConfigExtender configExtender;

                foreach (string configurationExtender in LoggingConfig.ConfigurationExtenders)
                {
                    try
                    {
                        // Pull the type and and constructor for the extender, by name
                        extenderType = Type.GetType(configurationExtender);
                        if (extenderType != null)
                        {
                            constructorInfo = extenderType.GetConstructor(new Type[] { });
                            configExtender = (ILoggingConfigExtender)constructorInfo.Invoke(null);
                            ConfigExtenders.Add(configExtender);
                        }
                        else
                        {
                            throw new LoggingException(String.Format(TypeNotFoundMessage, configurationExtender));
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

        /// <summary>
        /// Executes any loaded configuration extenders.
        /// These are not used in the base implementation of the framework,
        /// but are provided for the purposes of extending the framework.
        /// For example, this is used by the legacy extension of the framework,
        /// adding the TagGroups to emulate the different log levels of a
        /// traditional logging framework.
        /// </summary>
        private void ExecuteConfigExtenders()
        {
            foreach (var configExtender in ConfigExtenders)
            {
                try
                {
                    configExtender.UpdateConfig(LoggingConfig);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(String.Format(ConfigExtendersLoadFailedMessage, configExtender.GetType().FullName, ex.Message, ex.StackTrace), TraceConfigCategory);
                }
            }
        }

        /// <summary>
        /// Shuts down the logging framework.
        /// The logging framework will come down cleanly when an application
        /// exits anyways, so this is simply provided as a convenience if
        /// the case arises that a developer needs to shut down logging
        /// separately from the implementing application.
        /// 
        /// As a caveat, if the logging framework hasn't been initialized
        /// yet, this will startup the logging framework before shutting
        /// it down.
        /// </summary>
        public static void Shutdown()
        {
            lock (_factoryLock)
            {
                var instance = Instance.Value;

                // Bring the extenders down first, their shutdown
                //  process may depend on the dispatcher
                instance.ShutdownExtenders();

                // Bring the dispatcher down last
                if (instance.LogEventDispatcher != null)
                    instance.LogEventDispatcher.Shutdown();
            }
        }

        #endregion

        #region Loggers

        /// <summary>
        /// Gets a logger using an optionally provided logger name and assigning the optional default tags
        /// </summary>
        /// <param name="loggerName">The name to assign to the logger.  This is used to decrease the number of logger instances initialized.</param>
        /// <param name="defaultTags">A list of default tags to assign to the logger.  The default tags on a logger will be assigned to all log events passing through the logger.</param>
        /// <returns></returns>
        public static LoggerBase GetLogger(string loggerName = null, params string[] defaultTags)
        {
            // Make sure the framework is initialized
            //  This is what allows for us to include a NuLog.json file
            //  and take off running with LoggerFactory.GetLogger without
            //  having to initialize first.
            Initialize(force: false);

            // Get a hold of the stackframe of the calling method
            //  and use it to determine the full name of the
            //  class that requested the logger
            var stackFrame = new StackFrame(1);
            string reqClassFullName = stackFrame.GetMethod().DeclaringType.FullName;

            // Make sure that a name is assigned to the logger;
            //  use the requesting class' full name as the logger
            //  name if no name is provided
            if (String.IsNullOrEmpty(loggerName))
                loggerName = reqClassFullName;

            // Synchronization is needed here to ensure that only one logger of the
            //  determined name is instantiated
            lock (_factoryLock)
            {
                var instance = Instance.Value;

                // Get the distinct default tags
                var tags = defaultTags.Distinct().ToList();

                // Make sure that the logger includes the requesting class' full name
                //  as a tag
                if (tags.Contains(reqClassFullName) == false)
                    tags.Add(reqClassFullName);

                // Make sure that the logger includes the name of the logger as a tag
                if (tags.Contains(loggerName) == false)
                    tags.Add(loggerName);

                // Check to see if we need to create a new instance of the logger
                if (instance.NamedLoggers.ContainsKey(loggerName) == false)
                {
                    // Create and return a new instance of the logger
                    //  using the determined default tags

                    var logger = new DefaultLogger(instance.LogEventDispatcher, tags);
                    instance.NamedLoggers[loggerName] = logger;
                    return logger;
                }
                else
                {
                    // We already have an instance of the logger
                    //  Let's make sure that the logger has the default tags
                    //  we have recieved and then return it

                    var logger = instance.NamedLoggers[loggerName];

                    foreach (var tag in tags)
                        if (logger.DefaultTags.Contains(tag) == false)
                            logger.DefaultTags.Add(tag);

                    return logger;
                }
            }
        }

        /// <summary>
        /// Returns a stand-alone instance of a logger with an attached runtime meta data provider.  See the official
        /// documentation for more information on using runtime meta data providers and practical use-cases for them.
        /// </summary>
        /// <param name="metaDataProvider">The meta data provider to attach to the logger</param>
        /// <param name="defaultTags">The default tags to associate with the logger</param>
        /// <returns></returns>
        public static LoggerBase GetLogger(IMetaDataProvider metaDataProvider, params string[] defaultTags)
        {
            // Make sure the framework is initialized
            //  This is what allows for us to include a NuLog.json file
            //  and take off running with LoggerFactory.GetLogger without
            //  having to initialize first.
            Initialize(force: false);

            // Get a hold of the calling method's stack frame
            //  and determine the full name of the requesting
            //  class
            var stackFrame = new StackFrame(1);
            string reqClassFullName = stackFrame.GetMethod().DeclaringType.FullName;

            // Get the distinct default tags
            var tags = defaultTags.Distinct().ToList();

            // Make sure that the requesting class' full name
            //  is included as a default tag for the logger
            if (tags.Contains(reqClassFullName) == false)
                tags.Add(reqClassFullName);

            // Return a new instance of the logger with the associated
            //  runtime meta data provider
            return new DefaultLogger(Instance.Value.LogEventDispatcher, tags)
            {
                MetaDataProvider = metaDataProvider
            };
        }

        #endregion

        #region Helpers

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
                if(logEventInfo == null || logEventInfo.Silent == false)
                    Trace.WriteLine(message);
        }

        #endregion

    }

}