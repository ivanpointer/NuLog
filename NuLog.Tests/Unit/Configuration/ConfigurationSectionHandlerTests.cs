/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Configuration;
using System.Xml;
using Xunit;

namespace NuLog.Tests.Unit.Configuration {

    /// <summary>
    /// Documents (and verifies) the expected behavior of the configuration section handler.
    /// </summary>
    [Trait("Category", "Unit")]
    public class ConfigurationSectionHandlerTests {

        /// <summary>
        /// The configuration section handler should just pass the section back as XML.
        /// </summary>
        [Fact(DisplayName = "Should_ReturnConfigSectionXml")]
        public void Should_ReturnConfigSectionXml() {
            // Setup / Execute
            var section = ConfigurationManager.GetSection("nulog") as XmlElement;

            // Verify
            var serial = section.OuterXml;
            Assert.StartsWith("<nulog><targets>", serial);
        }
    }
}