/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Dispatchers.TagRouters;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NuLog.Tests.Unit.TagRouters.RuleProcessors
{
    /// <summary>
    /// Documents the expected behavior around "excludes" rules.
    /// </summary>
    [Trait("Category", "Unit")]
    public class RuleProcessorFinalRulesTests : RuleProcessorTestsBase
    {
        /// <summary>
        /// The router should stop processing rules once a rule that is marked final is matched.
        /// </summary>
        [Fact(DisplayName = "Should_StopProcessingOnFinal")]
        public void Should_StopProcessingOnFinal()
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
                    Final = true
                },
                new Rule
                {
                    Include = new string[] { "hello_tag" },
                    Targets = new string[] { "califragilistic" }
                }
            };
            var processor = GetRuleProcessor(rules);

            // Execute
            var targets = processor.DetermineTargets(new string[] { "hello_tag" });

            // Verify
            Assert.Equal(2, targets.Count());
            Assert.Contains("super_target", targets);
            Assert.Contains("duper_target", targets);
        }

                /// <summary>
        /// The router should stop processing rules once a rule that is marked final is matched.
        /// </summary>
        [Fact(DisplayName = "Should_StopProcessingOnFirstFinalHit")]
        public void Should_StopProcessingOnFirstFinalHit()
        {
            // Setup
            var rules = new List<Rule>
            {
                new Rule
                {
                    Include = new string[] { "first_tag" },
                    Targets = new string[] { "super_target" },
                    Final = true
                },
                new Rule
                {
                    Include = new string[] { "hello_tag" },
                    Targets = new string[] { "duper_target" },
                    Final = true
                },
                new Rule
                {
                    Include = new string[] { "*" },
                    Targets = new string[] { "califragilistic" }
                }
            };
            var processor = GetRuleProcessor(rules);

            // Execute
            var targets = processor.DetermineTargets(new string[] { "hello_tag" });

            // Verify
            Assert.Equal(1, targets.Count());
            Assert.Contains("duper_target", targets);
        }
    }
}