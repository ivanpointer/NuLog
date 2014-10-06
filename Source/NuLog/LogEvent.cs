/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/5/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace NuLog
{
    /// <summary>
    /// A LogEvent represents a single log event (or message) that is to be
    /// delivered to the different targets.  The LogEvent carries all the
    /// information needed, including a MetaData mechanism to allow for
    /// sending any kind of information to targets, standard and custom.
    /// </summary>
    public class LogEvent
    {
        /// <summary>
        /// Represents the point-in-time that the log event occurred (as opposed to when the event was logged)
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// The thread from which the log event originated
        /// </summary>
        public Thread Thread { get; set; }
        /// <summary>
        /// The stack frame of the method from which the log event originated.
        /// By default, this is only populated if "debug" is turned on in the
        /// master logging configuration
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
        /// The meta data associated with the log event.  Meta data is used to
        /// communicate special information to targets, or can be referenced
        /// in layouyt targets and represented as text in the layout target
        /// destination.
        /// </summary>
        public IDictionary<string, object> MetaData { get; set; }

        /// <summary>
        /// Constructs a default instance of a log event set
        /// to DateTime.UtcNow and the current thread
        /// </summary>
        public LogEvent()
        {
            DateTime = DateTime.UtcNow;
            Tags = new List<string>();
            Thread = Thread.CurrentThread;
        }

        /// <summary>
        /// A complex constructor allowing for building more information into the log event
        /// </summary>
        /// <param name="message">The message associated with the log event</param>
        /// <param name="dateTime">A date time representing when the event occured</param>
        /// <param name="exception">An exception (if any) that is associated with the log event</param>
        /// <param name="metaData">The meta data associated with the log event</param>
        /// <param name="tags">Any tags to be assigned to the log event</param>
        public LogEvent(string message, DateTime? dateTime = null, Exception exception = null, IDictionary<string, object> metaData = null, params string[] tags)
        {
            DateTime = dateTime.HasValue ? dateTime.Value : DateTime.UtcNow;
            Message = message;
            Exception = exception;
            MetaData = metaData;
            Tags = tags.ToList();
            Thread = Thread.CurrentThread;
        }
    }
}
