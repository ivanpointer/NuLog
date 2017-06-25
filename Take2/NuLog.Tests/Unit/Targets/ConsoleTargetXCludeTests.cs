/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.Configuration;
using NuLog.LogEvents;
using NuLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NuLog.Tests.Unit.Targets
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the console target.
    ///
    /// These tests are excluded from the AppVeyor build - as something about them fails in AppVeyor
    /// - it seems that setting the console color doesn't work there.
    /// </summary>
    [Collection("ConsoleTargetTests")]
    [Trait("Category", "Unit-Exclude")]
    public class ConsoleTargetXCludeTests : ConsoleTargetTests
    {
        /// <summary>
        /// The console logger should set the background color, if configured.
        /// </summary>
        [Fact(DisplayName = "Should_SetBackgroundColor")]
        public void Should_SetBackgroundColor()
        {
            // Setup
            var layout = A.Fake<ILayout>();
            var layoutFactory = A.Fake<ILayoutFactory>();
            A.CallTo(() => layoutFactory.MakeLayout(A<string>.Ignored))
                .Returns(layout);
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Green background!");

            var config = new TargetConfig
            {
                Properties = new Dictionary<string, object>
                {
                    { "background", "DarkGreen" }
                }
            };

            var logger = new ConsoleTarget();
            logger.Configure(config);
            logger.Configure(config, layoutFactory);

            // Execute
            logger.Write(new LogEvent());

            // Validate
            var message = this.textWriter.ConsoleMessages.Single(m => m.Message == "Green background!");
            Assert.Equal(ConsoleColor.DarkGreen, message.BackgroundColor);
        }

        /// <summary>
        /// The console logger should set the foreground color, if configured.
        /// </summary>
        [Fact(DisplayName = "Should_SetForegroundColor")]
        public void Should_SetForegroundColor()
        {
            // Setup
            var layout = A.Fake<ILayout>();
            var layoutFactory = A.Fake<ILayoutFactory>();
            A.CallTo(() => layoutFactory.MakeLayout(A<string>.Ignored))
                .Returns(layout);
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Red foreground!");

            var config = new TargetConfig
            {
                Properties = new Dictionary<string, object>
                {
                    { "foreground", "Red" }
                }
            };

            var logger = new ConsoleTarget();
            logger.Configure(config);
            logger.Configure(config, layoutFactory);

            // Execute
            logger.Write(new LogEvent());

            // Validate
            var message = this.textWriter.ConsoleMessages.Single(m => m.Message == "Red foreground!");
            Assert.Equal(ConsoleColor.Red, message.ForegroundColor);
        }
    }
}