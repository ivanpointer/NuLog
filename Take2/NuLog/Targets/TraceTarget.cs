/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.LogEvents;
using System.Diagnostics;

namespace NuLog.Targets
{
    /// <summary>
    /// A logger target that writes to trace (System.Diagnostics).
    /// </summary>
    public class TraceTarget : LayoutTargetBase
    {
        public override void Write(LogEvent logEvent)
        {
            var formatted = this.Layout.Format(logEvent);
            Trace.Write(formatted);

            // Removed, because the check for null on the layout was a hot spot in performance tuning.
            //// There isn't an assigned layout. Yell about it.
            //throw new InvalidOperationException("Layout target was asked to write, without having been given a layout first.  First pass an instance of ILayout to the target using SetLayout, before writing log events to it.");
        }
    }
}