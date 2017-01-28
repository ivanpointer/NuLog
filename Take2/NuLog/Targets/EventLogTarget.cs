/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.LogEvents;
using System;

namespace NuLog.Targets
{
    /// <summary>
    /// A target for writing to the Windows event log.
    /// </summary>
    public class EventLogTarget : LayoutTargetBase
    {
        private readonly IEventLog eventLog;

        public EventLogTarget()
        {
            eventLog = null;
        }

        public EventLogTarget(IEventLog eventLog)
        {
            this.eventLog = eventLog;
        }

        public override void Write(LogEvent logEvent)
        {
            throw new NotImplementedException();
        }
    }
}