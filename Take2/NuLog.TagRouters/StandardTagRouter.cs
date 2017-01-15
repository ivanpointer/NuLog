/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Dispatchers.TagRouters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NuLog.TagRouters
{
    /// <summary>
    /// The standard implementation of a tag router.
    /// </summary>
    public class StandardTagRouter : ITagRouter
    {
        /// <summary>
        /// The list of rules that this router is working off of.
        /// </summary>
        private readonly IEnumerable<Rule> rules;

        // A regular expression for validating the tags coming in.
        private static readonly Regex tagValPattern = new Regex(@"[a-zA-Z0-9_\.]+", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        // Cache the computed regex patterns for our rule tags
        private readonly IDictionary<string, Regex> ruleTagPatterns;

        // A cache of already processed rules for a given set of tags
        private readonly IDictionary<string, IEnumerable<string>> routeCache;

        /// <summary>
        /// Declares a new instance of this standard router, with the given rules.
        /// </summary>
        public StandardTagRouter(IEnumerable<Rule> rules)
        {
            this.rules = rules ?? new Rule[] { };

            this.ruleTagPatterns = new Dictionary<string, Regex>();

            this.routeCache = new Dictionary<string, IEnumerable<string>>();
        }

        public IEnumerable<string> Route(params string[] tags)
        {
            // Build our cache key
            var cacheKey = BuildTagsKey(tags);

            // Check to see if our route cache has the entry already
            if (routeCache.ContainsKey(cacheKey) == false)
            {
                routeCache[cacheKey] = CalculateRoute(tags);
            }

            // Return the route from our cache
            return routeCache[cacheKey];
        }

        #region Route Caching

        /// <summary>
        /// Calculates the route for the given tags.
        /// </summary>
        private IEnumerable<string> CalculateRoute(string[] tags)
        {
            // Safety first - make sure the tags are formatted properly
            ValidateTags(tags);

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
        /// Build and return a string representation of the given tags.
        /// </summary>
        private static string BuildTagsKey(string[] tags)
        {
            var copy = new string[tags.Length];
            tags.CopyTo(copy, 0);
            Array.Sort(copy);
            var conjoined = string.Join(";", copy);
            return conjoined.ToLower();
        }

        #endregion Route Caching

        #region Tag Validation

        /// <summary>
        /// Checks the tags to make sure they look legit. Will throw an InvalidOperationException if
        /// a tag is found that isn't compliant.
        /// </summary>
        /// <param name="tags">The tags to check.</param>
        private void ValidateTags(string[] tags)
        {
            foreach (var tag in tags)
            {
                if (!tagValPattern.IsMatch(tag))
                {
                    throw new InvalidOperationException(string.Format("tag \"{0}\" contains invalid characters.", tag));
                }
            }
        }

        #endregion Tag Validation

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

            // Run the check
            return ruleTagPattern.IsMatch(tag);
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