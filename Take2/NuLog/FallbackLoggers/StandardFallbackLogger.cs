/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Dispatchers;
using NuLog.LogEvents;
using System;
using System.IO;

namespace NuLog.FallbackLoggers
{
    /// <summary>
    /// The standard implementation of the fallback logger.
    /// </summary>
    public class StandardFallbackLogger : IFallbackLogger
    {
        /// <summary>
        /// The path of the file being logged to.
        /// </summary>
        private readonly string filePath;

        public StandardFallbackLogger(string filePath)
        {
            this.filePath = filePath;
        }

        public void Log(Exception exception, ITarget target, LogEvent logEvent)
        {
            var message = string.Format("{0:yyyy-MM-dd hh:mm:ss.fff} | Failure writing to target \"{1}\" of type {2} | {3} | {4} | {5} | {6}", DateTime.Now, target.Name, target.GetType().FullName, JoinTags(logEvent), logEvent.Message, GetExceptionMessage(logEvent), exception);
            File.AppendAllText(this.filePath, message);
        }

        private static string JoinTags(LogEvent logEvent)
        {
            return logEvent.Tags != null ? string.Join(",", logEvent.Tags) : string.Empty;
        }

        private static string GetExceptionMessage(LogEvent logEvent)
        {
            return string.Format("LogEvent Exception: \"{0}\"", logEvent.Exception != null ? logEvent.Exception.Message : string.Empty);
        }
    }
}