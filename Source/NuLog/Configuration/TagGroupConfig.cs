/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/7/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using System.Collections.Generic;

namespace NuLog.Configuration
{
    /// <summary>
    /// Represents a single tag group in the configuration
    /// </summary>
    public class TagGroupConfig
    {
        /// <summary>
        /// The tag that the child tags are to be grouped under
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// The child tags that are grouped under the tag of this tag group
        /// </summary>
        public ICollection<string> ChildTags { get; set; }

        /// <summary>
        /// Constructs an empty tag group
        /// </summary>
        public TagGroupConfig()
        {
            ChildTags = new List<string>();
        }

        /// <summary>
        /// Constructs the tag group
        /// </summary>
        /// <param name="tag">The tag of the tag group</param>
        /// <param name="childTags">The child tags of the tag group</param>
        public TagGroupConfig(string tag, params string[] childTags)
        {
            Tag = tag;
            ChildTags = childTags;
        }
    }
}