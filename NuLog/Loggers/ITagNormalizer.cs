/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;

namespace NuLog {

    /// <summary>
    /// Defines the expected behavior of a tag normalizer.
    ///
    /// Tag normalizers are responsible for standardizing/normalizing, and validating tags.
    /// </summary>
    public interface ITagNormalizer {

        /// <summary>
        /// Normalizes and validates the tag. Can be as simple as trimming white space and making
        /// lower case, but could be as extensive as even replacing invalid characters with an
        /// accepted character.
        /// </summary>
        string NormalizeTag(string tag);

        /// <summary>
        /// Normalizes and validates a list of tags. Expected to use the same logic as for a single tag.
        /// </summary>
        ICollection<string> NormalizeTags(IEnumerable<string> tags);
    }
}