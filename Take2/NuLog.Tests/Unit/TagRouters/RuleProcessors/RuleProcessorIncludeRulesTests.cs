/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Dispatchers.TagRouters;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NuLog.Tests.Unit.TagRouters.RuleProcessors
{
    /// <summary>
    /// Documents the expected behavior of a tag router's "include" rules.
    /// </summary>
    [Trait("Category", "Unit")]
    public class RuleProcessorIncludeRulesTests : RuleProcessorTestsBase
    {
        /// <summary>
        /// Should handle receiving a "null" list of rules.
        /// </summary>
        [Fact(DisplayName = "Should_HandleNullRules")]
        public void Should_HandleNullRules()
        {
            // Setup
            var processor = GetRuleProcessor(null);

            // Execute
            var targets = processor.DetermineTargets("hello_tag");

            // Verify
            Assert.NotNull(targets);
        }

        /// <summary>
        /// Should handle routing to a single target using a single tag, defined in a single rule.
        /// </summary>
        [Fact(DisplayName = "Should_RouteSingleTag")]
        public void Should_RouteSingleTag()
        {
            // Setup
            var rules = new List<Rule>
            {
                new Rule
                {
                    Include = new string[] { "hello_tag" },
                    Targets = new string[] { "super_target" }
                }
            };
            var processor = GetRuleProcessor(rules);

            // Execute
            var targets = processor.DetermineTargets("hello_tag");

            // Verify
            Assert.Equal("super_target", targets.Single());
        }

        /// <summary>
        /// Tag matching should be case insensitive.
        /// </summary>
        [Fact(DisplayName = "Should_MatchTagsCaseInsensitive")]
        public void Should_MatchTagsCaseInsensitive()
        {
            // Setup
            var rules = new List<Rule>
            {
                new Rule
                {
                    Include = new string[] { "hello_tag" },
                    Targets = new string[] { "super_target" }
                }
            };
            var processor = GetRuleProcessor(rules);

            // Execute
            var targets = processor.DetermineTargets("HELLO_TAG");

            // Verify
            Assert.Equal("super_target", targets.Single());
        }

        /// <summary>
        /// The result list of targets shouldn't be null if there is no match on rules.
        /// </summary>
        [Fact(DisplayName = "Should_ReturnNonNullOnNoMatch")]
        public void Should_ReturnNonNullOnNoMatch()
        {
            // Setup
            var rules = new List<Rule>
            {
                new Rule
                {
                    Include = new string[] { "hello_tag" },
                    Targets = new string[] { "super_target" }
                }
            };
            var processor = GetRuleProcessor(rules);

            // Execute
            var targets = processor.DetermineTargets("no_match");

            // Verify
            Assert.NotNull(targets);
        }

        /// <summary>
        /// Should return a complete list of targets, if a tag matches multiple rules.
        /// </summary>
        [Fact(DisplayName = "Should_ReturnMultipleRules")]
        public void Should_ReturnMultipleRules()
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
                    Targets = new string[] { "duper_target" }
                }
            };
            var processor = GetRuleProcessor(rules);

            // Execute
            var targets = processor.DetermineTargets("hello_tag");

            // Verify
            Assert.Equal(2, targets.Count());
            Assert.Contains("super_target", targets);
            Assert.Contains("duper_target", targets);
        }

        /// <summary>
        /// Should return a distinct list of targets, if a target is listed in multiple matched rules.
        /// </summary>
        [Fact(DisplayName = "Should_ReturnDistinctTargetList")]
        public void Should_ReturnDistinctTargetList()
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
                    Targets = new string[] { "duper_target", "super_target" }
                }
            };
            var processor = GetRuleProcessor(rules);

            // Execute
            var targets = processor.DetermineTargets("hello_tag");

            // Verify
            Assert.Equal(2, targets.Count());
            Assert.Contains("super_target", targets);
            Assert.Contains("duper_target", targets);
        }

        /// <summary>
        /// Should match multiple rules, which have different tags, by sending in multiple tags.
        /// </summary>
        [Fact(DisplayName = "Should_MatchMultipleRulesWithMultipleTags")]
        public void Should_MatchMultipleRulesWithMultipleTags()
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
                    Include = new string[] { "goodbye_tag" },
                    Targets = new string[] { "duper_target", }
                }
            };
            var processor = GetRuleProcessor(rules);

            // Execute
            var targets = processor.DetermineTargets("hello_tag", "goodbye_tag");

            // Verify
            Assert.Equal(2, targets.Count());
            Assert.Contains("super_target", targets);
            Assert.Contains("duper_target", targets);
        }

        /// <summary>
        /// Should support the wild-card "*" in various parts of the include tag in the rule. Tests a
        /// single instance of the wild-card.
        /// </summary>
        [Theory(DisplayName = "Should_MatchSingleWildcardRule")]
        [InlineData("*", "woah", true)]
        [InlineData("some*", "some", true)]
        [InlineData("some*", "something", true)]
        [InlineData("*thing", "thing", true)]
        [InlineData("*thing", "something", true)]
        [InlineData("some*thing", "something", true)]
        [InlineData("some*thing", "some_super_thing", true)]
        [InlineData("some.super*thing", "some.super_duper_thing", true)]
        [InlineData("some*", "nope", false)]
        [InlineData("*thing", "nope", false)]
        [InlineData("some*thing", "some", false)]
        [InlineData("some*thing", "thing", false)]
        [InlineData("some*thing", "somenope", false)]
        [InlineData("some*thing", "nopething", false)]
        public void Should_MatchWildcardRule(string tagRule, string matchTag, bool shouldMatch)
        {
            // Setup
            var rules = new List<Rule>
            {
                new Rule
                {
                    Include = new string[] { tagRule },
                    Targets = new string[] { "super_target" }
                }
            };
            var processor = GetRuleProcessor(rules);

            // Execute
            var targets = processor.DetermineTargets(matchTag);

            // Verify
            if (shouldMatch)
            {
                Assert.Contains("super_target", targets);
            }
            else
            {
                Assert.Equal(0, targets.Count());
            }
        }

        /// <summary>
        /// When the strict include flag is set for a rule, all tags defined in the rule's "Include"
        /// should be present in order for the rules' targets to be returned.
        /// </summary>
        [Theory(DisplayName = "Should_RequireAllTagsToMatchWithStrictIncludeFlag")]
        [InlineData(new string[] { "hello_tag" }, 1)]
        [InlineData(new string[] { "hello_tag", "missed_me" }, 2)]
        public void Should_RequireAllTagsToMatchWithStrictIncludeFlag(string[] tags, int expectedCount)
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
                    Include = new string[] { "hello_tag", "missed_me" },
                    Targets = new string[] { "duper_target" },
                    StrictInclude = true
                }
            };
            var processor = GetRuleProcessor(rules);

            // Execute
            var targets = processor.DetermineTargets(tags);

            // Verify
            Assert.Equal(expectedCount, targets.Count());
        }
    }
}