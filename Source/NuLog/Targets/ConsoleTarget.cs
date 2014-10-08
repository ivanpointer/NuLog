using NuLog.Configuration.Targets;
using NuLog.Dispatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Targets
{
    public class ConsoleTarget : LayoutTargetBase
    {
        public const string MetaForeground = "ConsoleForeground";
        public const string MetaBackground = "ConsoleBackground";

        private static readonly object ConsoleLock = new object();
        private ICollection<ConsoleColorRule> ColorRules { get; set; }

        private IDictionary<string, Tuple<ConsoleColor?, ConsoleColor?>> RuleCache { get; set; }

        public ConsoleTarget()
        {
            ColorRules = new List<ConsoleColorRule>();
            RuleCache = new Dictionary<string, Tuple<ConsoleColor?, ConsoleColor?>>();
        }

        public override void Initialize(TargetConfig targetConfig, LogEventDispatcher dispatcher = null, bool? synchronous = null)
        {
            base.Initialize(targetConfig, dispatcher, synchronous.HasValue ? synchronous : true); // Default to synchronous

            if (targetConfig != null)
            {
                ConsoleTargetConfig consoleConfig = typeof(ConsoleTargetConfig).IsAssignableFrom(targetConfig.GetType())
                    ? (ConsoleTargetConfig)targetConfig
                    : new ConsoleTargetConfig(targetConfig.Config);

                ColorRules = consoleConfig.ColorRules;
                RuleCache.Clear();
            }
        }

        public override void NotifyNewConfig(TargetConfig targetConfig)
        {
            Initialize(targetConfig);
        }

        public override void Log(LogEvent logEvent)
        {
            ConsoleColor foregroundColor = Console.ForegroundColor;
            ConsoleColor backgroundColor = Console.BackgroundColor;

            bool match = false;

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

            if (match)
            {
                lock (ConsoleLock)
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
            }
            else
            {
                Console.Write(Layout.FormatLogEvent(logEvent));
            }

        }

        private Tuple<ConsoleColor?, ConsoleColor?> GetConsoleColors(LogEvent logEvent)
        {
            string flattenedTags = FlattenTags(logEvent.Tags);
            if (RuleCache.ContainsKey(flattenedTags))
            {
                return RuleCache[flattenedTags];
            }
            else
            {
                var tagKeeper = Dispatcher.TagKeeper;
                ConsoleColor? backgroundColor = null;
                ConsoleColor? foregroundColor = null;

                if (ColorRules != null && ColorRules.Count > 0)
                {
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

                            if (match)
                                break;
                        }
                    }
                }

                var consoleColors = new Tuple<ConsoleColor?, ConsoleColor?>(backgroundColor, foregroundColor);
                RuleCache[flattenedTags] = consoleColors;
                return consoleColors;
            }
        }

        private static string FlattenTags(IEnumerable<string> tags)
        {
            return String.Join(",", tags);
        }

    }
}
