/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Dispatchers.TagRouters;
using NuLog.TagRouters;
using System.Collections.Generic;

namespace NuLog.Tests.Unit.TagRouters
{
    /// <summary>
    /// Defines the common functionality for tests for the tag router.
    ///
    /// Most notably, provides a method for getting the "system under test".
    /// </summary>
    public abstract class TagRouterTestsBase
    {
        protected ITagRouter GetTagRouter(ICollection<Rule> rules)
        {
            return new StandardTagRouter(rules);
        }
    }
}