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
    public class RulesCollectionTests
    {
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
    }
}