/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Dispatchers.TagRouters;
using System;
using System.Collections.Generic;

namespace NuLog.TagRouters
{
    /// <summary>
    /// The standard implementation of a tag router.
    /// </summary>
    public class StandardTagRouter : ITagRouter
    {
        /// <summary>
        /// The rule processor for this router.
        /// </summary>
        private readonly IRuleProcessor ruleProcessor;

        /// <summary>
        /// The tag group processor for this router.
        /// </summary>
        private readonly ITagGroupProcessor tagGroupProcessor;

        /// <summary>
        /// A cache of already processed rules for a given set of tags
        /// </summary>
        private readonly IDictionary<string, IEnumerable<string>> routeCache;

        /// <summary>
        /// Declares a new instance of this standard router, with the given rules.
        /// </summary>
        public StandardTagRouter(IRuleProcessor ruleProcessor)
        {
            this.ruleProcessor = ruleProcessor;

            this.routeCache = new Dictionary<string, IEnumerable<string>>();
        }

        public IEnumerable<string> Route(params string[] tags)
        {
            // Build our cache key
            var cacheKey = BuildTagsKey(tags);

            // Check to see if our route cache has the entry already
            if (routeCache.ContainsKey(cacheKey) == false)
            {
                routeCache[cacheKey] = this.ruleProcessor.DetermineTargets(tags);
            }

            // Return the route from our cache
            return routeCache[cacheKey];
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
    }
}