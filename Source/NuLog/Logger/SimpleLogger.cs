using NuLog.Dispatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Logger
{
    public class SimpleLogger : LoggerBase
    {
        public SimpleLogger(LogEventDispatcher dispatcher) : base(dispatcher) { }

        public SimpleLogger(LogEventDispatcher dispatcher, ICollection<string> defaultTags) : base(dispatcher, defaultTags) { }
    }
}
