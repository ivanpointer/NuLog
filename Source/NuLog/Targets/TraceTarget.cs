using NuLog.Dispatch;
using NuLog.Targets.Layouts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Targets
{
    public class TraceTarget : LayoutTargetBase
    {
        public TraceTarget() : base() { }

        public TraceTarget(string name) : base() { Name = name; }

        public override void Log(LogEvent logEvent)
        {
            Trace.Write(Layout.FormatLogEvent(logEvent));
        }
    }
}
