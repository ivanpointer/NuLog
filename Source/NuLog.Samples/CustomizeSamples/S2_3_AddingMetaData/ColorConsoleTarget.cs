﻿/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/11/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Configuration.Targets;
using NuLog.Dispatch;
using NuLog.Targets;
using System;
using System.Diagnostics;

namespace NuLog.Samples.CustomizeSamples.S2_3_AddingMetaData
{

    /// <summary>
    /// A sample class used to illustrate creating custom targets.  The narrative for this
    /// can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/2.3-Adding-Meta-Data
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

        #endregion

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
                    // Set the colors to our custom colors
                    Console.BackgroundColor = GetConsoleColor(logEvent, BackgroundColorMeta, ColorConfig.BackgroundColor);
                    Console.ForegroundColor = GetConsoleColor(logEvent, ForegroundColorMeta, ColorConfig.ForegroundColor);

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

        // Figures the console color we need based on the meta data of the log event, and the passed default color
        private static ConsoleColor GetConsoleColor(LogEvent logEvent, string metaDataKey, ConsoleColor defaultColor)
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
                    Trace.WriteLine(String.Format(ConsoleColorParseFailedMessage, metaDataKey));
                }
            }

            // If we didn't get it from the meta data, return the passed default
            return defaultColor;
        }

    }
}

