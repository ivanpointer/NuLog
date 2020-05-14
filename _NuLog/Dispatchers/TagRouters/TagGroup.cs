/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;

namespace NuLog.Dispatchers.TagRouters {

    /// <summary>
    /// Defines a single tag group: a base tag, and a number of alias tags, that are to be treated
    /// the same as the base tag.
    /// </summary>
    public class TagGroup {

        /// <summary>
        /// The base tag
        /// </summary>
        public string BaseTag { get; set; }

        /// <summary>
        /// The aliases for the base tag
        /// </summary>
        public ICollection<string> Aliases { get; set; }
    }
}