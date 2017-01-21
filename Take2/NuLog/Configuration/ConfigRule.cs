/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;

namespace NuLog.Configuration
{
    /// <summary>
    /// Represents a single rule in a NuLog configuration.
    /// </summary>
    public class ConfigRule
    {
        /// <summary>
        /// The include tags for this rule.
        /// </summary>
        public ICollection<string> Includes { get; set; }

        /// <summary>
        /// The exclude tags for this rule.
        /// </summary>
        public ICollection<string> Excludes { get; set; }

        /// <summary>
        /// The target names for this rule.
        /// </summary>
        public ICollection<string> Targets { get; set; }

        /// <summary>
        /// True indicates that all tags in the "includes" list must match for the rule to apply.
        /// </summary>
        public bool StrictInclude { get; set; }

        /// <summary>
        /// True indicates that if the rule is matched, that no further rules should be processed.
        /// </summary>
        public bool Final { get; set; }
    }
}