/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;

namespace NuLog.Dispatchers.TagRouters
{
    /// <summary>
    /// Defines the expected behavior of a tag router.
    ///
    /// Tag routers are responsible for interpreting rules to determine which targets a particular
    /// set of tags qualify for.
    /// </summary>
    public interface ITagRouter
    {
        /// <summary>
        /// Returns a list of targets who match the given set of tags, based on the rules.
        /// </summary>
        /// <param name="tags">The tags to determine the targets for.</param>
        IEnumerable<string> Route(params string[] tags);
    }
}