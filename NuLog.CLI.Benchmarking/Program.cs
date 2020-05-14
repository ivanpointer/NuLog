/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.Diagnostics;
using System.IO;

namespace NuLog.CLI.Benchmarking {

    internal static class Program {
        private const string BENCHMARK_TYPE = "NuLog";
        private const string BENCHMARK_LOG_PATH = @"C:\Temp\benchmark.log";
        private const string BENCHMARK_COMMENTS = "Without";

        private const int ITERATIONS = 10000000;

        private static void Main(string[] args) {
            Log();
        }

        private static void Log() {
            var logger = LogManager.GetLogger();
            var sw = new Stopwatch();
            sw.Start();
            for (var lp = 1; lp <= ITERATIONS; lp++) {
                logger.Log("Benchmark message " + lp);
            }
            LogManager.Shutdown();
            sw.Stop();
            Console.WriteLine("Elapsed: " + sw.Elapsed);
            RecordBenchmark(sw.Elapsed, ITERATIONS, BENCHMARK_TYPE, BENCHMARK_COMMENTS);
        }

        private static void RecordBenchmark(TimeSpan executionTime, int iterations, string benchmarkType, string comments) {
            var currentTime = DateTime.Now;
            var timePerIteration = Math.Ceiling(executionTime.TotalMilliseconds / (double)iterations);
            var entry = string.Format("\r\n{0:MM/dd/yyyy hh:mm:ss} | {1} | {2} | {3} | {4} | {5}", currentTime, benchmarkType, executionTime, iterations, timePerIteration, comments);
            File.AppendAllText(BENCHMARK_LOG_PATH, entry);
        }
    }
}