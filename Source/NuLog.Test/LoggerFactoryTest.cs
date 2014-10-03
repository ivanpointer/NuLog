using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
