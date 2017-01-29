/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.Diagnostics;

namespace NuLog.FallbackLoggers
{
    /// <summary>
    /// A standard fallback logger that writes failures to trace.
    /// </summary>
    public class StandardTraceFallbackLogger : StandardFallbackLoggerBase
    {
        public override void Log(Exception exception, ITarget target, ILogEvent logEvent)
        {
            var message = this.FormatMessage(exception, target, logEvent);
            Trace.WriteLine(message);
        }

        public override void Log(string message, params object[] args)
        {
            var formatted = this.FormatMessage(message, args);
            Trace.WriteLine(formatted);
        }
    }
}