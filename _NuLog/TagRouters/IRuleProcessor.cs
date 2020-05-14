/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;

namespace NuLog.TagRouters {

    /// <summary>
    /// Defines the expected behavior for a rule processor; which is to interpret rules, and for a
    /// given set of tags, return a list of targets. This is a sub-function of the tag router.
    /// </summary>
    public interface IRuleProcessor {

        /// <summary>
        /// Returns a list of targets who match the given set of tags, based on the rules.
        /// </summary>
        /// <param name="tags">The tags to determine the targets for.</param>
        IEnumerable<string> DetermineTargets(IEnumerable<string> tags);
    }
}