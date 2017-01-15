/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.Dispatch;
using System.Collections.Generic;
using Xunit;

namespace NuLog.Tests.Integration.Dispatchers
{
    /// <summary>
    /// MIGRATED FROM EXISTING TEST PROJECT - Need to take a look at each of the NuLog components
    /// again, and rework to the DIP, to have better test isolation (I.E. true unit tests).
    ///
    /// I pulled these in as "integration" tests, even though some may qualify as unit tests, as this
    /// is a quick and dirty port.
    ///
    /// Documents (and validates) the expected behavior of the standard log event dispatcher.
    /// </summary>
    [Trait("Category", "Integration")]
    [Trait("Domain", "Dispatchers")]
    public class LogEventDispatcherTests
    {
        [Fact]
        public void TestTagList()
        {
            var config = new LoggingConfig();
            config.LoadConfig();
            var dispatcher = new LogEventDispatcher(config);

            var tagList = new List<string> { "fatal", "error", "warn", "info", "debug", "trace" };
            var tagKeeper = dispatcher.TagKeeper;
            for (int lp = 0; lp < tagList.Count - 1; lp++)
                for (int lp2 = lp + 1; lp2 < tagList.Count; lp2++)
                    Assert.True(tagKeeper.CheckMatch(tagList[lp], tagList[lp2]), string.Format("Checking {0} = {1}", tagList[lp2], tagList[lp]));
        }

        [Fact]
        public void TestTagListWildcard()
        {
            var config = new LoggingConfig();
            config.LoadConfig();
            var dispatcher = new LogEventDispatcher(config);
            var tagKeeper = dispatcher.TagKeeper;

            Assert.True(tagKeeper.CheckMatch("helloworld", "hello*"));
        }
    }
}