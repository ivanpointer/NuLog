/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NuLog
{
    /// <summary>
    /// The standard implementation of a tag normalizer.
    /// </summary>
    public class StandardTagNormalizer : ITagNormalizer
    {
        /// <summary>
        /// A regular expression for validating the tags coming in.
        /// </summary>
        private static readonly Regex tagValPattern = new Regex(@"[a-zA-Z0-9_\.]+", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public string NormalizeTag(string tag)
        {
            // Null or empty tags are not allowed
            if (string.IsNullOrEmpty(tag))
                throw new InvalidOperationException("Tags cannot be null or white space (empty).  One such tag was given.");

            // Trim white space off the tag
            var normalized = tag.Trim();

            // Make the tag lower case
            normalized = normalized.ToLower();

            // Validate the normalized tag
            ValidateTag(normalized);

            // Return our normalized tag
            return normalized;
        }

        public ICollection<string> NormalizeTags(IEnumerable<string> tags)
        {
            var hashSet = new HashSet<string>();

            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    var normalizedTag = NormalizeTag(tag);
                    hashSet.Add(normalizedTag);
                }
            }

            return hashSet;
        }

        /// <summary>
        /// Checks the tag to make sure it looks legit. Will throw an InvalidOperationException if
        /// the tag isn't compliant.
        /// </summary>
        /// <param name="tag">The tag to check.</param>
        private void ValidateTag(string tag)
        {
            if (!tagValPattern.IsMatch(tag))
            {
                throw new InvalidOperationException(string.Format("tag \"{0}\" contains invalid characters.", tag));
            }
        }

        /// <summary>
        /// Checks the tags to make sure they look legit. Will throw an InvalidOperationException if
        /// a tag is found that isn't compliant.
        /// </summary>
        /// <param name="tags">The tags to check.</param>
        private void ValidateTags(string[] tags)
        {
            foreach (var tag in tags)
            {
                ValidateTag(tag);
            }
        }
    }
}