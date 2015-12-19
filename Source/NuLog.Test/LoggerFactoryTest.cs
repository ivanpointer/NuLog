using Microsoft.VisualStudio.TestTools.UnitTesting;

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