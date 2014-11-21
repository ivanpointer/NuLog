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
            // This is a "unique" condition where "Silent" is interpreted differently;
            //  The "Silent" flag indicates to the framework that no trace/debug information
            //  is to be written about logging the log event.  This is to prevent unwanted
            //  "feedback loops" when using a trace listener.  This is the one case where
            //  a target is actually completely silenced by this setting.
            if(logEvent != null && logEvent.Silent == false)
                Trace.Write(Layout.FormatLogEvent(logEvent));
        }
    }
}
