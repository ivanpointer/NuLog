/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Dispatchers.TagRouters;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NuLog.Tests.Unit.TagRouters
{
    /// <summary>
    /// Documents the expected behavior around "excludes" rules.
    /// </summary>
    [Trait("Category", "Unit")]
    public class TagRouterExcludeRulesTests : TagRouterTestsBase
    {
        /// <summary>
        /// Checks a single simple exclude tag.
        /// </summary>
        [Fact(DisplayName = "Should_ExcludeSingleSimpleTag")]
        public void Should_ExcludeSingleSimpleTag()
        {
            // Setup
            var rules = new List<Rule>
            {
                new Rule
                {
                    Include = new string[] { "hello_tag" },
                    Targets = new string[] { "super_target" }
                },
                new Rule
                {
                    Include = new string[] { "hello_tag" },
                    Targets = new string[] { "duper_target" },
                    Exclude = new string[] { "exclude_tag" }
                }
            };
            var router = GetTagRouter(rules);

            // Execute
            var targets = router.Route("hello_tag", "exclude_tag");

            // Verify
            Assert.Equal(1, targets.Count());
            Assert.Contains("super_target", targets);
        }

        /// <summary>
        /// Checks a single simple exclude tag that has a wild-card.
        /// </summary>
        [Theory(DisplayName = "Should_ExcludeSingleWildcardTag")]
        [InlineData("exclude*", "exclude", 1)]
        [InlineData("exclude*", "exclude_dangle", 1)]
        [InlineData("*exclude", "exclude", 1)]
        [InlineData("*exclude", "dongle_exclude", 1)]
        [InlineData("*exclude*", "exclude", 1)]
        [InlineData("*exclude*", "dongle_exclude_dangle", 1)]
        [InlineData("*exclude*", "world_tag", 2)]
        public void Should_ExcludeSingleWildcardTag(string excludeTag, string tag, int expectedCount)
        {
            // Setup
            var rules = new List<Rule>
            {
                new Rule
                {
                    Include = new string[] { "hello_tag" },
                    Targets = new string[] { "super_target" }
                },
                new Rule
                {
                    Include = new string[] { "hello_tag" },
                    Targets = new string[] { "duper_target" },
                    Exclude = new string[] { excludeTag }
                }
            };
            var router = GetTagRouter(rules);

            // Execute
            var targets = router.Route("hello_tag", tag);

            // Verify
            Assert.Equal(expectedCount, targets.Count());
            Assert.Contains("super_target", targets);
        }
    }
}