/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/8/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using NuLog.Configuration.Targets;
using NuLog.Dispatch;
using System;
using System.Collections.Generic;

namespace NuLog.Targets
{
    public class ConsoleTarget : LayoutTargetBase
    {

        #region Constants
        
        public const string MetaForeground = "ConsoleForeground";
        public const string MetaBackground = "ConsoleBackground";

        #endregion

        #region Members, Constructors, Initialization and Configuration

        private static readonly object ConsoleLock = new object();

        // The parsed color rules and a cache: we cache the results of processing the rules, as it is expensive
        private ICollection<ConsoleColorRule> ColorRules { get; set; }
        private IDictionary<string, Tuple<ConsoleColor?, ConsoleColor?>> RuleCache { get; set; }

        /// <summary>
        /// Sets up the default console target
        /// </summary>
        public ConsoleTarget()
        {
            ColorRules = new List<ConsoleColorRule>();
            RuleCache = new Dictionary<string, Tuple<ConsoleColor?, ConsoleColor?>>();
        }

        /// <summary>
        /// Initializes the target with the given targe tconfig, dispatcher and synchronous flag
        /// </summary>
        /// <param name="targetConfig">The configuration to build this target with</param>
        /// <param name="dispatcher">The dispatcher that this target is attached to</param>
        /// <param name="synchronous">The synchronous flag, used to overwrite the synchronous behavior in the configuration</param>
        public override void Initialize(TargetConfig targetConfig, LogEventDispatcher dispatcher = null, bool? synchronous = null)
        {
            // Default to synchronous
            base.Initialize(targetConfig, dispatcher, synchronous.HasValue ? synchronous : true);

            // Parse out the target configuration
            if (targetConfig != null)
            {
                ConsoleTargetConfig consoleConfig = typeof(ConsoleTargetConfig).IsAssignableFrom(targetConfig.GetType())
                    ? (ConsoleTargetConfig)targetConfig
                    : new ConsoleTargetConfig(targetConfig.Config);

                ColorRules = consoleConfig.ColorRules;

                // The rules may have changed, reset the cache
                RuleCache.Clear();
            }
        }

        /// <summary>
        /// The observer hook for a new target configuration
        /// </summary>
        /// <param name="targetConfig">The new target configuration</param>
        public override void NotifyNewConfig(TargetConfig targetConfig)
        {
            Initialize(targetConfig);
        }

        #endregion

        #region Logging

        /// <summary>
        /// Logs the given log event to console, using the console color rules or overriding meta data to set the color
        /// </summary>
        /// <param name="logEvent">The log event to log</param>
        public override void Log(LogEvent logEvent)
        {
            lock (ConsoleLock)
            {
                // Default out the colors to the "current" colors of the console
                ConsoleColor foregroundColor = Console.ForegroundColor;
                ConsoleColor backgroundColor = Console.BackgroundColor;
                bool match = false;

                // Check the configured color rules for colors
                var rulesColor = GetConsoleColors(logEvent);
                if (rulesColor.Item1.HasValue)
                {
                    backgroundColor = rulesColor.Item1.Value;
                    match = true;
                }
                if (rulesColor.Item2.HasValue)
                {
                    foregroundColor = rulesColor.Item2.Value;
                    match = true;
                }

                // Check the meta data for colors
                if (logEvent.MetaData != null && logEvent.MetaData.Count > 0)
                {
                    if (logEvent.MetaData.ContainsKey(MetaForeground))
                    {
                        var foregroundRaw = logEvent.MetaData[MetaForeground];
                        if (foregroundRaw != null)
                            if (typeof(ConsoleColor).IsAssignableFrom(foregroundRaw.GetType()))
                            {
                                foregroundColor = (ConsoleColor)foregroundRaw;
                                match = true;
                            }
                    }

                    if (logEvent.MetaData.ContainsKey(MetaBackground))
                    {
                        var backgroundRaw = logEvent.MetaData[MetaBackground];
                        if (backgroundRaw != null)
                            if (typeof(ConsoleColor).IsAssignableFrom(backgroundRaw.GetType()))
                            {
                                backgroundColor = (ConsoleColor)backgroundRaw;
                                match = true;
                            }
                    }
                }

                // If colors are found, set them then write, otherwise, just write
                if (match)
                {
                    try
                    {
                        Console.ForegroundColor = foregroundColor;
                        Console.BackgroundColor = backgroundColor;

                        Console.Write(Layout.FormatLogEvent(logEvent));
                    }
                    finally
                    {
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.Write(Layout.FormatLogEvent(logEvent));
                }
            }
        }

        #endregion

        #region Helpers

        // Looks up the colors using the rules in the configuration and leverages a cache so that the rules don't have
        //  to be parsed multiple times
        private Tuple<ConsoleColor?, ConsoleColor?> GetConsoleColors(LogEvent logEvent)
        {
            // The cache is based on the tags associated to the log event
            //  The same tags will result in the same colors, every time
            string flattenedTags = FlattenTags(logEvent.Tags);
            if (RuleCache.ContainsKey(flattenedTags))
            {
                // Return the cached results
                return RuleCache[flattenedTags];
            }
            else
            {
                ConsoleColor? backgroundColor = null;
                ConsoleColor? foregroundColor = null;

                // Iterate over the color rules, looking for a match based on 
                //  the tag keeper assocaited with the dispatcher.
                if (ColorRules != null && ColorRules.Count > 0)
                {
                    var tagKeeper = Dispatcher.TagKeeper;
                    bool match = false;
                    foreach (var colorRule in ColorRules)
                    {
                        if (tagKeeper.CheckMatch(logEvent.Tags, colorRule.Tags))
                        {
                            if (colorRule.ForegroundColor != null)
                            {
                                foregroundColor = colorRule.ForegroundColor.Value;
                                match = true;
                            }

                            if (colorRule.BackgroundColor != null)
                            {
                                backgroundColor = colorRule.BackgroundColor.Value;
                                match = true;
                            }

                            // break as soon as a match is found
                            if (match)
                                break;
                        }
                    }
                }

                // Cache and return the results
                var consoleColors = new Tuple<ConsoleColor?, ConsoleColor?>(backgroundColor, foregroundColor);
                RuleCache[flattenedTags] = consoleColors;
                return consoleColors;
            }
        }

        // Flatten the tags into a string so that the results can be cached
        private static string FlattenTags(IEnumerable<string> tags)
        {
            return String.Join(",", tags);
        }

        #endregion

    }
}
