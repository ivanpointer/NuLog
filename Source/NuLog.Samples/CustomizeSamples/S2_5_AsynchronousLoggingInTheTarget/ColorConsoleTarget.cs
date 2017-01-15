/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration.Targets;
using NuLog.Dispatch;
using NuLog.Targets;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;

namespace NuLog.Samples.CustomizeSamples.S2_5_AsynchronousLoggingInTheTarget
{
    /// <summary>
    /// A sample class used to illustrate creating custom targets.  The narrative for this
    /// can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/2.5-Asynchronous-Logging-in-the-Target
    /// </summary>
    public class ColorConsoleTarget : LayoutTargetBase
    {
        #region Constants

        // Our message for failing to parse the color
        private const string ConsoleColorParseFailedMessage = "Failed to parse the console color \"{0}\" from the meta data";

        // A lock to make sure our colors don't get a discombobulated
        private static readonly object _colorLock = new object();

        // A couple constants for our meta data keys
        public const string BackgroundColorMeta = "ColorConsoleBackground";

        public const string ForegroundColorMeta = "ColorConsoleForeground";

        #endregion Constants

        // Our ColorConfig
        public ColorConsoleTargetConfig ColorConfig { get; set; }

        // Initialize our configuration
        public override void Initialize(TargetConfig targetConfig, LogEventDispatcher dispatcher = null, bool? synchronous = null)
        {
            // We need to inherit the configuration of the parent targets
            base.Initialize(targetConfig, dispatcher, synchronous);

            // Cast or parse our configuration into a ColorConsoleTargetConfig
            if (targetConfig != null)
                ColorConfig = targetConfig is ColorConsoleTargetConfig
                    ? (ColorConsoleTargetConfig)targetConfig
                    : new ColorConsoleTargetConfig(targetConfig.Config);
        }

        // Synchronous logging
        public override void Log(LogEvent logEvent)
        {
            // Lock, to keep our colors from getting messed up
            lock (_colorLock)
            {
                // Get a hold of the current colors
                var oldForeground = Console.ForegroundColor;
                var oldBackground = Console.BackgroundColor;

                try
                {
                    // Feedback loop prevention
                    bool silent = logEvent != null && logEvent.Silent;

                    // Set the colors to our custom colors
                    Console.BackgroundColor = GetConsoleColor(logEvent, BackgroundColorMeta, ColorConfig.BackgroundColor, silent);
                    Console.ForegroundColor = GetConsoleColor(logEvent, ForegroundColorMeta, ColorConfig.ForegroundColor, silent);

                    // Write out our message
                    Console.Out.Write(Layout.FormatLogEvent(logEvent));
                }
                finally
                {
                    // Make sure we reset our colors
                    Console.ForegroundColor = oldForeground;
                    Console.BackgroundColor = oldBackground;
                }
            }
        }

        // Designed to handle a group of log events - more performant
        protected override void ProcessLogQueue(ConcurrentQueue<LogEvent> logQueue, LogEventDispatcher dispatcher)
        {
            // Get a hold of the current colors and prep for the new ones
            var oldForeground = Console.ForegroundColor;
            var oldBackground = Console.BackgroundColor;
            ConsoleColor newBackground = oldBackground;
            ConsoleColor newForeground = oldForeground;
            string formattedLogEvent;

            using (var writer = new StreamWriter(new BufferedStream(Console.OpenStandardOutput())))
            {
                // Iterate over the queue, removing and logging each log event
                LogEvent logEvent;
                while (!DoShutdown && logQueue.IsEmpty == false)
                {
                    if (logQueue.TryDequeue(out logEvent))
                    {
                        // Figure out what our colors should be
                        newBackground = GetConsoleColor(logEvent, BackgroundColorMeta, ColorConfig.BackgroundColor, logEvent.Silent);
                        newForeground = GetConsoleColor(logEvent, ForegroundColorMeta, ColorConfig.ForegroundColor, logEvent.Silent);
                        formattedLogEvent = Layout.FormatLogEvent(logEvent);

                        lock (_colorLock)
                        {
                            try
                            {
                                // Re-center our colors
                                oldForeground = Console.ForegroundColor;
                                oldBackground = Console.BackgroundColor;

                                // Change our colors if we need to
                                if (newBackground != oldBackground)
                                    Console.BackgroundColor = newBackground;

                                if (newForeground != oldForeground)
                                    Console.ForegroundColor = newForeground;

                                // Finally, write out our message
                                writer.Write(formattedLogEvent);
                            }
                            finally
                            {
                                // Make sure we reset our colors
                                if (newBackground != oldBackground)
                                    Console.BackgroundColor = oldBackground;
                                if (newForeground != oldForeground)
                                    Console.ForegroundColor = oldForeground;
                            }
                        }
                    }
                }
            }
        }

        // Figures the console color we need based on the meta data of the log event, and the passed default color
        private static ConsoleColor GetConsoleColor(LogEvent logEvent, string metaDataKey, ConsoleColor defaultColor, bool silent)
        {
            // Try and return the console color from the metea data
            if (logEvent.MetaData != null && logEvent.MetaData.ContainsKey(metaDataKey))
            {
                try
                {
                    return (ConsoleColor)logEvent.MetaData[metaDataKey];
                }
                catch
                {
                    if (!silent)
                        Trace.WriteLine(string.Format(ConsoleColorParseFailedMessage, metaDataKey));
                }
            }

            // If we didn't get it from the meta data, return the passed default
            return defaultColor;
        }

        // Shutdown the target
        public override bool Shutdown()
        {
            // Make sure we finish writing our current log event before shutting down
            //  in other words, make sure that the colors have been reset before
            //  shutting down
            lock (_colorLock)
            {
                Trace.WriteLine("Achieved color lock, we can now shutdown...");
                return base.Shutdown();
            }
        }
    }
}