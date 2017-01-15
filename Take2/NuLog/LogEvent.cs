/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace NuLog
{
    /// <summary>
    /// A LogEvent represents a single log event (or message) that is to be delivered to the
    /// different targets. The LogEvent carries all the information needed, including a MetaData
    /// mechanism to allow for sending any kind of information to targets, standard and custom.
    /// </summary>
    public class LogEvent
    {
        /// <summary>
        /// Represents the point-in-time that the log event occurred (as opposed to when the event
        /// was logged)
        /// </summary>
        public DateTime DateTime { get; set; }

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

        ///// <summary>
        ///// A flag that if set to true, indicates to the system that no trace or debug messages should be written concerning
        ///// processing this log event.  This is to help prevent "feedback" loops in trace/debug listeners.  Note that
        ///// custom targets, and said trace/debug listeners must be programmed to honor this.
        ///// </summary>
        //public bool Silent { get; set; }
    }
}