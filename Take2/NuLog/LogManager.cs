/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;
using System.Diagnostics;

namespace NuLog
{
    /// <summary>
    /// The log manager ties it all together. This is the "single point of entry" for applications
    /// leveraging NuLog, such as getting a logger instance, and is where the standard behavior of
    /// NuLog can be overridden.
    /// </summary>
    public static class LogManager
    {
        /// <summary>
        /// The logger factory behind this log manager. Implemented with a singleton pattern.
        /// </summary>
        private static ILoggerFactory LoggerFactory;

        /// <summary>
        /// Sets the factory for this log manager to use in creating new loggers.
        /// </summary>
        /// <param name="loggerFactory">The logger factory to use.</param>
        public static void SetFactory(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
        }

        /// <summary>
        /// Gets the logger for the calling class.
        /// </summary>
        public static ILogger GetLogger(IMetaDataProvider metaDataProvider = null, params string[] defaultTags)
        {
            var classTag = GetClassTag();
            return LoggerFactory.GetLogger(metaDataProvider, GetDefaultTags(defaultTags, classTag));
        }

        /// <summary>
        /// Uses reflection to get the class tag of the calling class
        /// </summary>
        private static string GetClassTag()
        {
            var method = new StackFrame(2).GetMethod();
            return method.ReflectedType.FullName;
        }

        /// <summary>
        /// Merges the class tag into the given tags, returning the resulting set.
        /// </summary>
        private static IEnumerable<string> GetDefaultTags(IEnumerable<string> givenTags, string classTag)
        {
            var hashSet = new HashSet<string>();

            if (givenTags != null)
            {
                foreach (var tag in givenTags)
                {
                    hashSet.Add(tag);
                }
            }

            hashSet.Add(classTag);

            return hashSet;
        }
    }
}