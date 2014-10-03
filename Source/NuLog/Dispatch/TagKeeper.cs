using NuLog.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Dispatch
{
    public class TagKeeper : IConfigObserver
    {
        private static readonly object _tagLock = new object();
        private IDictionary<string, ICollection<string>> TagDefs { get; set; }

        public TagKeeper()
        {
            TagDefs = new Dictionary<string, ICollection<string>>();
        }

        public bool CheckMatch(string tag, string mustMatch, bool strict = false)
        {
            return CheckMatch(new List<string> { tag }, new List<string> { mustMatch }, strict);
        }

        public bool CheckMatch(ICollection<string> tags, ICollection<string> mustMatch, bool strict = false)
        {
            lock (_tagLock)
            {
                var formattedTags = FormatTags(tags);
                var formattedMustMatch = FormatTags(mustMatch);

                bool allFound = true;
                bool anyFound = false;
                bool tagFound = false;
                ICollection<string> expandedMustMatch;
                foreach (var matchTag in formattedMustMatch)
                {
                    expandedMustMatch = ExpandTagList(TagDefs, matchTag);
                    tagFound = false;
                    foreach (var tag in formattedTags)
                    {
                        if (expandedMustMatch.Any(mustMatchTag => MatchWildcardString(mustMatchTag, tag)))
                        {
                            tagFound = true;
                            break;
                        }
                    }

                    anyFound = anyFound || tagFound;
                    allFound = allFound && tagFound;

                    if (anyFound && !strict)
                        break;
                }

                return strict
                    ? anyFound && allFound
                    : anyFound;
            }
        }

        public void NotifyNewConfig(LoggingConfig loggingConfig)
        {
            lock (_tagLock)
            {
                UpdateTagDefs(TagDefs, loggingConfig.TagGroups);
            }
        }

        #region Helpers

        private static ICollection<string> ExpandTagList(IDictionary<string, ICollection<string>> tagDefs, string tag)
        {
            return ExpandTagList(tagDefs, new List<string> { tag });
        }

        private static ICollection<string> ExpandTagList(IDictionary<string, ICollection<string>> tagDefs, ICollection<string> tags)
        {
            var expandedList = new List<string>();
            var formattedTags = FormatTags(tags);

            foreach (var tag in formattedTags)
            {
                if (expandedList.Contains(tag) == false)
                    expandedList.Add(tag);

                if (tagDefs.ContainsKey(tag))
                    foreach (var childTag in tagDefs[tag])
                        if (expandedList.Contains(childTag) == false)
                            expandedList.Add(childTag);
            }

            return expandedList;
        }

        private static void UpdateTagDefs(IDictionary<string, ICollection<string>> tagDefs, ICollection<TagGroupConfig> tagGroupConfigs)
        {
            tagDefs.Clear();
            if (tagGroupConfigs != null && tagGroupConfigs.Count > 0)
                foreach (TagGroupConfig tagGroupConfig in tagGroupConfigs)
                    UpdateTagDefsRecurse(tagDefs, tagGroupConfig.Tag, tagGroupConfig.ChildTags);
        }

        private static void UpdateTagDefsRecurse(IDictionary<string, ICollection<string>> tagDefs, string parentTag, ICollection<string> childTags)
        {
            string fTag = FormatTag(parentTag);
            ICollection<string> fChildTags = FormatTags(childTags);
            ICollection<string> finalTags;

            if (!tagDefs.ContainsKey(fTag))
            {
                finalTags = new List<string>();
                tagDefs.Add(fTag, finalTags);
            }
            else
            {
                finalTags = tagDefs[fTag];
            }

            foreach (string childTag in fChildTags)
            {
                if (!finalTags.Contains(childTag))
                {
                    finalTags.Add(childTag);
                    if (tagDefs.ContainsKey(childTag))
                        UpdateTagDefsRecurse(tagDefs, parentTag, tagDefs[parentTag]);
                }
            }
        }

        private static ICollection<string> FormatTags(ICollection<string> tags)
        {
            ICollection<string> fTags = new List<string>();
            foreach (string tag in tags)
                fTags.Add(FormatTag(tag));
            return fTags;
        }

        private static string FormatTag(string tag)
        {
            return tag.ToLower();
        }

        public Boolean MatchWildcardString(string pattern, string input)
        {
            if (String.Compare(pattern, input) == 0)
            {
                return true;
            }
            else if (String.IsNullOrEmpty(input))
            {
                if (String.IsNullOrEmpty(pattern.Trim(new Char[1] { '*' })))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (pattern.Length == 0)
            {
                return false;
            }
            else if (pattern[0] == '?')
            {
                return MatchWildcardString(pattern.Substring(1), input.Substring(1));
            }
            else if (pattern[pattern.Length - 1] == '?')
            {
                return MatchWildcardString(pattern.Substring(0, pattern.Length - 1), input.Substring(0, input.Length - 1));
            }
            else if (pattern[0] == '*')
            {
                if (MatchWildcardString(pattern.Substring(1), input))
                {
                    return true;
                }
                else
                {
                    return MatchWildcardString(pattern, input.Substring(1));
                }
            }
            else if (pattern[pattern.Length - 1] == '*')
            {
                if (MatchWildcardString(pattern.Substring(0, pattern.Length - 1), input))
                {
                    return true;
                }
                else
                {
                    return MatchWildcardString(pattern, input.Substring(0, input.Length - 1));
                }
            }
            else if (pattern[0] == input[0])
            {
                return MatchWildcardString(pattern.Substring(1), input.Substring(1));
            }
            return false;
        }

        #endregion

    }
}
