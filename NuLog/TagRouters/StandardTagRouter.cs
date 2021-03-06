﻿/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Dispatchers.TagRouters;
using System.Collections.Generic;
using System.Text;

namespace NuLog.TagRouters {

    /// <summary>
    /// The standard implementation of a tag router.
    /// </summary>
    public class StandardTagRouter : ITagRouter {

        /// <summary>
        /// The rule processor for this router.
        /// </summary>
        private readonly IRuleProcessor ruleProcessor;

        /// <summary>
        /// A cache of already processed rules for a given set of tags
        /// </summary>
        private readonly IDictionary<string, IEnumerable<string>> routeCache;

        /// <summary>
        /// Declares a new instance of this standard router, with the given rules.
        /// </summary>
        public StandardTagRouter(IRuleProcessor ruleProcessor) {
            this.ruleProcessor = ruleProcessor;

            this.routeCache = new Dictionary<string, IEnumerable<string>>();
        }

        public IEnumerable<string> Route(IEnumerable<string> tags) {
            // Build our cache key
            var cacheKey = BuildTagsKey(tags);

            // Check to see if our route cache has the entry already
            if (!routeCache.ContainsKey(cacheKey)) {
                routeCache[cacheKey] = this.ruleProcessor.DetermineTargets(tags);
            }

            // Return the route from our cache
            return routeCache[cacheKey];
        }

        /// <summary>
        /// Build and return a string representation of the given tags.
        /// </summary>
        private static string BuildTagsKey(IEnumerable<string> tags) {
            var sb = new StringBuilder();
            foreach (var tag in tags) {
                sb.Append(tag + ";");
            }
            return sb.ToString();
        }
    }
}