/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using Microsoft.Extensions.Configuration;
using NuLog.NetCore.Configuration;
using System.Linq;
using Xunit;
using INuLogConfigProvider = NuLog.Configuration.IConfigurationProvider;

namespace NuLog.NetCore.Tests.Configuration
{
    [Trait("Category", "Unit")]
    public class SimpleConfigurationProvider_Tests
    {
        [Fact]
        public void Should_GetRules()
        {
            //! Setup
            //- No setup

            //! Execute
            var config = _underTest.GetConfiguration();

            //! Verify
            Assert.NotNull(config);
            Assert.NotNull(config.Rules);
            var rule = config.Rules.SingleOrDefault();
            Assert.NotNull(rule);

            foreach (var expectedTag in new[] { "one_tag", "two_tag", "red_tag", "blue_tag" })
                Assert.True(rule.Includes.Contains(expectedTag));

            Assert.True(rule.Excludes.Contains("green_tag"));

            Assert.True(rule.Targets.Contains("stream"));

            Assert.True(rule.StrictInclude);

            Assert.True(rule.Final);
        }

        #region Scaffolding

        private readonly INuLogConfigProvider _underTest;

        public SimpleConfigurationProvider_Tests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            var section = configuration.GetSection("NuLog");

            _underTest = new SimpleConfigurationProvider(section);
        }

        #endregion Scaffolding
    }
}