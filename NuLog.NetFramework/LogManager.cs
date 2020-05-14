/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.Factories;
using System.Collections.Generic;
using System.Diagnostics;

namespace NuLog {

    /// <summary>
    /// The log manager ties it all together. This is the "single point of entry" for applications
    /// leveraging NuLog, such as getting a logger instance, and is where the standard behavior of
    /// NuLog can be overridden.
    /// </summary>
    public static class LogManager {

        /// <summary>
        /// A lock for making the log manager more thread safe.
        /// </summary>
        private static readonly object LogManagerLock = new object();

        /// <summary>
        /// The logger factory behind this log manager. Implemented with a singleton pattern.
        /// </summary>
        private static ILoggerFactory _loggerFactory;

        /// <summary>
        /// Sets the factory for this log manager to use in creating new loggers.
        /// </summary>
        /// <param name="loggerFactory">The logger factory to use.</param>
        public static void SetFactory(ILoggerFactory loggerFactory) {
            lock (LogManagerLock) {
                _loggerFactory = loggerFactory;
            }
        }

        /// <summary>
        /// Gets the logger for the calling class.
        /// </summary>
        public static ILogger GetLogger(IMetaDataProvider metaDataProvider = null, params string[] defaultTags) {
            var classTag = GetClassTag();
            var factory = GetLoggerFactory();
            return factory.GetLogger(metaDataProvider, GetDefaultTags(defaultTags, classTag));
        }

        /// <summary>
        /// Shuts down the logging operation. Shuts down the dispatcher, which should flush any
        /// outgoing messages, and throw exceptions on any new messages.
        /// </summary>
        public static void Shutdown() {
            lock (LogManagerLock) {
                if (_loggerFactory != null) {
                    _loggerFactory.Dispose();
                    _loggerFactory = null;
                }
            }
        }

        /// <summary>
        /// Uses reflection to get the class tag of the calling class
        /// </summary>
        private static string GetClassTag() {
            var method = new StackFrame(2).GetMethod();
            return method.ReflectedType.FullName;
        }

        /// <summary>
        /// Merges the class tag into the given tags, returning the resulting set.
        /// </summary>
        private static IEnumerable<string> GetDefaultTags(IEnumerable<string> givenTags, string classTag) {
            var hashSet = new HashSet<string>();

            if (givenTags != null) {
                foreach (var tag in givenTags) {
                    hashSet.Add(tag);
                }
            }

            hashSet.Add(classTag);

            return hashSet;
        }

        private static ILoggerFactory GetLoggerFactory() {
            if (_loggerFactory == null) {
                lock (LogManagerLock) {
                    if (_loggerFactory == null) {
                        var configProvider = new ConfigurationManagerProvider();
                        var config = configProvider.GetConfiguration();
                        _loggerFactory = new StandardLoggerFactory(config);
                    }
                }
            }

            return _loggerFactory;
        }
    }
}