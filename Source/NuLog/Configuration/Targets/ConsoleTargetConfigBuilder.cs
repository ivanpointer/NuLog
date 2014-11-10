/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/5/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using NuLog.Configuration.Layouts;
using NuLog.Targets;
using System;
using System.Collections.Generic;

namespace NuLog.Configuration.Targets
{
    /// <summary>
    /// Used to build a console target config at runtime
    /// </summary>
    public class ConsoleTargetConfigBuilder
    {
        /// <summary>
        /// Represents the name of the target
        /// </summary>
        protected string Name { get; set; }
        /// <summary>
        /// Represents the synchronous flag of the target
        /// </summary>
        protected bool? Synchronous { get; set; }
        /// <summary>
        /// Represents the layout configuration of the target
        /// </summary>
        protected LayoutConfig LayoutConfig { get; set; }
        /// <summary>
        /// Represents the color rules of the target
        /// </summary>
        protected ICollection<ConsoleColorRule> ColorRules { get; set; }

        /// <summary>
        /// Builds a default configuration builder
        /// </summary>
        private ConsoleTargetConfigBuilder()
        {
            Name = "console";
            Synchronous = null;
            LayoutConfig = new LayoutConfig();
            ColorRules = new List<ConsoleColorRule>();
        }

        /// <summary>
        /// Gets a new instance of this console target config builder
        /// </summary>
        /// <returns>A new instance of this console target config builder</returns>
        public static ConsoleTargetConfigBuilder Create()
        {
            return new ConsoleTargetConfigBuilder();
        }

        /// <summary>
        /// Sets the represented name
        /// </summary>
        /// <param name="name">Represents the name of the target</param>
        /// <returns>This console target config builder instance</returns>
        public ConsoleTargetConfigBuilder SetName(string name)
        {
            Name = name;
            return this;
        }

        /// <summary>
        /// Sets the represented synchronous flag
        /// </summary>
        /// <param name="synchronous">Represents the synchronous flag of the target</param>
        /// <returns>This console target config builder instance</returns>
        public ConsoleTargetConfigBuilder SetSynchronous(bool synchronous)
        {
            Synchronous = synchronous;
            return this;
        }

        /// <summary>
        /// Sets a standard layout with the given layout format
        /// </summary>
        /// <param name="layoutFormat">The layout format to use for the new standard layout config</param>
        /// <returns>This console target config builder instance</returns>
        public ConsoleTargetConfigBuilder SetLayoutConfig(string layoutFormat)
        {
            LayoutConfig = new LayoutConfig(layoutFormat);
            return this;
        }

        /// <summary>
        /// Sets the layout configuration
        /// </summary>
        /// <param name="layoutConfig">The layout configuration to use</param>
        /// <returns>This console target config builder instance</returns>
        public ConsoleTargetConfigBuilder SetLayoutConfig(LayoutConfig layoutConfig)
        {
            LayoutConfig = layoutConfig;
            return this;
        }

        /// <summary>
        /// Adds a color rule to the list of color rules
        /// </summary>
        /// <param name="consoleColorRule">The color rule to add</param>
        /// <returns>This console target config builder instance</returns>
        public ConsoleTargetConfigBuilder AddConsoleColorRule(ConsoleColorRule consoleColorRule)
        {
            ColorRules.Add(consoleColorRule);
            return this;
        }

        /// <summary>
        /// Adds a color rule to the list of color rules
        /// </summary>
        /// <param name="foregroundColor">The foreground color of the rule</param>
        /// <param name="backgroundColor">The background color of the rule</param>
        /// <param name="tags">The tags used to match log events to the rule</param>
        /// <returns>This console target config builder instance</returns>
        public ConsoleTargetConfigBuilder AddConsoleColorRule(string foregroundColor, string backgroundColor, params string[] tags)
        {
            // Create a new rule and set the passed tags
            ConsoleColorRule rule = new ConsoleColorRule();
            rule.Tags = tags;

            // Parse the colors from the passed in string color names
            ConsoleColor? fColor = GetColorByName(foregroundColor);
            ConsoleColor? bColor = GetColorByName(backgroundColor);

            // And assign them if they are found
            if (fColor.HasValue)
                rule.ForegroundColor = fColor.Value;

            if (bColor.HasValue)
                rule.BackgroundColor = bColor.Value;

            // Add the new rule to the list
            ColorRules.Add(rule);

            return this;
        }

        /// <summary>
        /// Builds out the console target config using the vaules set into this console target config builder
        /// </summary>
        /// <returns>A console target config built using the vaules set into this console target config</returns>
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

        /// <summary>
        /// Returns a ConsoleColor based on the given string representation of the color
        /// </summary>
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
