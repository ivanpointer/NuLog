/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Dispatchers;
using NuLog.LogEvents;
using System;

namespace NuLog.FallbackLoggers
{
    /// <summary>
    /// The core/basic functionality of the standard fallback logger implementation.
    /// </summary>
    public abstract class StandardFallbackLoggerBase : IFallbackLogger
    {
        public abstract void Log(Exception exception, ITarget target, ILogEvent logEvent);

        protected virtual string FormatMessage(Exception exception, ITarget target, ILogEvent logEvent)
        {
            return string.Format("{0:yyyy-MM-dd hh:mm:ss.fff} | Failure writing to target \"{1}\" of type {2} | {3} | {4} | {5} | {6}", DateTime.Now, target.Name, target.GetType().FullName, JoinTags(logEvent), GetMessage(logEvent), GetExceptionMessage(logEvent), exception);
        }

        private static string GetMessage(ILogEvent logEvent)
        {
            return logEvent is LogEvent ? ((LogEvent)logEvent).Message : logEvent.ToString();
        }

        private static string JoinTags(ILogEvent logEvent)
        {
            return logEvent.Tags != null ? string.Join(",", logEvent.Tags) : string.Empty;
        }

        private static string GetExceptionMessage(ILogEvent logEvent)
        {
            if (logEvent is LogEvent)
            {
                var exception = ((LogEvent)logEvent).Exception;
                return string.Format("LogEvent Exception: \"{0}\"", exception != null ? exception.Message : string.Empty);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}