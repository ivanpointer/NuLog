/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration.StandardConfiguration;
using NuLog.Targets;
using System.Configuration;
using System.Linq;
using Xunit;

namespace NuLog.Tests.Unit.Configuration.StandardConfigSection
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the target element of the standard
    /// configuration section.
    /// </summary>
    [Trait("Category", "Unit")]
    public class TargetElementTests
    {
        /// <summary>
        /// Target elements should have a name.
        /// </summary>
        [Fact(DisplayName = "Should_TargetHaveName")]
        public void Should_TargetHaveName()
        {
            // Setup
            var config = (StandardConfigurationSection)ConfigurationManager.GetSection("Should_TargetHaveName");

            // Execute / Validate
            var target = config.Targets.Single();
            Assert.Equal("hello_target", target.Name);
        }

        /// <summary>
        /// Target elements should have a type.
        /// </summary>
        [Fact(DisplayName = "Should_TargetHaveType")]
        public void Should_TargetHaveType()
        {
            // Setup
            var config = (StandardConfigurationSection)ConfigurationManager.GetSection("Should_TargetHaveType");

            // Execute / Validate
            var target = config.Targets.Single();
            Assert.Equal(typeof(StreamTarget), target.Type);
        }

        /// <summary>
        /// Target elements should provide a list of properties.
        /// </summary>
        [Fact(DisplayName = "Should_HaveTargetProperties")]
        public void Should_HaveTargetProperties()
        {
            // Setup
            var config = (StandardConfigurationSection)ConfigurationManager.GetSection("Should_HaveTargetProperties");

            // Execute / Validate
            var target = config.Targets.Single();
            Assert.Equal(1, target.TargetProperties.Count);
        }
    }
}