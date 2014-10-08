/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/7/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using System.Collections.Generic;

namespace NuLog.Configuration
{
    /// <summary>
    /// Defines a rule
    /// </summary>
    public class RuleConfig
    {
        /// <summary>
        /// Default constructor.  Initializes the lists as empty lists.
        /// </summary>
        public RuleConfig()
        {
            Include = new List<string>();
            StrictInclude = false;
            Exclude = new List<string>();
            WriteTo = new List<string>();
            Final = false;
        }

        /// <summary>
        /// The list of tags to include in the matching rule.  Allows for * wild cards.
        /// </summary>
        public ICollection<string> Include { get; set; }
        /// <summary>
        /// Determines if all tags listed in "Include" must be present in the log event for it to be considered a match
        /// </summary>
        public bool StrictInclude { get; set; }
        /// <summary>
        /// A list of tags to explicitly exclude from the rule
        /// </summary>
        public ICollection<string> Exclude { get; set; }
        /// <summary>
        /// A list of target names to delegate matching log events to
        /// </summary>
        public ICollection<string> WriteTo { get; set; }
        /// <summary>
        /// Indicates whether further rules are to be processed if the log event matches this rule
        /// </summary>
        public bool Final { get; set; }
    }
}
