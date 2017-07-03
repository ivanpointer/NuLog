/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.Diagnostics;
using System.IO;

namespace NuLog.CLI.Benchmarking
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Log();

            Teardown();
        }

        private static void Teardown()
        {
            if(File.Exists("benchmark.log"))
            {
                File.Delete("benchmark.log");
            }
        }

        private static void ToConsole()
        {
            var sw = new Stopwatch();
            sw.Start();
            for (var lp = 1; lp < 10001; lp++)
            {
                Console.WriteLine("Benchmark message " + lp);
            }
            sw.Stop();
            Console.WriteLine("Elapsed: " + sw.Elapsed);
        }

        private static void Log()
        {
            var logger = LogManager.GetLogger();
            var sw = new Stopwatch();
            sw.Start();
            for (var lp = 1; lp < 10001; lp++)
            {
                logger.Log("Benchmark message " + lp);
            }
            LogManager.Shutdown();
            sw.Stop();
            Console.WriteLine("Elapsed: " + sw.Elapsed);
        }

        private static void LogNow()
        {
            var logger = LogManager.GetLogger();
            var sw = new Stopwatch();
            sw.Start();
            for (var lp = 1; lp < 10001; lp++)
            {
                logger.LogNow("Benchmark message " + lp);
            }
            LogManager.Shutdown();
            sw.Stop();
            Console.WriteLine("Elapsed: " + sw.Elapsed);
        }
    }
}