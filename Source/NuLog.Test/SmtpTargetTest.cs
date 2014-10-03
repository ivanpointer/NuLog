using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuLog.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Test
{
    [TestClass]
    public class SmtpTargetTest
    {
        [TestMethod]
        public void TestSimpleEmail()
        {
            var config = new LoggingConfig(@"Configs\SimpleEmailTest.json");
            LoggerFactory.Initialize(config);

            var logger = LoggerFactory.GetLogger();

            logger.Log("Hello, World!");
        }
    }
}
