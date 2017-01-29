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
    /// Documents (and verifies) the expected behavior of the trace target.
    /// </summary>
    [Trait("Category", "Unit")]
    public class TraceTargetTests : IDisposable
    {
        private HashSetTraceListener traceListener;

        public TraceTargetTests()
        {
            this.traceListener = new HashSetTraceListener();
            Trace.Listeners.Add(this.traceListener);
        }

        public void Dispose()
        {
            Trace.Listeners.Remove(this.traceListener);
            this.traceListener = null;
        }

        /// <summary>
        /// The trace target should write to trace.
        /// </summary>
        [Fact(DisplayName = "Should_WriteToTrace")]
        public void Should_WriteToTrace()
        {
            // Setup
            var logEvent = new LogEvent
            {
                Message = "Should_WriteToTrace - hello, world!"
            };
            var layout = A.Fake<ILayout>();
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored)).Returns(logEvent.Message);

            var target = new TraceTarget();
            target.SetLayout(layout);

            // Execute
            target.Write(logEvent);

            // Verify
            Assert.Contains("Should_WriteToTrace - hello, world!", this.traceListener.Messages);
        }

        /// <summary>
        /// The trace target should use the configured layout to format its messages.
        /// </summary>
        [Fact(DisplayName = "Should_UseLayout")]
        public void Should_UseLayout()
        {
            // Setup
            var layout = A.Fake<ILayout>();
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored)).Returns("Should_UseLayout - formatted");
            var target = new TraceTarget();
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
        /// The trace target should throw an invalid operation exception when no layout is given, and
        /// the target is asked to write a log message.
        /// </summary>
        [Fact(DisplayName = "Should_ThrowInvalidOperationWithoutLayout")]
        public void Should_ThrowInvalidOperationWithoutLayout()
        {
            // Setup
            var target = new TraceTarget();

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