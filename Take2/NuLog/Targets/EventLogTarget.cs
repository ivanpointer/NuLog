/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.LogEvents;
using System;
using System.Diagnostics;

namespace NuLog.Targets
{
    /// <summary>
    /// A target for writing to the Windows event log.
    /// </summary>
    public class EventLogTarget : LayoutTargetBase
    {
        private static readonly Type EventLogEntryTypeType = typeof(EventLogEntryType);

        private readonly IEventLog eventLog;

        private string source;

        private EventLogEntryType entryType;

        public EventLogTarget()
        {
            eventLog = new EventLogShim();
        }

        public EventLogTarget(IEventLog eventLog)
        {
            this.eventLog = eventLog;
        }

        public override void Write(LogEvent logEvent)
        {
            var message = Layout.Format(logEvent);
            eventLog.WriteEntry(source, message, entryType);
        }

        public override void Configure(TargetConfig config)
        {
            // Parse out the source
            source = GetProperty<string>(config, "source");
            if (string.IsNullOrEmpty(source))
            {
                throw new InvalidOperationException("Source is required for the event log target.");
            }

            // Parse out the source log, or default it to "Application" if it isn't configured
            var sourceLog = GetProperty<string>(config, "sourceLog");
            if (string.IsNullOrEmpty(sourceLog))
            {
                sourceLog = "Application";
            }

            // Create the source if it doesn't yet exist
            if (this.eventLog.SourceExists(source) == false)
            {
                this.eventLog.CreateEventSource(source, sourceLog);
            }

            // Parse out the entry type
            var entryTypeRaw = GetProperty<string>(config, "entryType");

#if PRENET4
            if (string.IsNullOrEmpty(entryTypeRaw) == false)
            {
                this.entryType = (EventLogEntryType)Enum.Parse(EventLogEntryTypeType, entryTypeRaw);
            }
            else
            {
                this.entryType = EventLogEntryType.Information;
            }
#else
            EventLogEntryType entryType;
            if (Enum.TryParse(entryTypeRaw, out entryType))
            {
                this.entryType = entryType;
            }
            else
            {
                this.entryType = EventLogEntryType.Information;
            }
#endif

            // Let the base configure
            base.Configure(config);
        }
    }
}