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
    /// Documents (and verifies) the expected behavior of the tag group element in the standard
    /// configuration section.
    /// </summary>
    [Trait("Category", "Unit")]
    public class TagGroupElementTests
    {
        /// <summary>
        /// The tag group element should contain the base tag.
        /// </summary>
        [Fact(DisplayName = "Should_ContainBaseTag")]
        public void Should_ContainBaseTag()
        {
            // Setup
            var config = (StandardConfigurationSection)ConfigurationManager.GetSection("Should_ContainBaseTag");

            // Execute / Validate
            var tagGroup = config.TagGroups.Single();
            Assert.Equal("hello_world", tagGroup.BaseTag);
        }

        /// <summary>
        /// The tag group element should contain aliases.
        /// </summary>
        [Fact(DisplayName = "Should_ContainAliases")]
        public void Should_ContainAliases()
        {
            // Setup
            var config = (StandardConfigurationSection)ConfigurationManager.GetSection("Should_ContainAliases");

            // Execute / Validate
            var tagGroup = config.TagGroups.Single();
            Assert.Equal(4, tagGroup.Aliases.Length);
            Assert.Contains("one_tag", tagGroup.Aliases);
            Assert.Contains("two_tag", tagGroup.Aliases);
            Assert.Contains("red_tag", tagGroup.Aliases);
            Assert.Contains("blue_tag", tagGroup.Aliases);
        }

    }
}