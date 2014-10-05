using NuLog.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.ConsoleTest
{
    class Program
    {
        private const string ExitOption = "x";
        private const string ExitOptionText = "Exit";
        private const string TestListItemFormat = "\t{0}. {1}\r\n";
        private const string SelectQuestion = "Select an option: ";
        private const string InvalidOptionFormat = "Invalid option {0}";
        private const string FailureMessageFormat = "Failed to execute option {0} for: {1}";
        private const string TestOptionDoneMessage = "\r\n---\r\nTest option \"{0}\" done.  Press [Enter] to continue.";

        private const int DefaultPerformanceTestTimes = 10000;

        private static readonly IList<Tuple<string, Action<string[]>>> TestOptions = new List<Tuple<string, Action<string[]>>>
        {
            new Tuple<string, Action<string[]>>("Async Console Test", TestAsyncConsole)
            ,new Tuple<string, Action<string[]>>("Sync Console Test", TestSyncConsole)
            ,new Tuple<string, Action<string[]>>("Async File Test", TestAsyncFile)
            ,new Tuple<string, Action<string[]>>("Sync File Test", TestSyncFile)
        };

        static void Main(string[] args)
        {
            PrintTestOptions();

            string selection = null;
            do
            {
                ExecuteTestSelection(selection, args);
                selection = GetUserTestOption();
            } while (!ExitOption.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
        }

        #region Option Menu

        private static void PrintTestOptions()
        {
            Console.Clear();

            int testNumber = 1;
            foreach (var test in TestOptions)
                Console.Write(String.Format(TestListItemFormat, testNumber++, test.Item1));
            Console.Write(String.Format(TestListItemFormat, ExitOption, ExitOptionText));
            Console.WriteLine(String.Empty);
        }

        private static string GetUserTestOption()
        {
            PrintTestOptions();

            Console.Write(SelectQuestion);
            return Console.ReadLine();
        }

        private static void ExecuteTestSelection(string selection, string[] args)
        {
            if (String.IsNullOrEmpty(selection) == false)
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
                        Console.WriteLine(String.Format(FailureMessageFormat, optionNumber, e));
                    }
                }
                else
                {
                    Console.WriteLine(String.Format(InvalidOptionFormat, selection));
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
                Console.Write(String.Format("{0}{1}: ", message, def.HasValue
                    ? String.Format(" [{0}]", def.Value) : String.Empty));
                input = Console.ReadLine();
                if (String.IsNullOrEmpty(input))
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

        #endregion

        private static void TestSyncConsole(string[] args)
        {
            int testTimes = GetIntFromUser("Number of test log events", DefaultPerformanceTestTimes);

            Stopwatch initializeSW = new Stopwatch();

            initializeSW.Start();
            LoggerFactory.Initialize(@"Configs\Console.json");
            LoggerBase logger = LoggerFactory.GetLogger("console", "console");
            initializeSW.Stop();

            Stopwatch logSW = new Stopwatch();
            logSW.Start();
            for (int lp = 0; lp < testTimes; lp++)
                logger.LogNow("Entry " + lp);
            logSW.Stop();

            Trace.WriteLine(String.Format("Initialize: {0} ms", initializeSW.ElapsedMilliseconds));
            Trace.WriteLine(String.Format("Log Time: {0} ms", logSW.ElapsedMilliseconds));
            Trace.WriteLine(String.Format("Total Messages: {0}", testTimes));
            Trace.WriteLine(String.Format("Time per message: {0} ms", (double)logSW.ElapsedMilliseconds / (double)testTimes));
        }

        private static void TestAsyncConsole(string[] args)
        {
            int testTimes = GetIntFromUser("Number of test log events", DefaultPerformanceTestTimes);

            Stopwatch initializeSW = new Stopwatch();

            initializeSW.Start();
            LoggerFactory.Initialize(@"Configs\Console.json");
            LoggerBase logger = LoggerFactory.GetLogger("console", "console");
            initializeSW.Stop();

            Stopwatch logSW = new Stopwatch();
            logSW.Start();
            for (int lp = 0; lp < testTimes; lp++)
                logger.Log("Entry " + lp);
            logSW.Stop();

            Trace.WriteLine(String.Format("Initialize: {0} ms", initializeSW.ElapsedMilliseconds));
            Trace.WriteLine(String.Format("Log Time: {0} ms", logSW.ElapsedMilliseconds));
            Trace.WriteLine(String.Format("Total Messages: {0}", testTimes));
            Trace.WriteLine(String.Format("Time per message: {0} ms", (double)logSW.ElapsedMilliseconds / (double)testTimes));
        }

        private static void TestAsyncFile(string[] args)
        {
            int testTimes = GetIntFromUser("Number of test log events", DefaultPerformanceTestTimes);

            Stopwatch initializeSW = new Stopwatch();

            initializeSW.Start();
            LoggerFactory.Initialize(@"Configs\File.json");
            LoggerBase logger = LoggerFactory.GetLogger("file", "file");
            initializeSW.Stop();

            Stopwatch logSW = new Stopwatch();
            logSW.Start();
            for (int lp = 0; lp < testTimes; lp++)
                logger.Log("Entry " + lp);
            logSW.Stop();

            Trace.WriteLine(String.Format("Initialize: {0} ms", initializeSW.ElapsedMilliseconds));
            Trace.WriteLine(String.Format("Log Time: {0} ms", logSW.ElapsedMilliseconds));
            Trace.WriteLine(String.Format("Total Messages: {0}", testTimes));
            Trace.WriteLine(String.Format("Time per message: {0} ms", (double)logSW.ElapsedMilliseconds / (double)testTimes));
        }

        private static void TestSyncFile(string[] args)
        {
            int testTimes = GetIntFromUser("Number of test log events", DefaultPerformanceTestTimes);

            Stopwatch initializeSW = new Stopwatch();

            initializeSW.Start();
            LoggerFactory.Initialize(@"Configs\File.json");
            LoggerBase logger = LoggerFactory.GetLogger("file", "file");
            initializeSW.Stop();

            Stopwatch logSW = new Stopwatch();
            logSW.Start();
            for (int lp = 0; lp < testTimes; lp++)
                logger.LogNow("Entry " + lp);
            logSW.Stop();

            Trace.WriteLine(String.Format("Initialize: {0} ms", initializeSW.ElapsedMilliseconds));
            Trace.WriteLine(String.Format("Log Time: {0} ms", logSW.ElapsedMilliseconds));
            Trace.WriteLine(String.Format("Total Messages: {0}", testTimes));
            Trace.WriteLine(String.Format("Time per message: {0} ms", (double)logSW.ElapsedMilliseconds / (double)testTimes));
        }
    }
}
