using NuLog.Layouts;
using System;
using System.Collections.Generic;
using Xunit;

namespace NuLog.Tests.Unit.Layouts
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the standard layout.
    /// </summary>
    [Trait("Category", "Unit")]
    [Trait("Domain", "Layouts")]
    public class StandardLayoutTests
    {
        [Fact(DisplayName = "Should_FindParameters")]
        public void Should_FindParameters()
        {
            var parameters = StandardLayout.FindParameterTexts("${HelloWorld} ${Hello.World} ${?HelloWorld} ${?Hello.World} ${HelloWorld:'{0}'} ${?Hello.World:' | {0}'} ${?Hello.World:'{0}!'}");

            Assert.True(parameters.Contains("${HelloWorld}"));
            Assert.True(parameters.Contains("${Hello.World}"));
            Assert.True(parameters.Contains("${?HelloWorld}"));
            Assert.True(parameters.Contains("${?Hello.World}"));
            Assert.True(parameters.Contains("${HelloWorld:'{0}'}"));
            Assert.True(parameters.Contains("${?Hello.World:' | {0}'}"));
            Assert.True(parameters.Contains("${?Hello.World:'{0}!'}"));
        }

        [Fact(DisplayName = "Should_InjectSimpleString")]
        public void Should_InjectSimpleString()
        {
            var layout = new StandardLayout("${Message}");

            var guid = Guid.NewGuid().ToString();
            var actualString = layout.FormatLogEvent(new LogEvent
            {
                Message = guid
            });

            Assert.Equal(guid, actualString);
        }

        [Fact(DisplayName = "Should_FormatDateTime")]
        public void Should_FormatDateTime()
        {
            var layout = new StandardLayout("${DateTime:'{0:yyyyMMdd}'}");

            var now = DateTime.Now;
            string actualString = layout.FormatLogEvent(new LogEvent
            {
                DateTime = now
            });

            var expectedString = now.ToString("yyyyMMdd");

            Assert.Equal(actualString, expectedString);
        }

        [Fact(DisplayName = "Should_IncluParmContingently_Include")]
        public void Should_IncluParmContingently_Include()
        {
            var layout = new StandardLayout("${Message}${?Exception.Message:' | {0}'}");

            string expectedWithError = "Hello, World! | Hello, Error!";
            string actualWithError = layout.FormatLogEvent(new LogEvent
            {
                Message = "Hello, World!",
                Exception = new Exception("Hello, Error!")
            });

            Assert.Equal(expectedWithError, actualWithError);
        }

        [Fact(DisplayName = "Should_IncluParmContingently_Exclude")]
        public void Should_IncluParmContingently_Exclude()
        {
            var layout = new StandardLayout("${Message}${?Exception.Message:' | {0}'}");

            string expectedWithoutError = "Hello, World!";
            string actualWithoutError = layout.FormatLogEvent(new LogEvent
            {
                Message = "Hello, World!"
            });

            Assert.Equal(expectedWithoutError, actualWithoutError);
        }

        [Fact(DisplayName = "Should_ConcatTagsCSV")]
        public void Should_ConcatTagsCSV()
        {
            var layout = new StandardLayout("${Tags}");

            string expected = "this,is,a,test";
            string actual = layout.FormatLogEvent(new LogEvent
            {
                Tags = new List<string> { "this", "is", "a", "test" }
            });

            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "Should_ReadParmFromMeta")]
        public void Should_ReadParmFromMeta()
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
            Assert.Equal(expected, actual);
        }
    }
}