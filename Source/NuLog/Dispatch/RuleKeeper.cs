/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/7/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Configuration;
using System;
using System.Collections.Generic;

namespace NuLog.Dispatch
{
    /// <summary>
    /// Responsible for "keeping the rules".  Uses the configured tag groups and rules to determine which
    ///   targets a particular set of tags are to be dispatched to
    /// </summary>
    public class RuleKeeper : IConfigObserver
    {
        #region Constants
        
        // We limit the number of routes that are cached
        //  a route is simply a definition of which targets a particular set of tags dispatch to
        private const int _routeCacheLimit = 1000;

        #endregion

        #region Members and Constructors

        // The rules that define which tags are to be dispatched to which targets
        private static readonly object _ruleLock = new object();
        private ICollection<RuleConfig> Rules { get; set; }

        // A cache of which tags go to which targets
        private IDictionary<string, ICollection<string>> _routeCache;
        private Queue<string> _routeKeyCache;

        // The tag keeper - which is repsonsible for determining
        //  tag matches based on wild-cards and tag-groups
        private TagKeeper TagKeeper { get; set; }

        /// <summary>
        /// The default constructor
        /// </summary>
        /// <param name="tagKeeper">The tag keeper to use in determining tag matches</param>
        public RuleKeeper(TagKeeper tagKeeper)
        {
            TagKeeper = tagKeeper;
            _routeCache = new Dictionary<string, ICollection<string>>();
            _routeKeyCache = new Queue<string>();
        }

        #endregion

        #region Work
        
        /// <summary>
        /// Determines a list of target names to be dispatched to based on the rules defined in the configuration, based on the tags passed
        /// </summary>
        /// <param name="tags">The tags to use for matching to targets using the rules</param>
        /// <returns>A list of targets that are to be dispatched to based on the configured rules and the passed tags</returns>
        public ICollection<string> GetTargetsForTags(ICollection<string> tags)
        {
            ICollection<string> targets = new List<string>();

            lock (_ruleLock)
            {
                // First check to see if we have already figured this route out
                var route = FlattenTags(tags);
                if (_routeCache.ContainsKey(route))
                {
                    targets = _routeCache[route];
                }
                else
                {
                    // We haven't figured this route out yet, we need to now
                    //  Iterate over each of the rules, looking for matches
                    bool doInclude;
                    foreach (var ruleConfig in Rules)
                    {
                        // Use the tag keeper to see if the tags match the "include" tags on the rule
                        //  including a consideration for the "Strict include" flag
                        doInclude = ruleConfig.Include != null && ruleConfig.Include.Count > 0
                            ? TagKeeper.CheckMatch(tags, ruleConfig.Include, ruleConfig.StrictInclude)
                            : true;

                        // Use the tag keeper to make sure that the tags do not match the "exclude" tags
                        //  on the rule
                        doInclude = doInclude
                            && (ruleConfig.Exclude == null
                                || ruleConfig.Exclude.Count == 0
                                || !TagKeeper.CheckMatch(tags, ruleConfig.Exclude));

                        // If we have determined that the tags match this rule
                        if (doInclude)
                        {
                            // Add each of the targets defined by this rule to the list
                            //  of targets to dispatch to
                            foreach (var target in ruleConfig.WriteTo)
                                if (targets.Contains(target) == false)
                                    targets.Add(target);

                            // If this rule is marked as final,
                            //  we are done checking rules
                            if (ruleConfig.Final)
                                break;
                        }
                    }

                    // Cleanup the route cache if we are full
                    if (_routeCache.Count > _routeCacheLimit)
                    {
                        var oldKey = _routeKeyCache.Dequeue();
                        _routeCache.Remove(oldKey);
                    }

                    // Cache the route
                    _routeKeyCache.Enqueue(route);  //First in, first out
                    _routeCache[route] = targets;
                }
            }

            // Return the findings
            return targets;
        }

        #endregion

        #region Configuration

        /// <summary>
        /// Notifies this rule keeper of a new configuration
        /// </summary>
        /// <param name="loggingConfig">The new configuration</param>
        public void NotifyNewConfig(LoggingConfig loggingConfig)
        {
            lock (_ruleLock)
            {
                // If rules are defined, assign them
                if (loggingConfig.Rules != null && loggingConfig.Rules.Count > 0)
                {
                    Rules = loggingConfig.Rules;
                }
                else
                {
                    // Otherwise, route everything everywhere
                    var newRule = RuleConfigBuilder.CreateRuleConfig()
                        .AddWriteTo("*")
                        .Build();
                    Rules = new List<RuleConfig> { newRule };
                }

                // Clear the cache
                _routeKeyCache.Clear();
                _routeCache.Clear();
            }
        }

        #endregion

        #region Helpers

        // A helper functionn for flattening the tags into a single string
        //  This is used for caching routes
        private static string FlattenTags(IEnumerable<string> tags)
        {
            return String.Join(",", tags);
        }

        #endregion

    }
}
