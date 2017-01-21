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
    public class XmlConfigTagGroupParsingTests : XmlConfigurationProviderTestsBase
    {
        /// <summary>
        /// Tag groups should contain a base tag.
        /// </summary>
        [Fact(DisplayName = "Should_ContainBaseTag")]
        public void Should_ContainBaseTag()
        {
            // Setup
            var provider = GetConfigurationProvider("<nulog><tagGroups><group baseTag=\"one_tag\" /></tagGroups></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            var tagGroup = config.TagGroups.Single();
            Assert.Equal("one_tag", tagGroup.BaseTag);
        }

        /// <summary>
        /// The tag group should contain one alias tag.
        /// </summary>
        [Fact(DisplayName = "Should_ContainOneAliasTag")]
        public void Should_ContainOneAliasTag()
        {
            // Setup
            var provider = GetConfigurationProvider("<nulog><tagGroups><group aliases=\"one_tag\" /></tagGroups></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            var tagGroup = config.TagGroups.Single();
            var alias = tagGroup.Aliases.Single();
            Assert.Equal("one_tag", alias);
        }

        /// <summary>
        /// The tag group should contain multiple alias tags.
        /// </summary>
        [Fact(DisplayName = "Should_ContainMultipleAliasTags")]
        public void Should_ContainMultipleAliasTags()
        {
            // Setup
            var provider = GetConfigurationProvider("<nulog><tagGroups><group aliases=\"one_tag,two_tag\" /></tagGroups></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            var tagGroup = config.TagGroups.Single();
            Assert.Equal(2, tagGroup.Aliases.Count);
            Assert.Contains("one_tag", tagGroup.Aliases);
            Assert.Contains("two_tag", tagGroup.Aliases);
        }
    }
}