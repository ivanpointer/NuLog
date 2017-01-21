﻿/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using Xunit;

namespace NuLog.Tests.Unit.Configuration.XmlConfigurationProviderTests
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the standard XML configuration provider.
    /// </summary>
    [Trait("Category", "Unit")]
    public class XmlConfigProviderTests : XmlConfigurationProviderTestsBase
    {
        /// <summary>
        /// The provider should provide a configuration.
        /// </summary>
        [Fact(DisplayName = "Shoud_ProvideConfiguration")]
        public void Shoud_ProvideConfiguration()
        {
            // Setup
            var provider = GetConfigurationProvider("<nulog></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            Assert.NotNull(config);
        }

        /// <summary>
        /// The configuration should provide one rule.
        /// </summary>
        [Fact(DisplayName = "Should_ContainOneRule")]
        public void Should_ContainOneRule()
        {
            // Setup
            var provider = GetConfigurationProvider("<nulog><rules><rule /></rules></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            Assert.Equal(1, config.Rules.Count);
        }

        /// <summary>
        /// The configuration should provide multiple rules.
        /// </summary>
        [Fact(DisplayName = "Should_ContainMultipleRules")]
        public void Should_ContainMultipleRules()
        {
            // Setup
            var provider = GetConfigurationProvider("<nulog><rules><rule /><rule /></rules></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            Assert.Equal(2, config.Rules.Count);
        }

        /// <summary>
        /// The configuration should provide one tag group.
        /// </summary>
        [Fact(DisplayName = "Should_ContainOneTagGroup")]
        public void Should_ContainOneTagGroup()
        {
            // Setup
            var provider = GetConfigurationProvider("<nulog><tagGroups><group /></tagGroups></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            Assert.Equal(1, config.TagGroups.Count);
        }

        /// <summary>
        /// The configuration should provide multiple tag groups.
        /// </summary>
        [Fact(DisplayName = "Should_ContainMultipleTagGroups")]
        public void Should_ContainMultipleTagGroups()
        {
            // Setup
            var provider = GetConfigurationProvider("<nulog><tagGroups><group /><group /></tagGroups></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            Assert.Equal(2, config.TagGroups.Count);
        }

        /// <summary>
        /// The configuration should provide one target.
        /// </summary>
        [Fact(DisplayName = "Should_ContainOneTarget")]
        public void Should_ContainOneTarget()
        {
            // Setup
            var provider = GetConfigurationProvider("<nulog><targets><target /></targets></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            Assert.Equal(1, config.Targets.Count);
        }

        /// <summary>
        /// The configuration should provide multiple targets.
        /// </summary>
        [Fact(DisplayName = "Should_ContainMultipleTargets")]
        public void Should_ContainMultipleTargets()
        {
            // Setup
            var provider = GetConfigurationProvider("<nulog><targets><target /><target /></targets></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            Assert.Equal(2, config.Targets.Count);
        }

        /// <summary>
        /// The configuration should include one default meta data entry.
        /// </summary>
        [Fact(DisplayName = "Should_ContainOneMetaData")]
        public void Should_ContainOneMetaData()
        {
            // Setup
            var provider = GetConfigurationProvider("<nulog><metaData><add key=\"thing_one\" /></metaData></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            Assert.Equal(1, config.MetaData.Count);
        }

        /// <summary>
        /// The configuration should include multiple default meta data entries.
        /// </summary>
        [Fact(DisplayName = "Should_ContainMultipleMetaDataEntries")]
        public void Should_ContainMultipleMetaDataEntries()
        {
            // Setup
            var provider = GetConfigurationProvider("<nulog><metaData><add key=\"thing_one\" /><add key=\"thing_two\" /></metaData></nulog>");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            Assert.Equal(2, config.MetaData.Count);
        }
    }
}