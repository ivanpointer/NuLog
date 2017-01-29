/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Dispatchers;
using NuLog.LogEvents;
using System;
using System.Linq;

namespace NuLog.FallbackLoggers
{
    /// <summary>
    /// The core/basic functionality of the standard fallback logger implementation.
    /// </summary>
    public abstract class StandardFallbackLoggerBase : IFallbackLogger
    {
        public abstract void Log(Exception exception, ITarget target, ILogEvent logEvent);

        public abstract void Log(string message, params object[] args);

        protected virtual string FormatMessage(Exception exception, ITarget target, ILogEvent logEvent)
        {
            return FormatMessage("Failure writing to target \"{0}\" of type {1} | {2} | {3} | {4} | {5}", target.Name, target.GetType().FullName, JoinTags(logEvent), GetMessage(logEvent), GetExceptionMessage(logEvent), exception);
        }

        protected virtual string FormatMessage(string message, params object[] args)
        {
            var formatted = args != null && args.Length > 0 ? string.Format(message, args) : message;
            return string.Format("{0:yyyy-MM-dd hh:mm:ss.fff} | {1}", DateTime.Now, formatted);
        }

        private static string GetMessage(ILogEvent logEvent)
        {
            return logEvent is LogEvent ? ((LogEvent)logEvent).Message : logEvent.ToString();
        }

        private static string JoinTags(ILogEvent logEvent)
        {
#if PRENET4
            return logEvent.Tags != null ? string.Join(",", logEvent.Tags.ToArray()) : string.Empty;
#else
            return logEvent.Tags != null ? string.Join(",", logEvent.Tags) : string.Empty;
#endif
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