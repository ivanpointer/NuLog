using NuLog.Targets;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NuLog.Test.TestHarness
{
    public class ListTarget : TargetBase
    {
        private static readonly object _listLock = new object();
        private static readonly IList<LogEvent> _logEvents = new List<LogEvent>();

        public override void Log(LogEvent logEvent)
        {
            lock (_listLock)
            {
                _logEvents.Add(logEvent);
            }
        }

        public static IList<LogEvent> GetList()
        {
            lock (_listLock)
            {
                return new ReadOnlyCollection<LogEvent>(_logEvents);
            }
        }

        public static void ClearList()
        {
            lock (_listLock)
            {
                _logEvents.Clear();
            }
        }

        public override bool Shutdown()
        {
            lock (_listLock)
                _logEvents.Clear();

            return true;
        }
    }
}