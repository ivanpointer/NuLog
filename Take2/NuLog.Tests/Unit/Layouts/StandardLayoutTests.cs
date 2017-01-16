/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Layouts;
using NuLog.Targets;
using System;
using System.Collections.Generic;
using Xunit;

namespace NuLog.Tests.Unit.Layouts
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
    /// Documents the expected behavior of a layout.
    /// </summary>
    [Trait("Category", "Unit")]
    public class StandardLayoutTests
    {
        /// <summary>
        /// The layout should render simple, static text.
        /// </summary>
        [Fact(DisplayName = "Should_RenderStaticText")]
        public void Should_RenderStaticText()
        {
            // Setup
            var layout = GetLayout("hello, world!");

            // Execute
            var formatted = layout.Format(new StubLogEvent());

            // Verify
            Assert.Equal("hello, world!", formatted);
        }

        ///// <summary>
        ///// The layout should render a parameter from the log event.
        ///// </summary>
        //[Fact(DisplayName = "Should_RenderParameter")]
        //public void Should_RenderParameter()
        //{
        //    // Setup
        //    var layout = GetLayout("${DateLogged}");

        //    // Execute
        //    var formatted = layout.Format(new StubLogEvent
        //    {
        //        DateLogged = new DateTime(2017, 1, 16, 0, 36, 7)
        //    });

        //    // Verify
        //    Assert.Equal("hello, world!", formatted);
        //}

        /// <summary>
        /// Get a new instance of the layout under test.
        /// </summary>
        protected ILayout GetLayout(string layout)
        {
            return new StandardLayout(layout);
        }
    }

    /// <summary>
    /// A stub implementation of a log event, for testing the layout.
    /// </summary>
    internal class StubLogEvent : ILogEvent
    {
        public DateTime DateLogged { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public void Dispose()
        {
            // Nothing to do
        }

        public void WriteTo(ITarget target)
        {
            // Stubbed
        }
    }
}