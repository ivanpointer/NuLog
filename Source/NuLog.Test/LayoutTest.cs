using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using NuLog.Configuration.Layouts;
using NuLog.Layouts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace NuLog.Test
{
    [TestClass]
    public class LayoutTest
    {
        [TestMethod]
        public void TestFindParameters()
        {
            var parameters = StandardLayout.FindParameterTexts("${HelloWorld} ${Hello.World} ${?HelloWorld} ${?Hello.World} ${HelloWorld:'{0}'} ${?Hello.World:' | {0}'} ${?Hello.World:'{0}!'}");

            Assert.IsTrue(parameters.Contains("${HelloWorld}"));
            Assert.IsTrue(parameters.Contains("${Hello.World}"));
            Assert.IsTrue(parameters.Contains("${?HelloWorld}"));
            Assert.IsTrue(parameters.Contains("${?Hello.World}"));
            Assert.IsTrue(parameters.Contains("${HelloWorld:'{0}'}"));
            Assert.IsTrue(parameters.Contains("${?Hello.World:' | {0}'}"));
            Assert.IsTrue(parameters.Contains("${?Hello.World:'{0}!'}"));
        }

        [TestMethod]
        public void TestMessage()
        {
            var layout = new StandardLayout("${Message}");

            string guid = Guid.NewGuid().ToString();
            string actualString = layout.FormatLogEvent(new LogEvent
            {
                Message = guid
            });

            Assert.AreEqual(guid, actualString);
        }

        [TestMethod]
        public void TestDateTimeFormat()
        {
            var layout = new StandardLayout("${DateTime:'{0:yyyyMMdd}'}");

            var now = DateTime.Now;
            string actualString = layout.FormatLogEvent(new LogEvent
            {
                DateTime = now
            });

            var expectedString = now.ToString("yyyyMMdd");

            Assert.AreEqual(actualString, expectedString);
        }

        [TestMethod]
        public void TestContingent()
        {
            var layout = new StandardLayout("${Message}${?Exception.Message:' | {0}'}");

            string expectedWithError = "Hello, World! | Hello, Error!";
            string actualWithError = layout.FormatLogEvent(new LogEvent
            {
                Message = "Hello, World!",
                Exception = new Exception("Hello, Error!")
            });

            Assert.AreEqual(expectedWithError, actualWithError);

            string expectedWithoutError = "Hello, World!";
            string actualWithoutError = layout.FormatLogEvent(new LogEvent
            {
                Message = "Hello, World!"
            });

            Assert.AreEqual(expectedWithoutError, actualWithoutError);
        }

        [TestMethod]
        public void TestTags()
        {
            var layout = new StandardLayout("${Tags}");

            string expected = "this,is,a,test";
            string actual = layout.FormatLogEvent(new LogEvent
            {
                Tags = new List<string> { "this", "is", "a", "test" }
            });

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestFactoryNoLayout()
        {
            string jsonString = ReadFileText(@"Configs\NoLayoutTest.json");
            JObject jsonConfig = JObject.Parse(jsonString);
            JToken layoutConfig = jsonConfig["layout"];

            var layout = LayoutFactory.BuildLayout(new LayoutConfig(layoutConfig));

            var now = DateTime.Now;
            string actual;
            try
            {
                throw new Exception("Hello, Exception!");
            }
            catch (Exception inner)
            {
                try
                {
                    throw new LoggingException("Outer exception", inner);
                }
                catch (Exception outer)
                {
                    actual = layout.FormatLogEvent(new LogEvent
                    {
                        DateTime = now,
                        Message = "Hello, World!",
                        Exception = outer,
                        Tags = new List<string> { "hello", "world" }
                    });
                }
            }

            string expected = String.Format(@"{0} | {1,4} | hello,world | Hello, World!
NuLog.LoggingException: Outer exception
   at NuLog.Test.LayoutTest.TestFactoryNoLayout() in ", now.ToString("MM/dd/yyyy hh:mm:ss.fff"), Thread.CurrentThread.ManagedThreadId);

            Assert.IsTrue(actual.StartsWith(expected));
        }

        [TestMethod]
        public void TestFactorySimpleLayout()
        {
            string jsonString = ReadFileText(@"Configs\SimpleLayoutTest.json");
            JObject jsonConfig = JObject.Parse(jsonString);
            JToken layoutConfig = jsonConfig["layout"];

            var layout = LayoutFactory.BuildLayout(new LayoutConfig(layoutConfig));
        }

        [TestMethod]
        public void TestFactoryComplexLayout()
        {
            string jsonString = ReadFileText(@"Configs\ComplexLayoutTest.json");
            JObject jsonConfig = JObject.Parse(jsonString);
            JToken layoutConfig = jsonConfig["layout"];

            var layout = LayoutFactory.BuildLayout(new LayoutConfig(layoutConfig));

            var now = DateTime.Now;
            string actual = layout.FormatLogEvent(new LogEvent
            {
                DateTime = now,
                Message = "Hello, World!"
            });

            string expected = String.Format("{0:yyyyMMdd} | Hello, World!", now);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMetaData()
        {
            var layout = new StandardLayout("${HelloWorld:'{0}!'}");
            var actual = layout.FormatLogEvent(new LogEvent
            {
                MetaData = new Dictionary<string, object>
                {
                    {"HelloWorld", "Hello, World"}
                }
            });

            var expected = "Hello, World!";
            Assert.AreEqual(expected, actual);
        }

        #region helpers

        private static string ReadFileText(string configFile)
        {
            using (var fileStream = File.Open(configFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var streamReader = new StreamReader(fileStream))
                return streamReader.ReadToEnd();
        }

        #endregion

    }
}
