/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using System.Collections.Generic;
using Xunit;

namespace NuLog.Tests.Unit.Configuration
{
    [Trait("Category", "Unit")]
    public class TargetConfigTests
    {
        /// <summary>
        /// The properties of the target config should be case insensitive.
        /// </summary>
        [Fact]
        public void Should_Properties_BeCaseInsensitive()
        {
            //! Setup
            var expectedObject = new object();
            var targetConfig = new TargetConfig
            {
                Properties = new Dictionary<string, object>
                {
                    { "property", expectedObject }
                }
            };

            //! Verify
            Assert.Equal(expectedObject, targetConfig.Properties["PROPERTY"]);
        }
    }
}