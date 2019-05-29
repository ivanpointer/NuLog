/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.LogEvents;
using NuLog.Targets;
using System;
using Xunit;

namespace NuLog.Tests.Unit.Targets {

    [Collection("ColorConsoleTargetTests")]
    [Trait("Category", "Unit")]
    public class ConsoleTargetTests {
        protected readonly DummyTextWriter textWriter;

        public ConsoleTargetTests() {
            this.textWriter = new DummyTextWriter();
            Console.SetOut(this.textWriter);
        }

        /// <summary>
        /// The target should write to the console.
        /// </summary>
        [Fact(DisplayName = "Should_WriteToConsole")]
        public void Should_WriteToConsole() {
            // Setup
            var layout = A.Fake<ILayout>();
            var layoutFactory = A.Fake<ILayoutFactory>();
            A.CallTo(() => layoutFactory.MakeLayout(A<string>.Ignored))
                .Returns(layout);
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Write to console!");

            var logger = new ConsoleTarget();
            logger.Configure(null, layoutFactory);

            // Execute
            logger.Write(new LogEvent {
                Message = "Write to console!"
            });

            // Validate
            Assert.Contains(this.textWriter.ConsoleMessages, m => m.Message == "Write to console!");
        }

        /// <summary>
        /// The target should use the layout when writing to console.
        /// </summary>
        [Fact(DisplayName = "Should_UseLayout")]
        public void Should_UseLayout() {
            // Setup
            var logger = new ConsoleTarget();

            var layout = A.Fake<ILayout>();
            var layoutFactory = A.Fake<ILayoutFactory>();
            A.CallTo(() => layoutFactory.MakeLayout(A<string>.Ignored))
                .Returns(layout);
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Write to console using layout!");

            logger.Configure(null, layoutFactory);

            // Execute
            logger.Write(new LogEvent {
                Message = "Write to console!"
            });

            // Validate
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored)).MustHaveHappened();
            Assert.Contains(this.textWriter.ConsoleMessages, m => m.Message == "Write to console using layout!");
        }
    }
}