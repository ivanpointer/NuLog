﻿/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;

namespace NuLog.Configuration
{
    /// <summary>
    /// The configuration needed by the factories to build out the parts of NuLog.
    /// </summary>
    public class Config
    {
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
    }
}