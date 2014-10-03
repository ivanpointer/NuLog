using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuLog.Configuration;
using NuLog.Dispatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Test
{
    [TestClass]
    public class LogEventDispatcherTest
    {
        [TestMethod]
        public void TestTagList()
        {
            var config = new LoggingConfig();
            config.LoadConfig();
            var dispatcher = new LogEventDispatcher(config);

            var tagList = new List<string> { "fatal", "error", "warn", "info", "debug", "trace" };
            var tagKeeper = dispatcher.TagKeeper;
            for (int lp = 0; lp < tagList.Count - 1; lp++)
                for (int lp2 = lp + 1; lp2 < tagList.Count; lp2++)
                    Assert.IsTrue(tagKeeper.CheckMatch(tagList[lp], tagList[lp2]), String.Format("Checking {0} = {1}", tagList[lp2], tagList[lp]));
        }

        [TestMethod]
        public void TestTagListWildcard()
        {
            var config = new LoggingConfig();
            config.LoadConfig();
            var dispatcher = new LogEventDispatcher(config);
            var tagKeeper = dispatcher.TagKeeper;

            Assert.IsTrue(tagKeeper.CheckMatch("helloworld", "hello*"));
        }
    }
}
