using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuLog.Configuration;
using NuLog.Configuration.Targets;
using NuLog.Test.TestHarness;
using System;
using System.Threading;

namespace NuLog.Test
{
    [TestClass]
    public class MetaDataProviderTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            ListTarget.ClearList();
        }

        [TestMethod]
        public void TestMetaDataProvider()
        {
            var config = LoggingConfigBuilder.CreateLoggingConfig()
                .AddTarget(new TargetConfig
                {
                    Name = "list",
                    Type = String.Format("{0}, NuLog.Test", typeof(ListTarget).FullName)
                })
                .Build();

            LoggerFactory.Initialize(config);

            var metaDataProvider = new TestMetaDataProvider();

            var logger = LoggerFactory.GetLogger(metaDataProvider);

            logger.LogNow("This is a test");

            for (int checks = 0; checks < 20; checks++)
            {
                Thread.Sleep(100);
                if (ListTarget.GetList().Count > 0)
                    break;
            }

            var list = ListTarget.GetList();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count == 1);

            var message = list[0];
            Assert.IsNotNull(message);
            Assert.IsTrue(message.MetaData.ContainsKey("Hello"));
            Assert.IsTrue(message.MetaData["Hello"].ToString() == "World");
            Assert.IsTrue(message.MetaData.ContainsKey("Addtl"));
            Assert.IsTrue(message.MetaData["Addtl"].ToString() == "Data");
        }
    }
}