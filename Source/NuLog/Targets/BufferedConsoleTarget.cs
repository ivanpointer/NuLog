using NuLog.Configuration.Targets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NuLog.Targets
{
    public class BufferedConsoleTarget : LayoutTargetBase
    {
        public override void Log(LogEvent logEvent)
        {
            Console.Write(Layout.FormatLogEvent(logEvent));
        }

        protected override void ProcessLogQueue(System.Collections.Concurrent.ConcurrentQueue<LogEvent> logQueue, Dispatch.LogEventDispatcher dispatcher)
        {
            LogEvent logEvent;
            using (var writer = new StreamWriter(new BufferedStream(Console.OpenStandardOutput())))
            {
                while (logQueue.IsEmpty == false)
                {
                    if (logQueue.TryDequeue(out logEvent))
                    {
                        try
                        {
                            writer.Write(Layout.FormatLogEvent(logEvent));
                        }
                        catch (Exception e)
                        {
                            if (dispatcher != null)
                                dispatcher.HandleException(e, logEvent);
                            else
                                throw e;
                        }
                    }
                }
                writer.Flush();
            }
        }
    }
}
