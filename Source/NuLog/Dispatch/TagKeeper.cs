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
using System.Linq;

namespace NuLog.Dispatch
{
    /// <summary>
    /// Responsible for matching tags to each other using then configured tag groups and wildcards
    /// </summary>
    public class TagKeeper : IConfigObserver
    {

        #region Members and Constructors

        // Once the tag groups are calculated, they are cached
        private static readonly object _tagLock = new object();
        private IDictionary<string, ICollection<string>> TagGroups { get; set; }

        /// <summary>
        /// Constructs a default, empty tag keeper
        /// </summary>
        public TagKeeper()
        {
            TagGroups = new Dictionary<string, ICollection<string>>();
        }

        #endregion

        #region Match Checking

        /// <summary>
        /// Checks a single tag against another.  The passed "tag" must match the tag/pattern "mustMatch"
        /// </summary>
        /// <param name="tag">The tag to test</param>
        /// <param name="mustMatch">The tag that the first tag must match</param>
        /// <returns>True indicating the tag matches, false otherwise</returns>
        public bool CheckMatch(string tag, string mustMatch)
        {
            return CheckMatch(new List<string> { tag }, new List<string> { mustMatch });
        }

        /// <summary>
        /// Checks a list of tags against another list of tags.  "tags", must match "mustMatch" tags to return true.
        /// </summary>
        /// <param name="tags">The proposed tags for matching</param>
        /// <param name="mustMatch">The tags to match</param>
        /// <param name="strict">Whether or not every tag in "mustMatch" must be matched by a tag in "tags" to return true</param>
        /// <returns>True if "tags" match "mustMatch", false otherwise</returns>
        public bool CheckMatch(ICollection<string> tags, ICollection<string> mustMatch, bool strict = false)
        {
            lock (_tagLock)
            {
                // Make sure that the tags are formatted to match (case-insensitive)
                var formattedTags = FormatTags(tags);
                var formattedMustMatch = FormatTags(mustMatch);

                // Getting the variables outside of the loop
                //  so that the memory is not allocated during
                //  each iteration
                bool allFound = true;
                bool anyFound = false;
                bool tagFound = false;
                ICollection<string> expandedMustMatch;

                // Loop over each tag that must be matched
                // Expand out potential matches using the tag groups
                // And check to see if it is matched
                foreach (var matchTag in formattedMustMatch)
                {
                    expandedMustMatch = ExpandTagGroup(TagGroups, matchTag);
                    tagFound = false;
                    foreach (var tag in formattedTags)
                    {
                        if (expandedMustMatch.Any(mustMatchTag => MatchWildcardString(mustMatchTag, tag)))
                        {
                            tagFound = true;
                            break;
                        }
                    }

                    // Update if any and all have been found
                    anyFound = anyFound || tagFound;
                    allFound = allFound && tagFound;

                    // If we are okay matching any, we're good
                    if (anyFound && !strict)
                        break;
                }

                // Return the results
                return strict
                    ? anyFound && allFound
                    : anyFound;
            }
        }

        #endregion

        #region Configuration
        
        /// <summary>
        /// Notifies this TagKeeper of a new logging config
        /// </summary>
        /// <param name="loggingConfig">The new logging config</param>
        public void NotifyNewConfig(LoggingConfig loggingConfig)
        {
            lock (_tagLock)
            {
                UpdateTagGroups(TagGroups, loggingConfig.TagGroups);
            }
        }

        #endregion

        #region Tag Groups

        // Expands the tag to a list of tags using the given tag groups.  For example, in traditional logging, "info" would be expanded to "info", "debug" and "trace"
        private static ICollection<string> ExpandTagGroup(IDictionary<string, ICollection<string>> tagGroups, string tag)
        {
            return ExpandTagGroup(tagGroups, new List<string> { tag });
        }

        // Expands the list of tags using the given tag groups.
        private static ICollection<string> ExpandTagGroup(IDictionary<string, ICollection<string>> tagGroups, ICollection<string> tags)
        {
            // Format the tags so we work on a common denominator
            var expandedList = new List<string>();
            var formattedTags = FormatTags(tags);

            // For each of the tags
            foreach (var tag in formattedTags)
            {
                // Make sure that the expanded group includes the tag itself
                if (expandedList.Contains(tag) == false)
                    expandedList.Add(tag);

                // Uniquely include each of the tags defined in the tag group
                if (tagGroups.ContainsKey(tag))
                    foreach (var childTag in tagGroups[tag])
                        if (expandedList.Contains(childTag) == false)
                            expandedList.Add(childTag);
            }

            // Tada!
            return expandedList;
        }

        // Updates the tag groups by recursing through them using the tag groups (purportedly from the logging config)
        private static void UpdateTagGroups(IDictionary<string, ICollection<string>> tagGroups, ICollection<TagGroupConfig> tagGroupConfigs)
        {
            tagGroups.Clear();
            if (tagGroupConfigs != null && tagGroupConfigs.Count > 0)
                foreach (TagGroupConfig tagGroupConfig in tagGroupConfigs)
                    UpdateTagGroupsRecurse(tagGroups, FormatTag(tagGroupConfig.Tag), tagGroupConfig.ChildTags);
        }


        // Recurses over the tag groups to populate the child tags of a given tag
        private static void UpdateTagGroupsRecurse(IDictionary<string, ICollection<string>> tagGroups, string parentTag, ICollection<string> childTags)
        {
            // Format the tags we are working with
            ICollection<string> fChildTags = FormatTags(childTags);
            ICollection<string> finalTags;

            // Check to see if our group already contains the parent tag
            //  Get a hold of the existing list if it is already initilized
            //  or create a new one
            if (!tagGroups.ContainsKey(parentTag))
            {
                finalTags = new List<string>();
                tagGroups.Add(parentTag, finalTags);
            }
            else
            {
                finalTags = tagGroups[parentTag];
            }

            // Iterate over ach of the child tags
            foreach (string childTag in fChildTags)
            {
                // And check to see if we have already processed this child tag
                if (!finalTags.Contains(childTag))
                {
                    // And if not, add it to the list
                    //  Check to see if we have yet another tag group for the child tag
                    //   and recurse into it if we do, continuing the build-out
                    finalTags.Add(childTag);
                    if (tagGroups.ContainsKey(childTag))
                        UpdateTagGroupsRecurse(tagGroups, parentTag, tagGroups[parentTag]);
                }
            }
        }

        #endregion

        #region Helpers

        // Checks to see if "input" matches "pattern" using wildcards "*" and "?"
        private bool MatchWildcardString(string pattern, string input)
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

        // Formats the list of tags and returns them
        private static ICollection<string> FormatTags(ICollection<string> tags)
        {
            // Iterate over each tag formatting it and adding it to a list
            ICollection<string> fTags = new List<string>();
            foreach (string tag in tags)
                fTags.Add(FormatTag(tag));
            return fTags;
        }

        // Formats and returns a given tag
        private static string FormatTag(string tag)
        {
            return tag.ToLower();
        }

        #endregion

    }
}
