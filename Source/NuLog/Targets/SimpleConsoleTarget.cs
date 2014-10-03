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
    public class SimpleConsoleTarget : LayoutTargetBase
    {
        private const int FlushIdleWait = 500; //milliseconds

        private static readonly object BufferLock = new object();
        private StreamWriter ConsoleWriter { get; set; }

        private Stopwatch _sw;

        public SimpleConsoleTarget()
        {
            _sw = new Stopwatch();
        }

        public override void Initialize(TargetConfig targetConfig, bool? synchronous = null)
        {
            lock (BufferLock)
            {
                base.Initialize(targetConfig, synchronous);

                ConsoleWriter = new StreamWriter(new BufferedStream(Console.OpenStandardOutput()));

                Task.Factory.StartNew(() =>
                {
                    while (!DoShutdown)
                    {
                        if (ConsoleWriter != null && _sw.IsRunning && _sw.ElapsedMilliseconds > FlushIdleWait)
                        {
                            lock (BufferLock)
                            {
                                ConsoleWriter.Flush();
                                _sw.Stop();
                            }
                        }

                        Thread.Yield();
                        Thread.Sleep(FlushIdleWait);
                    }
                });
            }
        }

        public override void Log(LogEvent logEvent)
        {
            ConsoleWriter.Write(Layout.FormatLogEvent(logEvent));

            _sw.Restart();
        }

        public override bool Shutdown(int timeout = DefaultShutdownTimeout)
        {
            if (ConsoleWriter != null)
                ConsoleWriter.Close();

            return base.Shutdown(timeout);
        }
    }
}
