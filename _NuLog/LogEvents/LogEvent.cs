/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace NuLog.LogEvents {

    /// <summary>
    /// A LogEvent represents a single log event (or message) that is to be delivered to the
    /// different targets. The LogEvent carries all the information needed, including a MetaData
    /// mechanism to allow for sending any kind of information to targets, standard and custom.
    /// </summary>
    public class LogEvent : ILogEvent {

        /// <summary>
        /// Represents the point-in-time that the event was logged (not necessarily written, as
        /// writes are often deferred).
        /// </summary>
        public DateTime DateLogged { get; set; }

        /// <summary>
        /// The thread from which the log event originated
        /// </summary>
        public Thread Thread { get; set; }

        /// <summary>
        /// The stack frame of the method from which the log event originated. By default, this is
        /// only populated if "debug" is turned on in the master logging configuration
        /// </summary>
        public StackFrame LoggingStackFrame { get; set; }

        /// <summary>
        /// The message associated with the log event
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The list of tags associated with the log event
        /// </summary>
        public ICollection<string> Tags { get; set; }

        /// <summary>
        /// The exception (if any) associated with the log event
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// The meta data associated with the log event. Meta data is used to communicate special
        /// information to targets, or can be referenced in layout targets and represented as text in
        /// the layout target destination.
        /// </summary>
        public IDictionary<string, object> MetaData { get; set; }

        /// <summary>
        /// Write this log event to the target.
        /// </summary>
        /// <param name="target">The target to write to.</param>
        public virtual void WriteTo(ITarget target) {
            target.Write(this);
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing) {
            // Eh, nothing to do..
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

            // Tell the GC that we've got it
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}