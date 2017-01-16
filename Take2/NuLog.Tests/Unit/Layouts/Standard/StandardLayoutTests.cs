/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.Layouts;
using NuLog.Layouts.Standard;
using NuLog.Targets;
using System.Collections.Generic;
using Xunit;

namespace NuLog.Tests.Unit.Layouts.Standard
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the standard layout.
    /// </summary>
    [Trait("Category", "Integration")]
    public class StandardLayoutTests
    {
        /// <summary>
        /// The layout should render simple, static text.
        /// </summary>
        [Fact(DisplayName = "Should_RenderStaticText")]
        public void Should_RenderStaticText()
        {
            // Setup
            var parms = new List<LayoutParameter>
            {
                new LayoutParameter
                {
                    StaticText = true,
                    Text = "hello, world!"
                }
            };
            var paramParser = A.Fake<IPropertyParser>();
            var layout = GetLayout(parms, paramParser);

            // Execute
            var formatted = layout.Format(new LogEvent());

            // Verify
            Assert.Equal("hello, world!", formatted);
        }

        /// <summary>
        /// The layout should render a parameter from the log event.
        /// </summary>
        [Fact(DisplayName = "Should_RenderParameter")]
        public void Should_RenderParameter()
        {
            // Setup
            var parms = new List<LayoutParameter>
            {
                new LayoutParameter
                {
                    Path = "Message"
                }
            };
            var logEvent = new LogEvent();
            var paramParser = A.Fake<IPropertyParser>();

            // The standard layout looks at the meta first, and FakeItEasy returns an object by
            // default - instead of null. We need to override this default behavior to return null here...
            A.CallTo(() => paramParser.GetProperty(logEvent.MetaData, "Message"))
                .Returns(null);

            // Now, setup our mock parameter parser to return "hello, world!" when asked for
            // "Message" from the log event
            A.CallTo(() => paramParser.GetProperty(logEvent, "Message"))
                .Returns("hello, world!");

            // Build our layout
            var layout = GetLayout(parms, paramParser);

            // Execute
            var formatted = layout.Format(logEvent);

            // Verify
            Assert.Equal("hello, world!", formatted);
        }

        // TODO - MORE TESTS!

        /// <summary>
        /// Get a new instance of the layout under test.
        /// </summary>
        protected ILayout GetLayout(IEnumerable<LayoutParameter> layoutParameters, IPropertyParser propertyParser)
        {
            return new StandardLayout(layoutParameters, propertyParser);
        }
    }
}