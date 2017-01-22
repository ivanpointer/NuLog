/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.Layouts;
using NuLog.LogEvents;
using NuLog.Targets;
using System;
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

        /// <summary>
        /// The layout should render a piece of meta data from the log event.
        /// </summary>
        [Fact(DisplayName = "Should_RenderMetaData")]
        public void Should_RenderMetaData()
        {
            // Setup
            var parms = new List<LayoutParameter>
            {
                new LayoutParameter
                {
                    Path = "SomeMeta"
                }
            };
            var logEvent = new LogEvent();
            var paramParser = A.Fake<IPropertyParser>();

            // Setup the response for some meta
            A.CallTo(() => paramParser.GetProperty(logEvent.MetaData, "SomeMeta"))
                .Returns(new DateTime(2017, 1, 16, 12, 1, 42));

            // Build our layout
            var layout = GetLayout(parms, paramParser);

            // Execute
            var formatted = layout.Format(logEvent);

            // Verify
            Assert.Equal("1/16/2017 12:01:42 PM", formatted);
        }

        /// <summary>
        /// The layout should look in the meta data before looking at the log event (meta data can
        /// override log event properties).
        /// </summary>
        [Fact(DisplayName = "Should_RenderMetaDataFirst")]
        public void Should_RenderMetaDataFirst()
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

            // Setup the response for our properties
            A.CallTo(() => paramParser.GetProperty(logEvent, "Message"))
                .Returns("Me!");
            A.CallTo(() => paramParser.GetProperty(logEvent.MetaData, "Message"))
                .Returns("No, me!");

            // Build our layout
            var layout = GetLayout(parms, paramParser);

            // Execute
            var formatted = layout.Format(logEvent);

            // Verify
            Assert.Equal("No, me!", formatted);
        }

        /// <summary>
        /// The layout should render a parameter using its format
        /// </summary>
        [Fact(DisplayName = "Should_RenderParameterFormatted")]
        public void Should_RenderParameterFormatted()
        {
            // Setup
            var parms = new List<LayoutParameter>
            {
                new LayoutParameter
                {
                    Path = "DateLogged",
                    Format = "{0:MM/dd/yyyy}"
                }
            };
            var logEvent = new LogEvent();
            var paramParser = A.Fake<IPropertyParser>();

            // The standard layout looks at the meta first, and FakeItEasy returns an object by
            // default - instead of null. We need to override this default behavior to return null here...
            A.CallTo(() => paramParser.GetProperty(logEvent.MetaData, "DateLogged"))
                .Returns(null);

            // Now, setup our mock parameter parser to return "hello, world!" when asked for
            // "Message" from the log event
            A.CallTo(() => paramParser.GetProperty(logEvent, "DateLogged"))
                .Returns(new DateTime(2017, 1, 16, 12, 1, 42));

            // Build our layout
            var layout = GetLayout(parms, paramParser);

            // Execute
            var formatted = layout.Format(logEvent);

            // Verify
            Assert.Equal("01/16/2017", formatted);
        }

        /// <summary>
        /// The layout should render a contingent parameter
        /// </summary>
        [Fact(DisplayName = "Should_RenderContingentParameter")]
        public void Should_RenderContingentParameter()
        {
            // Setup
            var parms = new List<LayoutParameter>
            {
                new LayoutParameter
                {
                    StaticText = true,
                    Text = "LOG MESSAGE"
                },
                new LayoutParameter
                {
                    Path = "MyStuffs",
                    Contingent = true,
                    Format = " | HELLO MY STUFFS: {0}"
                }
            };
            var logEvent = new LogEvent();
            var paramParser = A.Fake<IPropertyParser>();

            // Set a return value in meta because it's easier.
            A.CallTo(() => paramParser.GetProperty(logEvent.MetaData, "MyStuffs"))
                .Returns("Super!");

            // Build our layout
            var layout = GetLayout(parms, paramParser);

            // Execute
            var formatted = layout.Format(logEvent);

            // Verify
            Assert.Equal("LOG MESSAGE | HELLO MY STUFFS: Super!", formatted);
        }

        /// <summary>
        /// The layout should exclude a null/empty contingent parameter
        /// </summary>
        [Fact(DisplayName = "Should_ExcludeMissingContingentParameter")]
        public void Should_ExcludeMissingContingentParameter()
        {
            // Setup
            var parms = new List<LayoutParameter>
            {
                new LayoutParameter
                {
                    StaticText = true,
                    Text = "LOG MESSAGE"
                },
                new LayoutParameter
                {
                    Path = "MyStuffs",
                    Contingent = true,
                    Format = " | HELLO MY STUFFS: {0}"
                }
            };
            var logEvent = new LogEvent();
            var paramParser = A.Fake<IPropertyParser>();

            // Setup our returns
            A.CallTo(() => paramParser.GetProperty(logEvent.MetaData, "MyStuffs"))
                .Returns(null);
            A.CallTo(() => paramParser.GetProperty(logEvent, "MyStuffs"))
                .Returns(null);

            // Build our layout
            var layout = GetLayout(parms, paramParser);

            // Execute
            var formatted = layout.Format(logEvent);

            // Verify
            Assert.Equal("LOG MESSAGE", formatted);
        }

        /// <summary>
        /// Tags are treated special as a parameter - they should be printed as a CSV, with no white space.
        /// </summary>
        [Fact(DisplayName = "Should_FormatTagsSpecial")]
        public void Should_FormatTagsSpecial()
        {
            // Setup
            var parms = new List<LayoutParameter>
            {
                new LayoutParameter
                {
                    Path = "Tags"
                }
            };
            var logEvent = new LogEvent
            {
                Tags = new string[] { "one_tag", "two_tag", "red_tag", "blue_tag" }
            };
            var paramParser = A.Fake<IPropertyParser>();

            // Notice we don't setup a mock for the meta data or property - that's because we expect
            // this to be processed before it even gets to that point - special properties are
            // handled first.

            // Build our layout
            var layout = GetLayout(parms, paramParser);

            // Execute
            var formatted = layout.Format(logEvent);

            // Verify
            Assert.Equal("one_tag,two_tag,red_tag,blue_tag", formatted);
        }

        /// <summary>
        /// An exception is handled as a special parameter.
        /// </summary>
        [Fact(DisplayName = "Should_FormatExceptionSpecial")]
        public void Should_FormatExceptionSpecial()
        {
            // Setup
            var parms = new List<LayoutParameter>
            {
                new LayoutParameter
                {
                    Path = "Exception"
                }
            };
            var logEvent = new LogEvent
            {
                Exception = new Exception("Ha! A test exception!")
            };
            var paramParser = A.Fake<IPropertyParser>();

            // Notice we don't setup a mock for the meta data or property - that's because we expect
            // this to be processed before it even gets to that point - special properties are
            // handled first.

            // Build our layout
            var layout = GetLayout(parms, paramParser);

            // Execute
            var formatted = layout.Format(logEvent);

            // Verify
            Assert.Contains("System.Exception: Ha! A test exception!", formatted);
        }

        /// <summary>
        /// An exception is handled as a special parameter. Inner exceptions should also be included.
        /// </summary>
        [Fact(DisplayName = "Should_FormatInnerExceptionSpecial")]
        public void Should_FormatInnerExceptionSpecial()
        {
            // Setup
            var parms = new List<LayoutParameter>
            {
                new LayoutParameter
                {
                    Path = "Exception"
                }
            };
            var logEvent = new LogEvent
            {
                Exception = new Exception("Uh, something broke!", new Exception("Freaking segfault!"))
            };
            var paramParser = A.Fake<IPropertyParser>();

            // Notice we don't setup a mock for the meta data or property - that's because we expect
            // this to be processed before it even gets to that point - special properties are
            // handled first.

            // Build our layout
            var layout = GetLayout(parms, paramParser);

            // Execute
            var formatted = layout.Format(logEvent);

            // Verify
            Assert.Contains("System.Exception: Uh, something broke!", formatted);
            Assert.Contains("Caused by System.Exception: Freaking segfault!", formatted);
        }

        /// <summary>
        /// An enumerable list of objects should be concatenated into a CSV list.
        /// </summary>
        [Fact(DisplayName = "Should_CSVEnumerable")]
        public void Should_CSVEnumerable()
        {
            // Setup
            var parms = new List<LayoutParameter>
            {
                new LayoutParameter
                {
                    Path = "Enumerable"
                }
            };
            var logEvent = new LogEvent();
            var paramParser = A.Fake<IPropertyParser>();

            // Set a return value in meta
            A.CallTo(() => paramParser.GetProperty(logEvent.MetaData, "Enumerable"))
                .Returns(new ToStringTestClass[] {
                    new ToStringTestClass
                    {
                        MyString = "one_thing"
                    },
                    new ToStringTestClass
                    {
                        MyString = "two_thing"
                    }
                });

            // Build our layout
            var layout = GetLayout(parms, paramParser);

            // Execute
            var formatted = layout.Format(logEvent);

            // Verify
            Assert.Equal("[To String: \"one_thing\",To String: \"two_thing\"]", formatted);
        }

        /// <summary>
        /// Get a new instance of the layout under test.
        /// </summary>
        protected ILayout GetLayout(IEnumerable<LayoutParameter> layoutParameters, IPropertyParser propertyParser)
        {
            return new StandardLayout(layoutParameters, propertyParser);
        }
    }

    /// <summary>
    /// A test class for testing the formatting functionality of the layout.
    /// </summary>
    internal sealed class ToStringTestClass
    {
        public string MyString { get; set; }

        public override string ToString()
        {
            return string.Format("To String: \"{0}\"", MyString);
        }
    }
}