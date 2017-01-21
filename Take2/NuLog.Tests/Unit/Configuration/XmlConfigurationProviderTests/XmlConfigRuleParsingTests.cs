/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Linq;
using Xunit;

namespace NuLog.Tests.Unit.Configuration.XmlConfigurationProviderTests
{
    /// <summary>
    /// Tests around parsing out rules from XML.
    /// </summary>
    [Trait("Category", "Unit")]
    public class XmlConfigRuleParsingTests : XmlConfigurationProviderTestsBase
    {
        /// <summary>
        /// Rules should contain a rule with one include.
        /// </summary>
        [Fact(DisplayName = "Should_ContainOneInclude")]
        public void Should_ContainOneInclude()
        {
            // Setup
            var provider = GetConfigurationProvider("<nulog><rules><rule include=\"one_tag\" /></rules></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            var rule = config.Rules.Single();
            var include = rule.Includes.Single();
            Assert.Equal("one_tag", include);
        }

        /// <summary>
        /// Rules should contain a rule with multiple includes.
        /// </summary>
        [Fact(DisplayName = "Should_ContainMultipleIncludes")]
        public void Should_ContainMultipleIncludes()
        {
            // Setup
            var provider = GetConfigurationProvider("<nulog><rules><rule include=\"one_tag,two_tag\" /></rules></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            var rule = config.Rules.Single();
            Assert.Equal(2, rule.Includes.Count());
            Assert.Contains("one_tag", rule.Includes);
            Assert.Contains("two_tag", rule.Includes);
        }

        /// <summary>
        /// Rules should contain a rule with one exclude.
        /// </summary>
        [Fact(DisplayName = "Should_ContainOneExclude")]
        public void Should_ContainOneExclude()
        {
            // Setup
            var provider = GetConfigurationProvider("<nulog><rules><rule exclude=\"one_tag\" /></rules></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            var rule = config.Rules.Single();
            var exclude = rule.Excludes.Single();
            Assert.Equal("one_tag", exclude);
        }

        /// <summary>
        /// Rules should contain a rule with multiple excludes.
        /// </summary>
        [Fact(DisplayName = "Should_ContainMultipleExcludes")]
        public void Should_ContainMultipleExcludes()
        {
            // Setup
            var provider = GetConfigurationProvider("<nulog><rules><rule exclude=\"one_tag,two_tag\" /></rules></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            var rule = config.Rules.Single();
            Assert.Equal(2, rule.Excludes.Count());
            Assert.Contains("one_tag", rule.Excludes);
            Assert.Contains("two_tag", rule.Excludes);
        }

        /// <summary>
        /// Rules should contain a rule with one target.
        /// </summary>
        [Fact(DisplayName = "Should_ContainOneTarget")]
        public void Should_ContainOneTarget()
        {
            // Setup
            var provider = GetConfigurationProvider("<nulog><rules><rule targets=\"one_target\" /></rules></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            var rule = config.Rules.Single();
            var target = rule.Targets.Single();
            Assert.Equal("one_target", target);
        }

        /// <summary>
        /// Rules should contain a rule with multiple targets.
        /// </summary>
        [Fact(DisplayName = "Should_ContainMultipleTargets")]
        public void Should_ContainMultipleTargets()
        {
            // Setup
            var provider = GetConfigurationProvider("<nulog><rules><rule targets=\"one_target,two_targets\" /></rules></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            var rule = config.Rules.Single();
            Assert.Equal(2, rule.Targets.Count());
            Assert.Contains("one_target", rule.Targets);
            Assert.Contains("two_targets", rule.Targets);
        }

        /// <summary>
        /// Rules should contain a strict include flag that defaults to false.
        /// </summary>
        [Fact(DisplayName = "Should_ContainStrictIncludeFlagDefault")]
        public void Should_ContainStrictIncludeFlagDefault()
        {
            // Setup
            var provider = GetConfigurationProvider("<nulog><rules><rule /></rules></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            var rule = config.Rules.Single();
            Assert.False(rule.StrictInclude);
        }

        /// <summary>
        /// Rules should contain a strict include flag.
        /// </summary>
        [Fact(DisplayName = "Should_ContainStrictIncludeFlag")]
        public void Should_ContainStrictIncludeFlag()
        {
            // Setup
            var provider = GetConfigurationProvider("<nulog><rules><rule strictInclude=\"true\" /></rules></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            var rule = config.Rules.Single();
            Assert.True(rule.StrictInclude);
        }

        /// <summary>
        /// Rules should contain a final flag that defaults to false.
        /// </summary>
        [Fact(DisplayName = "Should_ContainFinalFlagDefault")]
        public void Should_ContainFinalFlagDefault()
        {
            // Setup
            var provider = GetConfigurationProvider("<nulog><rules><rule /></rules></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            var rule = config.Rules.Single();
            Assert.False(rule.Final);
        }

        /// <summary>
        /// Rules should contain a final flag.
        /// </summary>
        [Fact(DisplayName = "Should_ContainFinalFlag")]
        public void Should_ContainFinalFlag()
        {
            // Setup
            var provider = GetConfigurationProvider("<nulog><rules><rule final=\"true\" /></rules></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            var rule = config.Rules.Single();
            Assert.True(rule.Final);
        }
    }
}