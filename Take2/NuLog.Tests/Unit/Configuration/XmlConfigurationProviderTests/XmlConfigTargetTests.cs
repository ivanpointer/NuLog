/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Linq;
using Xunit;

namespace NuLog.Tests.Unit.Configuration.XmlConfigurationProviderTests {

    /// <summary>
    /// Documents (and verifies) the expected behavior of the standard configuration provider,
    /// specifically the target config parsing.
    /// </summary>
    [Trait("Category", "Unit")]
    public class XmlConfigTargetTests : XmlConfigurationProviderTestsBase {

        /// <summary>
        /// The target should contain a name.
        /// </summary>
        [Fact(DisplayName = "Should_ContainName")]
        public void Should_ContainName() {
            // Setup
            var provider = GetConfigurationProvider("<nulog><targets><target name=\"one_target\" /></targets></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            var target = config.Targets.Single();
            Assert.Equal("one_target", target.Name);
        }

        /// <summary>
        /// The target should contain a type.
        /// </summary>
        [Fact(DisplayName = "Should_ContainType")]
        public void Should_ContainType() {
            // Setup
            var provider = GetConfigurationProvider("<nulog><targets><target type=\"NuLog.Targets.FakeTarget\" /></targets></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            var target = config.Targets.Single();
            Assert.Equal("NuLog.Targets.FakeTarget", target.Type);
        }

        /// <summary>
        /// The target should contain a property.
        /// </summary>
        [Fact(DisplayName = "Should_ContainProperty")]
        public void Should_ContainProperty() {
            // Setup
            var provider = GetConfigurationProvider("<nulog><targets><target someProperty=\"hello, world!\" /></targets></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            var target = config.Targets.Single();
            var property = target.Properties["someProperty"];
            Assert.Equal("hello, world!", property);
        }

        /// <summary>
        /// The target should contain multiple properties.
        /// </summary>
        [Fact(DisplayName = "Should_ContainMultipleProperties")]
        public void Should_ContainMultipleProperties() {
            // Setup
            var provider = GetConfigurationProvider("<nulog><targets><target someProperty=\"hello, world!\" another=\"one fish, two fish...\" /></targets></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            var target = config.Targets.Single();
            Assert.Equal("hello, world!", target.Properties["someProperty"]);
            Assert.Equal("one fish, two fish...", target.Properties["another"]);
        }

        /// <summary>
        /// The properties should include the name and type of the target.
        /// </summary>
        [Fact(DisplayName = "Should_IncludeNameAndTypeInProperties")]
        public void Should_IncludeNameAndTypeInProperties() {
            // Setup
            var provider = GetConfigurationProvider("<nulog><targets><target name=\"props\" type=\"NuLog.Targets.FakePropsTarget\" someProperty=\"hello, world!\" another=\"one fish, two fish...\" /></targets></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            var target = config.Targets.Single();
            Assert.Equal("props", target.Properties["name"]);
            Assert.Equal("NuLog.Targets.FakePropsTarget", target.Properties["type"]);
            Assert.Equal("hello, world!", target.Properties["someProperty"]);
        }
    }
}