/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using System.Linq;
using Xunit;

namespace NuLog.Tests.Integration.Configuration {

    /// <summary>
    /// Documents (and verifies) the expected behavior of the configuration manager provider
    /// </summary>
    [Trait("Category", "Integration")]
    public class ConfigurationManagerProviderTests {

        /// <summary>
        /// The configuration manager provider should load the config from the App.config.
        /// </summary>
        [Fact(DisplayName = "Should_LoadAppConfig")]
        public void Should_LoadAppConfig() {
            // Setup
            IConfigurationProvider provider = new ConfigurationManagerProvider();

            // Execute
            var config = provider.GetConfiguration();

            // Verify
            var target = config.Targets.Single();
            Assert.Equal("stream", target.Name);
            Assert.Equal("NuLog.Targets.DebugTarget", target.Type);
            Assert.Equal("${Message}", target.Properties["layout"]);

            var rule = config.Rules.Single();
            Assert.Contains("one_tag", rule.Includes);
            Assert.Contains("two_tag", rule.Includes);
            Assert.Contains("red_tag", rule.Includes);
            Assert.Contains("blue_tag", rule.Includes);
            Assert.Contains("green_tag", rule.Excludes);
            Assert.Contains("stream", rule.Targets);

            var tagGroup = config.TagGroups.Single();
            Assert.Equal("base_tag", tagGroup.BaseTag);
            Assert.Contains("one_tag", tagGroup.Aliases);
            Assert.Contains("two_tag", tagGroup.Aliases);
            Assert.Contains("red_tag", tagGroup.Aliases);
            Assert.Contains("blue_tag", tagGroup.Aliases);

            Assert.Equal("data", config.MetaData["meta"]);
        }
    }
}