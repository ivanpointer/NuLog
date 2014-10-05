using NuLog.Configuration.Layouts;
using NuLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Configuration.Targets
{
    public class ConsoleTargetConfigBuilder
    {
        protected string Name { get; set; }
        protected bool? Synchronous { get; set; }
        protected LayoutConfig LayoutConfig { get; set; }
        protected ICollection<ConsoleColorRule> ColorRules { get; set; }

        private ConsoleTargetConfigBuilder()
        {
            Name = "console";
            Synchronous = null;
            LayoutConfig = new LayoutConfig();
            ColorRules = new List<ConsoleColorRule>();
        }

        public static ConsoleTargetConfigBuilder Create()
        {
            return new ConsoleTargetConfigBuilder();
        }

        public ConsoleTargetConfigBuilder SetName(string name)
        {
            Name = name;
            return this;
        }

        public ConsoleTargetConfigBuilder SetSynchronous(bool synchronous)
        {
            Synchronous = synchronous;
            return this;
        }

        public ConsoleTargetConfigBuilder SetLayoutConfig(LayoutConfig layoutConfig)
        {
            LayoutConfig = layoutConfig;
            return this;
        }

        public ConsoleTargetConfigBuilder AddConsoleColorRule(ConsoleColorRule consoleColorRule)
        {
            ColorRules.Add(consoleColorRule);
            return this;
        }

        public ConsoleTargetConfigBuilder AddConsoleColorRule(string foregroundColor, string backgroundColor, params string[] tags)
        {
            ConsoleColorRule rule = new ConsoleColorRule();
            rule.Tags = tags;

            ConsoleColor? fColor = GetColorByName(foregroundColor);
            ConsoleColor? bColor = GetColorByName(backgroundColor);

            if (fColor.HasValue)
                rule.Color = fColor.Value;

            if (bColor.HasValue)
                rule.BackgroundColor = bColor.Value;

            ColorRules.Add(rule);

            return this;
        }

        public ConsoleTargetConfig Build()
        {
            return new ConsoleTargetConfig
            {
                Name = this.Name,
                Type = typeof(ConsoleTarget).FullName,
                Synchronous = this.Synchronous,
                LayoutConfig = this.LayoutConfig,
                ColorRules = this.ColorRules
            };
        }

        #region Helpers

        private static readonly Type ConsoleColorType = typeof(ConsoleColor);
        private static ConsoleColor? GetColorByName(string colorName)
        {
            if (String.IsNullOrEmpty(colorName) == false)
            {
                try
                {
                    return (ConsoleColor)Enum.Parse(ConsoleColorType, colorName);
                }
                catch { }
            }
            return null;
        }

        #endregion

    }
}
