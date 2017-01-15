/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using Xunit;

namespace NuLog.Tests.Integration.Targets
{
    /// <summary>
    /// MIGRATED FROM EXISTING TEST PROJECT - Need to take a look at each of the NuLog components
    /// again, and rework to the DIP, to have better test isolation (I.E. true unit tests).
    ///
    /// I pulled these in as "integration" tests, even though some may qualify as unit tests, as this
    /// is a quick and dirty port.
    ///
    /// Documents (and validates) the expected behavior of the SMTP target.
    /// </summary>
    [Trait("Category", "Integration")]
    [Trait("Domain", "Targets")]
    public class SmtpTargetTests
    {
        [Fact]
        public void TestSimpleEmail()
        {
            var config = new LoggingConfig(@"Configs\SimpleEmailTest.json");
            using (var factory = new LoggerFactory(config))
            {
                var logger = factory.Logger();

                logger.Log("Hello, World!");

                // Uh, no validation?
            }
        }
    }
}