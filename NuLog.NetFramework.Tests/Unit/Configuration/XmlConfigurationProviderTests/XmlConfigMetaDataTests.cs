/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using Xunit;

namespace NuLog.Tests.Unit.Configuration.XmlConfigurationProviderTests {

    /// <summary>
    /// Documents (and verifies) the expected behavior of the standard XML configuration provider, in
    /// parsing default meta data.
    /// </summary>
    [Trait("Category", "Unit")]
    public class XmlConfigMetaDataTests : XmlConfigurationProviderTestsBase {

        /// <summary>
        /// Meta data should include one item.
        /// </summary>
        [Fact(DisplayName = "Should_IncludeOneItem")]
        public void Should_IncludeOneItem() {
            // Setup
            var provider = GetConfigurationProvider("<nulog><metaData><add key=\"one_meta\" value=\"hello, world!\" /></metaData></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            Assert.Equal("hello, world!", config.MetaData["one_meta"]);
        }

        /// <summary>
        /// Meta data should include more than one item.
        /// </summary>
        [Fact(DisplayName = "Should_IncludeMultipleItems")]
        public void Should_IncludeMultipleItems() {
            // Setup
            var provider = GetConfigurationProvider("<nulog><metaData><add key=\"one_meta\" value=\"hello, world!\" /><add key=\"two_thing\" value=\"two value\" /></metaData></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            Assert.Equal("hello, world!", config.MetaData["one_meta"]);
            Assert.Equal("two value", config.MetaData["two_thing"]);
        }

        /// <summary>
        /// When duplicate keys appear in the meta data, the last value should be taken.
        /// </summary>
        [Fact(DisplayName = "Should_TakeLastMetaDataItem")]
        public void Should_TakeLastMetaDataItem() {
            // Setup
            var provider = GetConfigurationProvider("<nulog><metaData><add key=\"one_meta\" value=\"hello, world!\" /><add key=\"one_meta\" value=\"two value\" /></metaData></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            Assert.Equal("two value", config.MetaData["one_meta"]);
        }
    }
}