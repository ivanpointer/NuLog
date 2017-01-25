/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.Configuration;
using NuLog.LogEvents;
using NuLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace NuLog.Tests.Unit.Targets
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the console target.
    /// </summary>
    [Trait("Category", "Unit")]
    public class ConsoleTargetTests : IDisposable
    {
        private readonly DummyTextWriter textWriter;

        private readonly TextWriter consoleTextWriter;

        private readonly ConsoleColor defaultBackgroundColor;

        private readonly ConsoleColor defaultForegroundColor;

        public ConsoleTargetTests()
        {
            this.textWriter = new DummyTextWriter();
            this.consoleTextWriter = Console.Out;
            Console.SetOut(this.textWriter);

            defaultBackgroundColor = Console.BackgroundColor;
            defaultForegroundColor = Console.ForegroundColor;
        }

        /// <summary>
        /// The target should write to the console.
        /// </summary>
        [Fact(DisplayName = "Should_WriteToConsole")]
        public void Should_WriteToConsole()
        {
            // Setup
            var logger = new ConsoleTarget();
            var layout = A.Fake<ILayout>();
            logger.SetLayout(layout);

            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Write to console!");

            // Execute
            logger.Write(new LogEvent
            {
                Message = "Write to console!"
            });

            // Validate
            Assert.True(this.textWriter.ConsoleMessages.Any(m => m.Message == "Write to console!"));
        }

        /// <summary>
        /// The target should use the layout when writing to console.
        /// </summary>
        [Fact(DisplayName = "Should_UseLayout")]
        public void Should_UseLayout()
        {
            // Setup
            var logger = new ConsoleTarget();
            var layout = A.Fake<ILayout>();
            logger.SetLayout(layout);

            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Write to console using layout!");

            // Execute
            logger.Write(new LogEvent
            {
                Message = "Write to console!"
            });

            // Validate
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored)).MustHaveHappened();
            Assert.True(this.textWriter.ConsoleMessages.Any(m => m.Message == "Write to console using layout!"));
        }

        /// <summary>
        /// The console logger should set the background color, if configured.
        /// </summary>
        [Fact(DisplayName = "Should_SetBackgroundColor")]
        public void Should_SetBackgroundColor()
        {
            // Setup
            var logger = new ConsoleTarget();
            logger.Configure(new TargetConfig
            {
                Properties = new Dictionary<string, object>
                {
                    { "background", "DarkGreen" }
                }
            });
            var layout = A.Fake<ILayout>();
            logger.SetLayout(layout);

            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Green background!");

            // Execute
            logger.Write(new LogEvent());

            // Validate
            Assert.True(this.textWriter.ConsoleMessages.Any(m => m.Message == "Green background!" && m.BackgroundColor == ConsoleColor.DarkGreen));
        }

        /// <summary>
        /// The default background color should be used, when not explicitly set.
        /// </summary>
        [Fact(DisplayName = "Should_UseDefaultBackgroud")]
        public void Should_UseDefaultBackgroud()
        {
            // Setup
            var logger = new ConsoleTarget();
            var layout = A.Fake<ILayout>();
            logger.SetLayout(layout);

            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Default background!");

            // Execute
            logger.Write(new LogEvent());

            // Validate
            Assert.True(this.textWriter.ConsoleMessages.Any(m => m.Message == "Default background!" && m.BackgroundColor == defaultBackgroundColor));
        }

        /// <summary>
        /// The console logger should set the foreground color, if configured.
        /// </summary>
        [Fact(DisplayName = "Should_SetForegroundColor")]
        public void Should_SetForegroundColor()
        {
            // Setup
            var logger = new ConsoleTarget();
            logger.Configure(new TargetConfig
            {
                Properties = new Dictionary<string, object>
                {
                    { "foreground", "Red" }
                }
            });
            var layout = A.Fake<ILayout>();
            logger.SetLayout(layout);

            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Red foreground!");

            // Execute
            logger.Write(new LogEvent());

            // Validate
            Assert.True(this.textWriter.ConsoleMessages.Any(m => m.Message == "Red foreground!" && m.ForegroundColor == ConsoleColor.Red));
        }

        /// <summary>
        /// The default foreground color should be used, when no foreground color is configured.
        /// </summary>
        [Fact(DisplayName = "Should_UseDefaultForeground")]
        public void Should_UseDefaultForeground()
        {
            // Setup
            var logger = new ConsoleTarget();
            var layout = A.Fake<ILayout>();
            logger.SetLayout(layout);

            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Default foreground!");

            // Execute
            logger.Write(new LogEvent());

            // Validate
            Assert.True(this.textWriter.ConsoleMessages.Any(m => m.Message == "Default foreground!" && m.ForegroundColor == defaultForegroundColor));
        }

        /// <summary>
        /// The console colors should be reset, after writing.
        /// </summary>
        [Fact(DisplayName = "Should_ResetColorAfterWrite")]
        public void Should_ResetColorAfterWrite()
        {
            // Setup
            var logger = new ConsoleTarget();
            logger.Configure(new TargetConfig
            {
                Properties = new Dictionary<string, object>
                {
                    { "foreground", "White" },
                    { "background", "DarkBlue" }
                }
            });
            var layout = A.Fake<ILayout>();
            logger.SetLayout(layout);

            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("White and Blue!");

            // Execute
            logger.Write(new LogEvent());

            // Validate
            Assert.Equal(defaultBackgroundColor, Console.BackgroundColor);
            Assert.Equal(defaultForegroundColor, Console.ForegroundColor);
        }

        public void Dispose()
        {
            Console.SetOut(this.consoleTextWriter);
        }
    }

    /// <summary>
    /// Stores a console message, including the text, and the colors at the time of writing.
    /// </summary>
    internal class ConsoleMessage
    {
        /// <summary>
        /// The background color at the time the message was written.
        /// </summary>
        public ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// The foreground color at the time the message was written.
        /// </summary>
        public ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// The message that was written.
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// A dummy text writer, which stores the messages written to it, and records the console's
    /// foreground and background colors at the time of writing.
    /// </summary>
    internal class DummyTextWriter : TextWriter
    {
        public List<ConsoleMessage> ConsoleMessages { get; private set; }

        public DummyTextWriter()
        {
            ConsoleMessages = new List<ConsoleMessage>();
        }

        public override Encoding Encoding
        {
            get
            {
                return Encoding.Default;
            }
        }

        public override void Write(string value)
        {
            var message = new ConsoleMessage
            {
                BackgroundColor = Console.BackgroundColor,
                ForegroundColor = Console.ForegroundColor,
                Message = value
            };
            ConsoleMessages.Add(message);
        }
    }
}