/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using Newtonsoft.Json.Linq;
using NuLog.Configuration.Layouts;
using NuLog.Layouts;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace NuLog.Tests.Integration.Layouts
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the layout factory.
    /// </summary>
    [Trait("Category", "Integration")]
    [Trait("Domain", "Layouts")]
    public class LayoutFactoryTests
    {
        [Fact(DisplayName = "Should_IncludeExceptionInDefaultLayout")]
        public void Should_IncludeExceptionInDefaultLayout()
        {
            var jsonString = File.ReadAllText(@"Configs\NoLayoutTest.json");
            var jsonConfig = JObject.Parse(jsonString);
            var layoutConfig = jsonConfig["layout"];

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

            Assert.True(actual.Contains("hello,world"));
            Assert.True(actual.Contains("Hello, World!"));
            Assert.True(actual.Contains("NuLog.LoggingException: Outer exception"));
            Assert.True(actual.Contains("at NuLog.Tests.Integration.Layouts.LayoutFactoryTests.Should_IncludeExceptionInDefaultLayout()"));
        }

        [Fact(DisplayName = "Should_BuildLayoutWithJTokenConfig")]
        public void Should_BuildLayoutWithJTokenConfig()
        {
            string jsonString = File.ReadAllText(@"Configs\SimpleLayoutTest.json");
            JObject jsonConfig = JObject.Parse(jsonString);
            JToken layoutConfig = jsonConfig["layout"];

            var layout = LayoutFactory.BuildLayout(new LayoutConfig(layoutConfig));
        }

        [Fact(DisplayName = "Should_BuildLayoutWithCustomLayout")]
        public void Should_BuildLayoutWithCustomLayout()
        {
            string jsonString = File.ReadAllText(@"Configs\ComplexLayoutTest.json");
            JObject jsonConfig = JObject.Parse(jsonString);
            JToken layoutConfig = jsonConfig["layout"];

            var layout = LayoutFactory.BuildLayout(new LayoutConfig(layoutConfig));

            var now = DateTime.Now;
            string actual = layout.FormatLogEvent(new LogEvent
            {
                DateTime = now,
                Message = "Hello, World!"
            });

            string expected = string.Format("{0:yyyyMMdd} | Hello, World!", now);

            Assert.Equal(expected, actual);
        }
    }
}