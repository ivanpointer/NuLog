﻿/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/7/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using NuLog.Configuration.Targets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NuLog.Configuration
{
    /// <summary>
    /// A helper class for building logging configurations at runtime
    /// </summary>
    public class LoggingConfigBuilder : ILoggingConfigBuilder
    {
        private IList<TargetConfig> Targets { get; set; }
        private IList<RuleConfig> Rules { get; set; }
        private IList<TagGroupConfig> TagGroups { get; set; }
        public Action<Exception, string> ExceptionHandler { get; set; }
        public bool Debug { get; set; }

        /// <summary>
        /// A private constructor for the factory method/method chaining pattern.  Builds an empty configuration builder.
        /// </summary>
        private LoggingConfigBuilder()
        {
            Targets = new List<TargetConfig>();
            Rules = new List<RuleConfig>();
            TagGroups = new List<TagGroupConfig>();
            ExceptionHandler = null;
        }

        /// <summary>
        /// Creates an instance of the LoggingConfigBuilder
        /// </summary>
        /// <returns>A new instance of the LoggingConfigBuilder</returns>
        public static LoggingConfigBuilder CreateLoggingConfig()
        {
            return new LoggingConfigBuilder();
        }

        /// <summary>
        /// Adds a target to the configuration
        /// </summary>
        /// <param name="targetConfig">The target to add</param>
        /// <returns>This LoggingConfigBuilder</returns>
        public LoggingConfigBuilder AddTarget(TargetConfig targetConfig)
        {
            Targets.Add(targetConfig);
            return this;
        }

        /// <summary>
        /// Adds a rule to the configuration
        /// </summary>
        /// <param name="rule">The rule to add</param>
        /// <returns>This LoggingConfigBuilder</returns>
        public LoggingConfigBuilder AddRule(RuleConfig rule)
        {
            Rules.Add(rule);
            return this;
        }

        /// <summary>
        /// Adds a tag group to the configuration
        /// </summary>
        /// <param name="tagGroup">The tag group to add to the configuration</param>
        /// <returns>This LoggingConfigBuilder</returns>
        public LoggingConfigBuilder AddTagGroup(TagGroupConfig tagGroup)
        {
            TagGroups.Add(tagGroup);
            return this;
        }

        /// <summary>
        /// Adds a new tag group to the configuration
        /// </summary>
        /// <param name="tag">The tag to group the child tags under</param>
        /// <param name="childTags">The child tags to add to the group</param>
        /// <returns>This LoggingConfigBuilder</returns>
        public LoggingConfigBuilder AddTagGroup(string tag, params string[] childTags)
        {
            TagGroups.Add(new TagGroupConfig
            {
                Tag = tag,
                ChildTags = new List<string>(childTags)
            });
            return this;
        }

        /// <summary>
        /// Sets the exception handler to the logging configuration
        /// </summary>
        /// <param name="exceptionHandler">The exception handler to set into the configuration</param>
        /// <returns>This LoggingConfigBuilder</returns>
        public LoggingConfigBuilder SetExceptionHandler(Action<Exception, string> exceptionHandler)
        {
            ExceptionHandler = exceptionHandler;
            return this;
        }

        /// <summary>
        /// Sets or unsets the debug flag
        /// </summary>
        /// <param name="debug">The debug flag</param>
        /// <returns>This LoggingConfigBuilder</returns>
        public LoggingConfigBuilder SetDebug(bool debug)
        {
            Debug = debug;
            return this;
        }

        /// <summary>
        /// Builds the logging config based on the values assigned to this config builder
        /// </summary>
        /// <returns>A built logging config</returns>
        public LoggingConfig Build()
        {
            if (this.Rules.Count == 0)
            {
                this.Rules.Add(new RuleConfig
                {
                    Include = new List<string> { "*" },
                    WriteTo = (from target in Targets select target.Name).ToList()
                });
            }

            return new LoggingConfig(loadConfig: false)
            {
                Targets = this.Targets,
                Rules = this.Rules,
                TagGroups = this.TagGroups,
                ExceptionHandler = this.ExceptionHandler,
                Debug = this.Debug
            };
        }
    }
}
