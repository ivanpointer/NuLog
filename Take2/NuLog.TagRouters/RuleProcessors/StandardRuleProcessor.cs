/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Dispatchers.TagRouters;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NuLog.TagRouters.RuleProcessors
{
    /// <summary>
    /// The standard implementation of a rule processor.
    /// </summary>
    public class StandardRuleProcessor : IRuleProcessor
    {
        /// <summary>
        /// The list of rules that this router is working off of.
        /// </summary>
        private readonly IEnumerable<Rule> rules;

        /// <summary>
        /// The tag group processor for determining aliases.
        /// </summary>
        private readonly ITagGroupProcessor tagGroupProcessor;

        /// <summary>
        /// Cache the computed regex patterns for our rule tags.
        /// </summary>
        private readonly IDictionary<string, Regex> ruleTagPatterns;

        /// <summary>
        /// Builds an instance of this standard rule processor, for the given set of rules.
        /// </summary>
        public StandardRuleProcessor(IEnumerable<Rule> rules, ITagGroupProcessor tagGroupProcessor)
        {
            this.rules = rules ?? new Rule[] { };

            this.tagGroupProcessor = tagGroupProcessor;

            this.ruleTagPatterns = new Dictionary<string, Regex>();
        }

        public IEnumerable<string> DetermineTargets(IEnumerable<string> tags)
        {
            // Our distinct set of targets to route to
            var targets = new HashSet<string>();

            // Ready, set... SEARCH!
            foreach (var rule in rules)
            {
                // First, check for any disqualification
                if (IsExcludeMatch(rule, tags))
                {
                    // The rule is disqualified, continue
                    continue;
                }

                // The rule isn't disqualified, check to see if it matches
                if (IsIncludeMatch(rule, tags))
                {
                    // Add all the targets
                    foreach (var target in rule.Targets)
                    {
                        targets.Add(target);
                    }
                }

                // Check for the final flag on the rule
                if (rule.Final)
                {
                    // The rule is marked final, don't process any more rules
                    break;
                }
            }

            // Return our found targets
            return targets;
        }

        /// <summary>
        /// Determine if the list of tags is a match for the "include" rules for the given rule.
        /// </summary>
        /// <param name="rule">The rule being checked.</param>
        /// <param name="tags">The tags to check against the rule.</param>
        private bool IsIncludeMatch(Rule rule, IEnumerable<string> tags)
        {
            var anyMatch = false;

            // Start checking includes
            foreach (var ruleTag in rule.Include)
            {
                var ruleMatch = false;

                // Check each given tag against this include
                foreach (var tag in tags)
                {
                    if (IsTagMatch(ruleTag, tag))
                    {
                        // We found a match, signal and break
                        anyMatch = true;
                        ruleMatch = true;
                        break;
                    }
                }

                // Switch based on strict mode
                if (ruleMatch && rule.StrictInclude == false)
                {
                    // Strict mode is off, which means any one match will work -
                    return true;
                }
                else if (ruleMatch == false && rule.StrictInclude)
                {
                    return false;
                }
            }

            // Did anything match?
            return anyMatch;
        }

        /// <summary>
        /// Determine if the rule is disqualified based on any listed "exclude" tags in the rule.
        /// </summary>
        /// <param name="rule">The rule being checked.</param>
        /// <param name="tags">The tags to check against the rule.</param>
        /// <returns>True if the rule is disqualified, False otherwise.</returns>
        private bool IsExcludeMatch(Rule rule, IEnumerable<string> tags)
        {
            // If there are no exclude rules, there's nothing to disqualify
            if (rule.Exclude == null || rule.Exclude.Count() == 0)
            {
                return false;
            }

            // Check each exclude rule for disqualification
            foreach (var ruleTag in rule.Exclude)
            {
                foreach (var tag in tags)
                {
                    if (IsTagMatch(ruleTag, tag))
                    {
                        // We matched an exclude tag - the rule is disqualified.
                        return true;
                    }
                }
            }

            // We didn't match any exclude tags, the rule is not disqualified.
            return false;
        }

        #region Tag Matching

        /// <summary>
        /// Checks to see if the given tag (from a log event, etc.) matches the tag in the rule.
        ///
        /// Returns True if they are a match.
        /// </summary>
        private bool IsTagMatch(string ruleTag, string tag)
        {
            // Get our rule tag pattern
            var ruleTagPattern = GetRuleTagPattern(ruleTag);

            // Iterate over the aliases of the group, checking each for a match
            var anyMatch = false;
            foreach (var alias in tagGroupProcessor.GetAliases(tag))
            {
                // Run the check
                if (ruleTagPattern.IsMatch(alias))
                {
                    anyMatch = true;
                    break;
                }
            }

            // If any aliases matched, it's a match
            return anyMatch;
        }

        /// <summary>
        /// Computes, caches and returns the regex pattern for a given rule tag.
        /// </summary>
        private Regex GetRuleTagPattern(string ruleTag)
        {
            if (ruleTagPatterns.ContainsKey(ruleTag) == false)
            {
                // First, make safe any dots in the rule.
                var regexRuleTag = ruleTag.Replace(".", @"\.");

                // Convert our simple wild-card "*" to the regex wild-card ".*"
                regexRuleTag = ruleTag.Replace("*", ".*");

                // Build our pattern
                var ruleTagPattern = new Regex(regexRuleTag, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled);

                // Cache it
                ruleTagPatterns[ruleTag] = ruleTagPattern;
            }

            // Return our rule tag
            return ruleTagPatterns[ruleTag];
        }

        #endregion Tag Matching
    }
}