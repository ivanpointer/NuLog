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
using Xunit;

namespace NuLog.Tests.Unit.Targets
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the console target.
    /// </summary>
    [Collection("ColorConsoleTargetTests")]
    [Trait("Category", "Unit")]
    public class ColorConsoleTargetTests : IDisposable
    {
        protected readonly DummyTextWriter textWriter;

        private readonly TextWriter consoleTextWriter;

        private readonly ConsoleColor defaultBackgroundColor;

        private readonly ConsoleColor defaultForegroundColor;

        public ColorConsoleTargetTests()
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
            var layout = A.Fake<ILayout>();
            var layoutFactory = A.Fake<ILayoutFactory>();
            A.CallTo(() => layoutFactory.MakeLayout(A<string>.Ignored))
                .Returns(layout);
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Write to console!");

            var logger = new ColorConsoleTarget();
            logger.Configure(null, layoutFactory);

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
            var logger = new ColorConsoleTarget();

            var layout = A.Fake<ILayout>();
            var layoutFactory = A.Fake<ILayoutFactory>();
            A.CallTo(() => layoutFactory.MakeLayout(A<string>.Ignored))
                .Returns(layout);
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Write to console using layout!");

            logger.Configure(null, layoutFactory);

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
        /// The default background color should be used, when not explicitly set.
        /// </summary>
        [Fact(DisplayName = "Should_UseDefaultBackgroud")]
        public void Should_UseDefaultBackgroud()
        {
            // Setup
            var layout = A.Fake<ILayout>();
            var layoutFactory = A.Fake<ILayoutFactory>();
            A.CallTo(() => layoutFactory.MakeLayout(A<string>.Ignored))
                .Returns(layout);
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Default background!");

            var logger = new ColorConsoleTarget();
            logger.Configure(null, layoutFactory);

            // Execute
            logger.Write(new LogEvent());

            // Validate
            Assert.True(this.textWriter.ConsoleMessages.Any(m => m.Message == "Default background!" && m.BackgroundColor == defaultBackgroundColor));
        }

        /// <summary>
        /// The default foreground color should be used, when no foreground color is configured.
        /// </summary>
        [Fact(DisplayName = "Should_UseDefaultForeground")]
        public void Should_UseDefaultForeground()
        {
            // Setup
            var layout = A.Fake<ILayout>();
            var layoutFactory = A.Fake<ILayoutFactory>();
            A.CallTo(() => layoutFactory.MakeLayout(A<string>.Ignored))
                .Returns(layout);
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Default foreground!");

            var logger = new ColorConsoleTarget();
            logger.Configure(null, layoutFactory);

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
            var layout = A.Fake<ILayout>();
            var layoutFactory = A.Fake<ILayoutFactory>();
            A.CallTo(() => layoutFactory.MakeLayout(A<string>.Ignored))
                .Returns(layout);
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("White and Blue!");

            var config = new TargetConfig
            {
                Properties = new Dictionary<string, object>
                {
                    { "foreground", "White" },
                    { "background", "DarkBlue" }
                }
            };

            var logger = new ColorConsoleTarget();
            logger.Configure(config);
            logger.Configure(config, layoutFactory);

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
}