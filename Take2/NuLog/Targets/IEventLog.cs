/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Diagnostics;

namespace NuLog.Targets {

    /// <summary>
    /// A shim for interacting with the event log. This shim allows for unit testing of the event log
    /// target, while using a "real" implementation "live", when not testing.
    /// </summary>
    public interface IEventLog {

        /// <summary>
        /// Write an entry to the Windows event log.
        /// </summary>
        void WriteEntry(string source, string message, EventLogEntryType type);

        /// <summary>
        /// Check to see if the source exists in the windows event log.
        /// </summary>
        bool SourceExists(string source);

        /// <summary>
        /// Creates the identified source in the identified log.
        /// </summary>
        void CreateEventSource(string source, string logName);
    }
}