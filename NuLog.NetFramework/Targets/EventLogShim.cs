/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Diagnostics;

namespace NuLog.Targets {

    /// <summary>
    /// The shim implementation for the event log - simply wraps the EventLog.
    /// </summary>
    public class EventLogShim : IEventLog {

        public void CreateEventSource(string source, string logName) {
            EventLog.CreateEventSource(source, logName);
        }

        public bool SourceExists(string source) {
            return EventLog.SourceExists(source);
        }

        public void WriteEntry(string source, string message, EventLogEntryType type) {
            EventLog.WriteEntry(source, message, type);
        }
    }
}