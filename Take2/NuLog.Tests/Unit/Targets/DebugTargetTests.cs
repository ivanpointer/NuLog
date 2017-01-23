/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.LogEvents;
using NuLog.Targets;
using System;
using System.Diagnostics;
using Xunit;

namespace NuLog.Tests.Unit.Targets
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the debug target.
    /// </summary>
    [Trait("Category", "Unit")]
    public class DebugTargetTests : IDisposable
    {
        private HashSetTraceListener traceListener;

        public DebugTargetTests()
        {
            this.traceListener = new HashSetTraceListener();
            Debug.Listeners.Add(this.traceListener);
        }

        public void Dispose()
        {
            Debug.Listeners.Remove(this.traceListener);
            this.traceListener = null;
        }

        /// <summary>
        /// The debug target should write to debug.
        /// </summary>
        [Fact(DisplayName = "Should_WriteToDebug")]
        public void Should_WriteToDebug()
        {
            // Setup
            var logEvent = new LogEvent
            {
                Message = "Should_WriteToDebug - hello, world!"
            };
            var layout = A.Fake<ILayout>();
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored)).Returns(logEvent.Message);

            var target = new DebugTarget();
            target.SetLayout(layout);

            // Execute
            target.Write(logEvent);

            // Verify
            Assert.Contains("Should_WriteToDebug - hello, world!", this.traceListener.Messages);
        }

        /// <summary>
        /// The debug target should use the configured layout to format its messages.
        /// </summary>
        [Fact(DisplayName = "Should_UseLayout")]
        public void Should_UseLayout()
        {
            // Setup
            var layout = A.Fake<ILayout>();
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored)).Returns("Should_UseLayout - formatted");
            var target = new DebugTarget();
            target.SetLayout(layout);

            // Execute
            target.Write(new LogEvent
            {
                Message = "hello, world!"
            });

            // Verify
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored)).MustHaveHappened();
            Assert.Contains("Should_UseLayout - formatted", this.traceListener.Messages);
        }

        /// <summary>
        /// The debug target should throw an invalid operation exception when no layout is given, and
        /// the target is asked to write a log message.
        /// </summary>
        [Fact(DisplayName = "Should_UseLayout")]
        public void Should_ThrowInvalidOperationWithoutLayout()
        {
            // Setup
            var target = new DebugTarget();

            // Execute / Verify
            Assert.Throws(typeof(InvalidOperationException), () =>
            {
                target.Write(new LogEvent
                {
                    Message = "hello, world!"
                });
            });
        }
    }
}