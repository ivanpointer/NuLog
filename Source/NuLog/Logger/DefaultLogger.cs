using NuLog.Dispatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Logger
{
    public class DefaultLogger : LoggerBase
    {
        public DefaultLogger(LogEventDispatcher dispatcher) : base(dispatcher) { }

        public DefaultLogger(LogEventDispatcher dispatcher, ICollection<string> defaultTags) : base(dispatcher, defaultTags) { }
    }
}
