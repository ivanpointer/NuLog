/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;

namespace NuLog.Configuration {

    /// <summary>
    /// The configuration needed by the factories to build out the parts of NuLog.
    /// </summary>
    public class Config {

        /// <summary>
        /// The rules within this configuration.
        /// </summary>
        public ICollection<RuleConfig> Rules { get; set; }

        /// <summary>
        /// The tag groups within this configuration.
        /// </summary>
        public ICollection<TagGroupConfig> TagGroups { get; set; }

        /// <summary>
        /// The targets within this configuration.
        /// </summary>
        public ICollection<TargetConfig> Targets { get; set; }

        /// <summary>
        /// Default meta data defined within this configuration.
        /// </summary>
        public IDictionary<string, string> MetaData { get; set; }

        /// <summary>
        /// When True, creates loggers with their IncludeStackFrame flag set to true - which causes
        /// the logger to include the stack frame in created log events.
        /// </summary>
        public bool IncludeStackFrame { get; set; }

        /// <summary>
        /// The (optional) path to a fallback log, which is used when failure occurs when logging a
        /// log event. Can be useful when there are problems with the logging system, such as
        /// programming errors in components, or configuration issues.
        /// </summary>
        public string FallbackLogPath { get; set; }
    }
}