﻿/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.LogEvents;
using System;
using System.Diagnostics;

namespace NuLog.Targets
{
    /// <summary>
    /// A logger target that writes to debug (System.Diagnostics).
    /// </summary>
    public class DebugTarget : LayoutTargetBase
    {
        public override void Write(LogEvent logEvent)
        {
            // Make sure we have a layout
            if (this.Layout != null)
            {
                // Use the layout to format the log event, before writing it to debug.
                var formatted = this.Layout.Format(logEvent);
                Debug.Write(formatted);
            }
            else
            {
                // There isn't an assigned layout. Yell about it.
                throw new InvalidOperationException("Layout target was asked to write, without having been given a layout first.  First pass an instance of ILayout to the target using SetLayout, before writing log events to it.");
            }
        }
    }
}