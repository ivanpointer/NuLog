/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Dispatchers.TagRouters;
using NuLog.TagRouters;
using NuLog.TagRouters.RuleProcessors;
using System.Collections.Generic;

namespace NuLog.Tests.Unit.TagRouters.RuleProcessors
{
    /// <summary>
    /// Defines common functionality for the rule processor tests, namely, identifying the "system
    /// under test".
    /// </summary>
    public abstract class RuleProcessorTestsBase
    {
        /// <summary>
        /// Get the rule processor under test, with the given rules.
        /// </summary>
        protected IRuleProcessor GetRuleProcessor(IEnumerable<Rule> rules)
        {
            return new StandardRuleProcessor(rules, new DummyTagGroupProcessor());
        }
    }

    internal class DummyTagGroupProcessor : ITagGroupProcessor
    {
        public IEnumerable<string> GetAliases(string tag)
        {
            yield return tag;
        }
    }
}