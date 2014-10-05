using NuLog.Configuration.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Configuration
{
    public class LoggingConfigBuilder : ILoggingConfigBuilder
    {
        private IList<TargetConfig> Targets { get; set; }
        private IList<RuleConfig> Rules { get; set; }
        private IList<TagGroupConfig> TagGroups { get; set; }
        public Action<Exception, string> ExceptionHandler { get; set; }
        public bool Debug { get; set; }

        private LoggingConfigBuilder()
        {
            Targets = new List<TargetConfig>();
            Rules = new List<RuleConfig>();
            TagGroups = new List<TagGroupConfig>();
            ExceptionHandler = null;
        }

        public static LoggingConfigBuilder CreateLoggingConfig()
        {
            return new LoggingConfigBuilder();
        }

        public LoggingConfigBuilder AddTarget(TargetConfig targetConfig)
        {
            Targets.Add(targetConfig);
            return this;
        }

        public LoggingConfigBuilder AddRule(RuleConfig rule)
        {
            Rules.Add(rule);
            return this;
        }

        public LoggingConfigBuilder AddTagGroup(TagGroupConfig tagGroup)
        {
            TagGroups.Add(tagGroup);
            return this;
        }

        public LoggingConfigBuilder AddTagGroup(string tag, params string[] childTags)
        {
            TagGroups.Add(new TagGroupConfig
            {
                Tag = tag,
                ChildTags = new List<string>(childTags)
            });
            return this;
        }

        public LoggingConfigBuilder SetExceptionHandler(Action<Exception, string> exceptionHandler)
        {
            ExceptionHandler = exceptionHandler;
            return this;
        }

        public LoggingConfigBuilder SetDebug(bool debug)
        {
            Debug = debug;
            return this;
        }

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
