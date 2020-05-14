/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Layouts;
using System.Linq;
using Xunit;

namespace NuLog.Tests.Unit.Layouts.Standard.LayoutParsers {

    /// <summary>
    /// Documents (and verifies) the expected behavior of a layout parser.
    /// </summary>
    [Trait("Category", "Unit")]
    public class LayoutParserTests {

        /// <summary>
        /// The layout parser should find simple static text
        /// </summary>
        [Fact(DisplayName = "Should_ParseSimpleStaticText")]
        public void Should_ParseSimpleStaticText() {
            // Setup
            var layoutParser = GetLayoutParser();

            // Execute
            var parameters = layoutParser.Parse("Hello, World!");

            // Verify
            Assert.Equal(1, parameters.Count);
            var parameter = parameters.First();
            Assert.True(parameter.StaticText);
            Assert.Equal("Hello, World!", parameter.Text);
        }

        /// <summary>
        /// The layout parser should find a simple parameter
        /// </summary>
        [Fact(DisplayName = "Should_ParseSimpleParameter")]
        public void Should_ParseSimpleParameter() {
            // Setup
            var layoutParser = GetLayoutParser();

            // Execute
            var parameters = layoutParser.Parse("${HelloWorld}");

            // Verify
            Assert.Equal(1, parameters.Count);
            var parameter = parameters.First();
            Assert.False(parameter.StaticText);
            Assert.Equal("HelloWorld", parameter.Path);
        }

        /// <summary>
        /// The layout parser should find a nested parameter
        /// </summary>
        [Fact(DisplayName = "Should_ParseNestedParameter")]
        public void Should_ParseNestedParameter() {
            // Setup
            var layoutParser = GetLayoutParser();

            // Execute
            var parameters = layoutParser.Parse("${HelloWorld.MyProperty}");

            // Verify
            Assert.Equal(1, parameters.Count);
            var parameter = parameters.First();
            Assert.False(parameter.StaticText);
        }

        /// <summary>
        /// The layout parser should find a parameter format
        /// </summary>
        [Fact(DisplayName = "Should_ParseParameterFormat")]
        public void Should_ParseParameterFormat() {
            // Setup
            var layoutParser = GetLayoutParser();

            // Execute
            var parameters = layoutParser.Parse("${DateTime:': {0:MM/DD/YYYY}'}");

            // Verify
            Assert.Equal(1, parameters.Count);
            var parameter = parameters.First();
            Assert.False(parameter.StaticText);
            Assert.Equal(": {0:MM/DD/YYYY}", parameter.Format);
        }

        /// <summary>
        /// The layout parser should find a parameter format, of a nested parameter
        /// </summary>
        [Fact(DisplayName = "Should_ParseNestedParameterFormat")]
        public void Should_ParseNestedParameterFormat() {
            // Setup
            var layoutParser = GetLayoutParser();

            // Execute
            var parameters = layoutParser.Parse("${MyObject.DateTime:'MM/DD/YYYY'}");

            // Verify
            Assert.Equal(1, parameters.Count);
            var parameter = parameters.First();
            Assert.False(parameter.StaticText);
            Assert.Equal("MM/DD/YYYY", parameter.Format);
        }

        /// <summary>
        /// The layout parser should find a simple contingent parameter
        /// </summary>
        [Fact(DisplayName = "Should_ParseSimpleContingentParameter")]
        public void Should_ParseSimpleContingentParameter() {
            // Setup
            var layoutParser = GetLayoutParser();

            // Execute
            var parameters = layoutParser.Parse("${?HelloWorld}");

            // Verify
            Assert.Equal(1, parameters.Count);
            var parameter = parameters.First();
            Assert.False(parameter.StaticText);
            Assert.True(parameter.Contingent);
            Assert.Equal("HelloWorld", parameter.Path);
        }

        /// <summary>
        /// The layout parser should parse a complex layout.
        /// </summary>
        [Fact(DisplayName = "Should_ParseComplexLayout")]
        public void Should_ParseComplexLayout() {
            // Setup
            var layoutParser = GetLayoutParser();

            // Execute
            var parameters = layoutParser.Parse("${DateTime:'{0:MM/dd/yyyy hh:mm:ss.fff}'} | ${Thread.ManagedThreadId:'{0,4}'} | ${Tags} | ${Message}${?Exception:'\r\n{0}'}\r\n");

            // Verify
            Assert.Equal(9, parameters.Count);

            // 0 - DateTime
            var dtParm = parameters.ElementAt(0);
            Assert.False(dtParm.StaticText);
            Assert.False(dtParm.Contingent);
            Assert.Equal("DateTime", dtParm.Path);
            Assert.Equal("{0:MM/dd/yyyy hh:mm:ss.fff}", dtParm.Format);

            // 1 - Static Text " | "
            var st1Parm = parameters.ElementAt(1);
            Assert.True(st1Parm.StaticText);
            Assert.Equal(" | ", st1Parm.Text);

            // 2 - Thread.ManagedThreadId
            var threadIdParm = parameters.ElementAt(2);
            Assert.False(threadIdParm.StaticText);
            Assert.False(threadIdParm.Contingent);
            Assert.Equal("Thread.ManagedThreadId", threadIdParm.Path);
            Assert.Equal("{0,4}", threadIdParm.Format);

            // 3 - Static Text " | "
            var st3Parm = parameters.ElementAt(3);
            Assert.True(st3Parm.StaticText);
            Assert.Equal(" | ", st3Parm.Text);

            // 4 - Tags
            var tagsParm = parameters.ElementAt(4);
            Assert.False(tagsParm.StaticText);
            Assert.False(tagsParm.Contingent);
            Assert.Equal("Tags", tagsParm.Path);

            // 5 - Static Text " | "
            var st5Parm = parameters.ElementAt(5);
            Assert.True(st5Parm.StaticText);
            Assert.Equal(" | ", st5Parm.Text);

            // 6 - Message
            var messageParm = parameters.ElementAt(6);
            Assert.False(messageParm.StaticText);
            Assert.False(messageParm.Contingent);
            Assert.Equal("Message", messageParm.Path);

            // 7 - Exception
            var excParm = parameters.ElementAt(7);
            Assert.False(excParm.StaticText);
            Assert.True(excParm.Contingent);
            Assert.Equal("Exception", excParm.Path);
            Assert.Equal("\r\n{0}", excParm.Format);

            // 8 - Static Text "\r\n"
            var st8Parm = parameters.ElementAt(8);
            Assert.True(st8Parm.StaticText);
            Assert.Equal("\r\n", st8Parm.Text);
        }

        /// <summary>
        /// Gets a new instance of the layout parser under test.
        /// </summary>
        protected ILayoutParser GetLayoutParser() {
            return new StandardLayoutParser();
        }
    }
}