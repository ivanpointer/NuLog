/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;

namespace NuLog.TagRouters {

    /// <summary>
    /// Defines the expected behavior of a tag group processor.
    ///
    /// Tag group processors are responsible for interpreting tag groups, performing lookups for
    /// rules to see if an incoming tag matches the tag expected by the rule.
    /// </summary>
    public interface ITagGroupProcessor {

        /// <summary>
        /// Returns a list of tags that the given tag should also match for.
        ///
        /// For example, if you were implementing a level based logging strategy (legacy), you'd
        /// expect the "fatal" tag to also match for rules expecting "error", "warn", "info", "debug"
        /// and "trace".
        /// </summary>
        /// <param name="tag">The tag to figure aliases for.</param>
        IEnumerable<string> GetAliases(string tag);
    }
}