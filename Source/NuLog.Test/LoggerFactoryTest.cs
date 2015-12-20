using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuLog.Configuration;
using NuLog.Configuration.Targets;
using NuLog.Test.TestHarness;
using System;

namespace NuLog.Test
{
    [TestClass]
    public class LoggerFactoryTest
    {
        [TestMethod]
        public void TestGetLogger()
        {
            LoggerFactory.GetLogger(defaultTags: new string[] { "abc", "123" });
        }

        [TestMethod]
        public void TestShutdown()
        {
            var config = LoggingConfigBuilder.CreateLoggingConfig()
                .AddTarget(new TargetConfig
                {
                    Name = "list",
                    Type = String.Format("{0}, NuLog.Test", typeof(ListTarget).FullName)
                })
                .Build();

            LoggerFactory.Initialize(config);

            var logger = LoggerFactory.GetLogger();

            ListTarget.ClearList();
            for (var x = 0; x < 1000; x++)
                logger.Log("Log message " + x);

            LoggerFactory.Shutdown();
            var list = ListTarget.GetList();
            Assert.AreEqual(1000, list.Count);
        }
    }
}