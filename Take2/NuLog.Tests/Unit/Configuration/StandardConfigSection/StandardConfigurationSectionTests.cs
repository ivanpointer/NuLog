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
    /// Documents (and verifies) the expected behavior of the standard configuration section.
    /// </summary>
    [Trait("Category", "Unit")]
    public class StandardConfigurationSectionTests
    {
        /// <summary>
        /// The standard configuration section should contain targets.
        /// </summary>
        [Fact(DisplayName = "Should_ContainTargets")]
        public void Should_ContainTargets()
        {
            // Setup
            var config = (StandardConfigurationSection)ConfigurationManager.GetSection("Should_ContainTargets");

            // Execute / Validate
            Assert.Equal(1, config.Targets.Count);
            Assert.NotNull(config.Targets.Single());
        }

        /// <summary>
        /// The standard configuration section should contain rules.
        /// </summary>
        [Fact(DisplayName = "Should_ContainRules")]
        public void Should_ContainRules()
        {
            // Setup
            var config = (StandardConfigurationSection)ConfigurationManager.GetSection("Should_ContainRules");

            // Execute / Validate
            Assert.Equal(1, config.Rules.Count);
            Assert.NotNull(config.Rules.Single());
        }

        /// <summary>
        /// The standard configuration should contain tag groups.
        /// </summary>
        [Fact(DisplayName = "Should_ContainTagGroups")]
        public void Should_ContainTagGroups()
        {
            // Setup
            var config = (StandardConfigurationSection)ConfigurationManager.GetSection("Should_ContainTagGroups");

            // Execute / Validate
            Assert.Equal(1, config.TagGroups.Count);
            Assert.NotNull(config.TagGroups.Single());
        }
    }
}