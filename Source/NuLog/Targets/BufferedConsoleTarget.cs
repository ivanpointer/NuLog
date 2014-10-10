/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/8/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using NuLog.Dispatch;
using System;
using System.Collections.Concurrent;
using System.IO;

namespace NuLog.Targets
{
    /// <summary>
    /// A console target that buffers writing when logging multiple log events asynchronously
    /// </summary>
    public class BufferedConsoleTarget : LayoutTargetBase
    {
        public override void Log(LogEvent logEvent)
        {
            Console.Write(Layout.FormatLogEvent(logEvent));
        }

        protected override void ProcessLogQueue(ConcurrentQueue<LogEvent> logQueue, LogEventDispatcher dispatcher)
        {
            // Open a buffered stream writer to the console standard out
            LogEvent logEvent;
            using (var writer = new StreamWriter(new BufferedStream(Console.OpenStandardOutput())))
            {
                // Pull all of the log events from the queue and write them to the buffered writer
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
                            try
                            {
                                writer.Flush();
                            }
                            finally
                            {
                                if (dispatcher != null)
                                    dispatcher.HandleException(e, logEvent);
                                else
                                    throw e;
                            }
                        }
                    }
                }
                writer.Flush();
            }
        }
    }
}
