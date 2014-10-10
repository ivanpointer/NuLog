/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/9/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using System.Diagnostics;

namespace NuLog.Targets
{
    /// <summary>
    /// Writes log events to trace
    /// </summary>
    public class TraceTarget : LayoutTargetBase
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public TraceTarget() : base() { }

        /// <summary>
        /// Constructor setting the name of the target
        /// </summary>
        /// <param name="name">The name of the target</param>
        public TraceTarget(string name) : base() { Name = name; }

        /// <summary>
        /// Writes the log event to trace
        /// </summary>
        /// <param name="logEvent">The log event to write to trace</param>
        public override void Log(LogEvent logEvent)
        {
            Trace.Write(Layout.FormatLogEvent(logEvent));
        }
    }
}
