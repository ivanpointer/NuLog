/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Dispatchers.TagRouters;
using System.Collections.Generic;
using System.Linq;

namespace NuLog.TagRouters.TagGroupProcessors {

    /// <summary>
    /// The standard implementation of a tag group processor.
    /// </summary>
    public class StandardTagGroupProcessor : ITagGroupProcessor {
        private readonly IEnumerable<TagGroup> tagGroups;

        private readonly IDictionary<string, string[]> aliasCache;

        public StandardTagGroupProcessor(IEnumerable<TagGroup> tagGroups) {
            this.tagGroups = tagGroups;

            this.aliasCache = new Dictionary<string, string[]>();
        }

        public IEnumerable<string> GetAliases(string tag) {
            // Fist, check our cache
            if (!aliasCache.ContainsKey(tag)) {
                var aliases = CalculateAliases(tag);
                aliasCache[tag] = aliases.ToArray();
            }

            // Return the aliases from cache
            return aliasCache[tag];
        }

        private IEnumerable<string> CalculateAliases(string tag) {
            yield return tag;

            if (tagGroups != null) {
                foreach (var tagGroup in tagGroups) {
                    foreach (var alias in tagGroup.Aliases) {
                        if (alias == tag) {
                            yield return tagGroup.BaseTag;
                        }
                    }
                }
            }
        }
    }
}