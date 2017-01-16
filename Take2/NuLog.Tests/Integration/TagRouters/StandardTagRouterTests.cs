/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Dispatchers.TagRouters;
using NuLog.TagRouters;
using NuLog.TagRouters.RuleProcessors;
using NuLog.TagRouters.TagGroupProcessors;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NuLog.Tests.Integration.TagRouters
{
    /// <summary>
    /// Documents the expected behavior of the integrated tag router. These aren't unit tests, but
    /// instead, make sure that the particular makeup of the tag router work as a sum of parts.
    ///
    /// For example, this documents/verifies that the pieces are working in cohesion, like the rule
    /// processor and tag group processor are both being consulted when routing an event.
    /// </summary>
    [Trait("Category", "Integration")]
    public class StandardTagRouterTests
    {
        /// <summary>
        /// Makes sure that the standard tag router is using both the tag group processor and the
        /// rule processor in conjunction, in a simple use-case.
        /// </summary>
        [Fact(DisplayName = "Should_RouteUsingTagGroups")]
        public void Should_RouteUsingTagGroups()
        {
            // Setup
            var tagGroupProcessor = new StandardTagGroupProcessor(new List<TagGroup>
            {
                new TagGroup
                {
                    BaseTag = "fruit",
                    Aliases = new string[] { "orange" }
                }
            });
            var ruleProcessor = new StandardRuleProcessor(new List<Rule>
            {
                new Rule
                {
                    Include = new string[] { "fruit" },
                    Targets = new string[] { "mytarget" }
                }
            }, tagGroupProcessor);
            var tagRouter = new StandardTagRouter(ruleProcessor);

            // Execute
            var targets = tagRouter.Route(new string[] { "orange" });

            // Verify
            Assert.Equal(1, targets.Count());
            Assert.Contains("mytarget", targets);
        }
    }
}