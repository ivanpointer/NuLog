using NuLog.Targets.Layouts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Targets
{
    public class TraceTarget : LayoutTargetBase
    {
        public TraceTarget()
        {
            Initialize();
        }

        public TraceTarget(string name = "target", string layout = null, bool? synchronous = false)
        {
            Initialize(name, layout, synchronous);
        }

        internal void Initialize(string name = "target", string layout = null, bool? synchronous = false)
        {
            Name = name;
            Synchronous = synchronous.HasValue
                ? synchronous.Value
                : false;
            Layout = new StandardLayout(layout);
        }

        public override void Log(LogEvent logEvent)
        {
            Trace.Write(Layout.FormatLogEvent(logEvent));
        }
    }
}
