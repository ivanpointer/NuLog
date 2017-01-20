/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration.StandardConfiguration;
using System.Configuration;
using System.Linq;
using Xunit;

namespace NuLog.Tests.Unit.Configuration.StandardConfigSection
{
    /// <summary>
    /// Documents the expected behavior of the rule element (standard configuration section).
    /// </summary>
    [Trait("Category", "Unit")]
    public class RuleElementTests
    {
        /// <summary>
        /// The rule element should have "includes" tags.
        /// </summary>
        [Fact(DisplayName = "Should_ContainIncludes")]
        public void Should_ContainIncludes()
        {
            // Setup
            var config = (StandardConfigurationSection)ConfigurationManager.GetSection("Should_ContainIncludes");

            // Execute / Validate
            var rule = config.Rules.Single();
            Assert.Equal(4, rule.Include.Length);
            Assert.Contains("one_tag", rule.Include);
            Assert.Contains("two_tag", rule.Include);
            Assert.Contains("red_tag", rule.Include);
            Assert.Contains("blue_tag", rule.Include);
        }

        /// <summary>
        /// The rule element should have "excludes" tags.
        /// </summary>
        [Fact(DisplayName = "Should_ContainExcludes")]
        public void Should_ContainExcludes()
        {
            // Setup
            var config = (StandardConfigurationSection)ConfigurationManager.GetSection("Should_ContainExcludes");

            // Execute / Validate
            var rule = config.Rules.Single();
            Assert.Equal(4, rule.Excludes.Length);
            Assert.Contains("one_tag", rule.Excludes);
            Assert.Contains("two_tag", rule.Excludes);
            Assert.Contains("red_tag", rule.Excludes);
            Assert.Contains("blue_tag", rule.Excludes);
        }

        /// <summary>
        /// The rule element should have "targets" target names.
        /// </summary>
        [Fact(DisplayName = "Should_RuleContainTargets")]
        public void Should_RuleContainTargets()
        {
            // Setup
            var config = (StandardConfigurationSection)ConfigurationManager.GetSection("Should_RuleContainTargets");

            // Execute / Validate
            var rule = config.Rules.Single();
            Assert.Equal(4, rule.Targets.Length);
            Assert.Contains("one_target", rule.Targets);
            Assert.Contains("two_target", rule.Targets);
            Assert.Contains("red_target", rule.Targets);
            Assert.Contains("blue_target", rule.Targets);
        }

        /// <summary>
        /// The rule element should have a strict include flag.
        /// </summary>
        [Fact(DisplayName = "Should_ContainStrictInclude")]
        public void Should_ContainStrictInclude()
        {
            // Setup
            var config = (StandardConfigurationSection)ConfigurationManager.GetSection("Should_ContainStrictInclude");

            // Execute / Validate
            var rule = config.Rules.Single();
            Assert.True(rule.StrictInclude);
        }

        /// <summary>
        /// The rule element should have a final flag.
        /// </summary>
        [Fact(DisplayName = "Should_ContainFinal")]
        public void Should_ContainFinal()
        {
            // Setup
            var config = (StandardConfigurationSection)ConfigurationManager.GetSection("Should_ContainFinal");

            // Execute / Validate
            var rule = config.Rules.Single();
            Assert.True(rule.Final);
        }
    }
}