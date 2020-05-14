/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;

namespace NuLog.CLI.PerfTune {

    /// <summary>
    /// A "throwaway" project for performance tuning NuLog.
    /// </summary>
    public static class Program {
        private static readonly ILogger _logger = LogManager.GetLogger();

        private static void Main(string[] args) {
            Console.WriteLine("Press [Enter] to begin...");
            Console.ReadLine();

            for (var times = 0; times < 3; times++) {
                for (var lp = 0; lp < 10000; lp++) {
                    _logger.Log("Stress message " + lp);
                }

                Console.WriteLine("Press [Enter] to continue...");
                Console.ReadLine();
            }

            LogManager.Shutdown();
            Console.WriteLine("Press [Enter] to exit...");
            Console.ReadLine();
        }
    }
}