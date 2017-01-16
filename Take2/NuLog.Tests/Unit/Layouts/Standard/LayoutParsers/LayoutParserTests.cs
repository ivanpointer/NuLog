/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Layouts;
using NuLog.Layouts.Standard.LayoutParsers;
using System.Linq;
using Xunit;

namespace NuLog.Tests.Unit.Layouts.Standard.LayoutParsers
{
    /* Documentation / Explanation:
     *
     * Layouts are a mechanism for converting a log event into text using a "layout" format
     * Layouts are used by the standard text-based targets, and the SMTP target for the subject and the body
     * Layouts allow for the formatting of different parts of the log event, even recursively
     * Layouts allow for conditionally showing formatted parts of the log event
     *
     * "Hello Layout${?DateTime:': {0:MM/dd/yyyy hh:mm:ss.fff}'}!\r\n"
     *
     * Static Text:
     *  - Anything not wrapped in a parameter enclosure ${} is treated as static text
     *  - Static text will always show in a log event formatted by a layout
     *  - Escaped characters (such as carriage return and line feed) are supported, and encouraged
     *
     * Parameters:
     *  ${?DateTime:': {0:MM/dd/yyyy hh:mm:ss.fff}'}
     *
     *  - Parameters are wrapped with the property enclosure ${}
     *  - A single parameter in the layout format refers to a single property of the log event
     *  - Parameters have three parts:
     *    - Conditional Flag (Optional)
     *    - Property Name (Required)
     *    - Property Format (Optional)
     *
     *  - Conditional Flag:
     *    - The conditional flag is a single '?' located at the front of the property, inside the enclosure ${}
     *    - If the conditional flag is present, the property will only be included in the resulting text if the property is not null or empty
     *
     *  - Property Name:
     *    - The name of the property within the log event is located at the beginning of the property string, after the conditional flag
     *    - All log events have optional "Meta Data"
     *    - The Property Name value is reflective and recursive, child values can be accessed with periods, for example: DateTime.Day
     *    - The "Meta Data" is searched first for the property
     *    - The log event is searched for the property, if the property is not found in the "Meta Data"
     *
     *  - Property Format:
     *    - The property format is used to format the value of the property which was evaluated from the log event
     *    - The property format is separated from the property name by a colon ':'
     *    - The property format is wrapped in single quotes to allow for escaping within the format string
     *    - The framework uses System.string.Format with the property format and value
     */

    /// <summary>
    /// Documents (and verifies) the expected behavior of a layout parser.
    /// </summary>
    [Trait("Category", "Unit")]
    public class LayoutParserTests
    {
        /// <summary>
        /// The layout parser should find simple static text
        /// </summary>
        [Fact(DisplayName = "Should_ParseSimpleStaticText")]
        public void Should_ParseSimpleStaticText()
        {
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
        public void Should_ParseSimpleParameter()
        {
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
        public void Should_ParseNestedParameter()
        {
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
        public void Should_ParseParameterFormat()
        {
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
        public void Should_ParseNestedParameterFormat()
        {
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
        public void Should_ParseSimpleContingentParameter()
        {
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
        public void Should_ParseComplexLayout()
        {
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
        protected ILayoutParser GetLayoutParser()
        {
            return new StandardLayoutParser();
        }
    }
}