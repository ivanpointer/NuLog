using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NuLog
{
    public class LogEvent
    {
        public DateTime DateTime { get; set; }
        public Thread Thread { get; set; }
        public StackFrame LoggingStackFrame { get; set; }
        public string Message { get; set; }
        public ICollection<string> Tags { get; set; }
        public Exception Exception { get; set; }
        public IDictionary<string, object> MetaData { get; set; }

        public LogEvent()
        {
            DateTime = DateTime.UtcNow;
            Tags = new List<string>();
            Thread = Thread.CurrentThread;
        }

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
