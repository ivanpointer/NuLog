using NuLog.Configuration.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Targets
{
    public class ConsoleTarget : SimpleConsoleTarget
    {
        public const string MetaForeground = "ConsoleForeground";
        public const string MetaBackground = "ConsoleBackground";

        private static readonly object ConsoleLock = new object();
        private ICollection<ConsoleColorRule> ColorRules { get; set; }

        public ConsoleTarget()
        {
            ColorRules = new List<ConsoleColorRule>();
        }

        public override void Initialize(TargetConfig targetConfig, bool? synchronous = null)
        {
            base.Initialize(targetConfig, synchronous.HasValue ? synchronous : true); // Default to synchronous

            if (targetConfig != null)
            {
                ConsoleTargetConfig consoleConfig = typeof(ConsoleTargetConfig).IsAssignableFrom(targetConfig.GetType())
                    ? (ConsoleTargetConfig)targetConfig
                    : new ConsoleTargetConfig(targetConfig.Config);

                ColorRules = consoleConfig.ColorRules;
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

            if (ColorRules != null && ColorRules.Count > 0)
            {
                var tagKeeper = Dispatcher.TagKeeper;
                foreach (var colorRule in ColorRules)
                {
                    if (tagKeeper.CheckMatch(logEvent.Tags, colorRule.Tags))
                    {
                        if (colorRule.Color != null)
                        {
                            foregroundColor = colorRule.Color.Value;
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

            if (logEvent.MetaData != null)
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

                        base.Log(logEvent);
                    }
                    finally
                    {
                        Console.ResetColor();
                    }
                }
            }
            else
            {
                base.Log(logEvent);
            }

        }
    }
}
