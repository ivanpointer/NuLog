/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using log4net;
using log4net.Config;
using System;
using System.Diagnostics;

namespace NuLog.CLI.Benchmarking.Log4Net
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            Log();
        }

        private static void Log()
        {
            var logger = LogManager.GetLogger(typeof(Program));
            var sw = new Stopwatch();
            sw.Start();
            for (var lp = 1; lp < 10001; lp++)
            {
                logger.Info("Benchmark message " + lp);
            }
            LogManager.Shutdown();
            sw.Stop();
            Console.WriteLine("Elapsed: " + sw.Elapsed);
        }
    }
}