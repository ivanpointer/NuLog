using NuLog.Configuration;
using NuLog.Factories.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NuLog.Tests.Unit.Configuration
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the standard configuration provider.
    /// </summary>
    [Trait("Category", "Unit")]
    public class StandardConfigurationProviderTests
    {
        /// <summary>
        /// The provider should provide a configuration.
        /// </summary>
        [Fact(DisplayName = "Shoud_ProvideConfiguration")]
        public void Shoud_ProvideConfiguration()
        {
            // Setup
            var provider = GetConfigurationProvider("nulog");

            // Execute
            var config = provider.GetConfiguration();

            // Validate
            Assert.NotNull(config);
        }

        /// <summary>
        /// Returns the configuration provider under test.
        /// </summary>
        protected IConfigurationProvider GetConfigurationProvider(string configSection)
        {
            return new StandardConfigurationProvider(configSection);
        }

    }
}
