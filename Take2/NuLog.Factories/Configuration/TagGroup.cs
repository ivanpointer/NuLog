/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;

namespace NuLog.Factories.Configuration
{
    /// <summary>
    /// Represents a single tag group in the NuLog configuration.
    /// </summary>
    public class TagGroup
    {
        /// <summary>
        /// The base tag of the tag group.
        /// </summary>
        public string BaseTag { get; set; }

        /// <summary>
        /// The aliases associated with the base tag.
        /// </summary>
        public ICollection<string> Aliases { get; set; }
    }
}