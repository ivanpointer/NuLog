/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/9/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
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
        /// A meta key that signals to the trace target not to trace the log event.
        /// This is helpful in preventing "feedback loops" when using a trace listener.
        /// </summary>
        public const string DontTraceMeta = "DontTrace";

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
            if(logEvent.MetaData == null || logEvent.MetaData.ContainsKey(DontTraceMeta) == false)
                Trace.Write(Layout.FormatLogEvent(logEvent));
        }
    }
}
