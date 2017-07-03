/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NLog;
using System;
using System.Diagnostics;

namespace NuLog.CLI.Benchmarking.NLog
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Log();
        }

        private static void Log()
        {
            var logger = LogManager.GetCurrentClassLogger();
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