/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;

namespace NuLog.Dispatchers.TagRouters {

    /// <summary>
    /// Defines a single rule for tag routing. See the comments around each property to see how the
    /// rules should work.
    /// </summary>
    public class Rule {

        /// <summary>
        /// The tags that trigger a match on this rule. See <see cref="Rule.StrictInclude" /> to see
        /// how that flag changes the behavior around these tags.
        /// </summary>
        public IEnumerable<string> Include { get; set; }

        /// <summary>
        /// The tags which disqualify for matching. There is no "strict" mode for these, any match on
        /// these will disqualify the rule.
        /// </summary>
        public IEnumerable<string> Exclude { get; set; }

        /// <summary>
        /// The names of the targets that the rule routes to.
        /// </summary>
        public IEnumerable<string> Targets { get; set; }

        /// <summary>
        /// When this is left to the default "false", only one of the tags in <see cref="Include" />
        /// need match for the rule to match. However, if this is set to "true", all tags in <see
        /// cref="Include" /> need to match in order for the rule to match.
        /// </summary>
        public bool StrictInclude { get; set; }

        /// <summary>
        /// When a rule is marked final, and it is matched, the router should process no further
        /// rules. This means that the routers should execute rules in the order they are given.
        /// </summary>
        public bool Final { get; set; }
    }
}