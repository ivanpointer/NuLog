/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */
/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/20/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NuLog.ConsoleTest
{
    internal class Program
    {
        #region Constants

        private const string ExitOption = "x";
        private const string ExitOptionText = "Exit";
        private const string TestListItemFormat = "\t{0}. {1}\r\n";
        private const string SelectQuestion = "Select an option: ";
        private const string InvalidOptionFormat = "Invalid option {0}";
        private const string FailureMessageFormat = "Failed to execute option {0} for: {1}";
        private const string TestOptionDoneMessage = "\r\n---\r\nTest option \"{0}\" done.  Press [Enter] to continue.";

        private const int DefaultPerformanceTestTimes = 10000;

        #endregion Constants

        private static readonly IList<Tuple<string, Action<string[]>>> TestOptions = new List<Tuple<string, Action<string[]>>>
        {
            new Tuple<string, Action<string[]>>("Async Console Test", TestAsyncConsole)
            ,new Tuple<string, Action<string[]>>("LogNow Console Test", TestLogNowConsole)
            ,new Tuple<string, Action<string[]>>("Sync Console Test", TestSyncConsole)
            ,new Tuple<string, Action<string[]>>("Async Color Console Test", TestAsyncColorConsole)
            ,new Tuple<string, Action<string[]>>("Sync Color Console Test", TestSyncColorConsole)
            ,new Tuple<string, Action<string[]>>("Async File Test", TestAsyncFile)
            ,new Tuple<string, Action<string[]>>("LogNow File Test", TestSyncFile)
            ,new Tuple<string, Action<string[]>>("Windows Event Log Test", TestWindowsEventLog)
        };

        private static void Main(string[] args)
        {
            PrintTestOptions();

            string selection = null;
            do
            {
                ExecuteTestSelection(selection, args);
                selection = GetUserTestOption();
            } while (!ExitOption.Equals(selection, StringComparison.InvariantCultureIgnoreCase));

            // Make sure the default instance is shutdown
            LoggerFactory.GetDefaultInstance().Shutdown();
        }

        #region Option Menu

        private static void PrintTestOptions()
        {
            Console.Clear();

            int testNumber = 1;
            foreach (var test in TestOptions)
                Console.Write(string.Format(TestListItemFormat, testNumber++, test.Item1));
            Console.Write(string.Format(TestListItemFormat, ExitOption, ExitOptionText));
            Console.WriteLine(string.Empty);
        }

        private static string GetUserTestOption()
        {
            PrintTestOptions();

            Console.Write(SelectQuestion);
            return Console.ReadLine();
        }

        private static void ExecuteTestSelection(string selection, string[] args)
        {
            if (string.IsNullOrEmpty(selection) == false)
            {
                int optionNumber;
                if (ParseOption(selection, out optionNumber))
                {
                    var option = TestOptions[optionNumber];
                    try
                    {
                        Console.Clear();
                        option.Item2(args);
                        Console.Write(TestOptionDoneMessage, option.Item1);
                        Console.ReadLine();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(string.Format(FailureMessageFormat, optionNumber, e));
                    }
                }
                else
                {
                    Console.WriteLine(string.Format(InvalidOptionFormat, selection));
                }
            }
        }

        private static bool ParseOption(string selection, out int optionNumber)
        {
            if (Int32.TryParse(selection, out optionNumber))
                if (1 <= optionNumber && optionNumber <= TestOptions.Count)
                {
                    optionNumber--;
                    return true;
                }

            return false;
        }

        private static int GetIntFromUser(string message, int? def = null)
        {
            string input;
            int? val = null;
            while (val.HasValue == false)
            {
                Console.Write(string.Format("{0}{1}: ", message, def.HasValue
                    ? string.Format(" [{0}]", def.Value) : string.Empty));
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    if (def.HasValue)
                        val = def;
                }
                else
                {
                    val = Convert.ToInt32(input);
                }
            }

            return val.Value;
        }

        #endregion Option Menu

        private static void TestLogNowConsole(string[] args)
        {
            var testTimes = GetIntFromUser("Number of test log events", DefaultPerformanceTestTimes);

            var initializeSW = new Stopwatch();

            initializeSW.Start();
            var factory = new LoggerFactory(@"Configs\Console.json");
            var logger = factory.Logger("console", "console");
            initializeSW.Stop();

            var logSW = new Stopwatch();
            logSW.Start();
            for (int lp = 0; lp < testTimes; lp++)
                logger.LogNow("Entry " + lp);
            logSW.Stop();

            Trace.WriteLine(string.Format("Initialize: {0} ms", initializeSW.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Log Time: {0} ms", logSW.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Total Messages: {0}", testTimes));
            Trace.WriteLine(string.Format("Time per message: {0} ms", (double)logSW.ElapsedMilliseconds / (double)testTimes));
        }

        private static void TestSyncConsole(string[] args)
        {
            var testTimes = GetIntFromUser("Number of test log events", DefaultPerformanceTestTimes);

            var initializeSW = new Stopwatch();

            initializeSW.Start();
            var factory = new LoggerFactory(@"Configs\SyncConsole.json");
            var logger = factory.Logger("console", "console");
            initializeSW.Stop();

            var logSW = new Stopwatch();
            logSW.Start();
            for (int lp = 0; lp < testTimes; lp++)
                logger.Log("Entry " + lp);
            logSW.Stop();

            Trace.WriteLine(string.Format("Initialize: {0} ms", initializeSW.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Log Time: {0} ms", logSW.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Total Messages: {0}", testTimes));
            Trace.WriteLine(string.Format("Time per message: {0} ms", (double)logSW.ElapsedMilliseconds / (double)testTimes));
        }

        private static void TestAsyncConsole(string[] args)
        {
            var testTimes = GetIntFromUser("Number of test log events", DefaultPerformanceTestTimes);

            var initializeSW = new Stopwatch();

            initializeSW.Start();
            var factory = new LoggerFactory(@"Configs\Console.json");
            var logger = factory.Logger("console", "console");
            initializeSW.Stop();

            var logSW = new Stopwatch();
            logSW.Start();
            for (int lp = 0; lp < testTimes; lp++)
                logger.Log("Entry " + lp);
            logSW.Stop();

            Trace.WriteLine(string.Format("Initialize: {0} ms", initializeSW.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Log Time: {0} ms", logSW.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Total Messages: {0}", testTimes));
            Trace.WriteLine(string.Format("Time per message: {0} ms", (double)logSW.ElapsedMilliseconds / (double)testTimes));
        }

        private static void TestAsyncFile(string[] args)
        {
            var testTimes = GetIntFromUser("Number of test log events", DefaultPerformanceTestTimes);

            var initializeSW = new Stopwatch();

            initializeSW.Start();
            var factory = new LoggerFactory(@"Configs\File.json");
            var logger = factory.Logger("file", "file");
            initializeSW.Stop();

            var logSW = new Stopwatch();
            logSW.Start();
            for (int lp = 0; lp < testTimes; lp++)
                logger.Log("Entry " + lp);
            logSW.Stop();

            Trace.WriteLine(string.Format("Initialize: {0} ms", initializeSW.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Log Time: {0} ms", logSW.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Total Messages: {0}", testTimes));
            Trace.WriteLine(string.Format("Time per message: {0} ms", (double)logSW.ElapsedMilliseconds / (double)testTimes));
        }

        private static void TestSyncFile(string[] args)
        {
            var testTimes = GetIntFromUser("Number of test log events", DefaultPerformanceTestTimes);

            var initializeSW = new Stopwatch();

            initializeSW.Start();
            var factory = new LoggerFactory(@"Configs\File.json");
            var logger = factory.Logger("file", "file");
            initializeSW.Stop();

            var logSW = new Stopwatch();
            logSW.Start();
            for (int lp = 0; lp < testTimes; lp++)
                logger.LogNow("Entry " + lp);
            logSW.Stop();

            Trace.WriteLine(string.Format("Initialize: {0} ms", initializeSW.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Log Time: {0} ms", logSW.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Total Messages: {0}", testTimes));
            Trace.WriteLine(string.Format("Time per message: {0} ms", (double)logSW.ElapsedMilliseconds / (double)testTimes));
        }

        private static void TestAsyncColorConsole(string[] args)
        {
            var testTimes = GetIntFromUser("Number of test log events", DefaultPerformanceTestTimes);

            var initializeSW = new Stopwatch();

            initializeSW.Start();
            var factory = new LoggerFactory(@"Configs\ColorConsole.json");
            var logger = factory.Logger("console", "console");
            initializeSW.Stop();

            var logSW = new Stopwatch();
            logSW.Start();
            for (int lp = 0; lp < testTimes; lp++)
                logger.Log("Entry " + lp);
            logSW.Stop();

            Trace.WriteLine(string.Format("Initialize: {0} ms", initializeSW.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Log Time: {0} ms", logSW.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Total Messages: {0}", testTimes));
            Trace.WriteLine(string.Format("Time per message: {0} ms", (double)logSW.ElapsedMilliseconds / (double)testTimes));
        }

        private static void TestSyncColorConsole(string[] args)
        {
            var testTimes = GetIntFromUser("Number of test log events", DefaultPerformanceTestTimes);

            var initializeSW = new Stopwatch();

            initializeSW.Start();
            var factory = new LoggerFactory(@"Configs\SyncColorConsole.json");
            var logger = factory.Logger("console", "console");
            initializeSW.Stop();

            var logSW = new Stopwatch();
            logSW.Start();
            for (int lp = 0; lp < testTimes; lp++)
                logger.Log("Entry " + lp);
            logSW.Stop();

            Trace.WriteLine(string.Format("Initialize: {0} ms", initializeSW.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Log Time: {0} ms", logSW.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Total Messages: {0}", testTimes));
            Trace.WriteLine(string.Format("Time per message: {0} ms", (double)logSW.ElapsedMilliseconds / (double)testTimes));
        }

        private static void TestWindowsEventLog(string[] args)
        {
            var testTimes = GetIntFromUser("Number of test log events", 3);

            var initializeSW = new Stopwatch();

            initializeSW.Start();
            var factory = new LoggerFactory(@"Configs\WindowsEventLog.json");
            var logger = factory.Logger("windowsEventLog", "windowsEventLog");
            initializeSW.Stop();

            var logSW = new Stopwatch();
            logSW.Start();
            for (int lp = 0; lp < testTimes; lp++)
                logger.LogNow("Entry " + lp);
            logSW.Stop();

            Trace.WriteLine(string.Format("Initialize: {0} ms", initializeSW.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Log Time: {0} ms", logSW.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Total Messages: {0}", testTimes));
            Trace.WriteLine(string.Format("Time per message: {0} ms", (double)logSW.ElapsedMilliseconds / (double)testTimes));
        }
    }
}