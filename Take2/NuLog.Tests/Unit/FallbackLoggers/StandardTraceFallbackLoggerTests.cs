/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.FallbackLoggers;
using NuLog.LogEvents;
using System;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace NuLog.Tests.Unit.FallbackLoggers {

    /// <summary>
    /// Documents (and verifies) the expected behavior of the standard trace fallback logger.
    /// </summary>
    [Trait("Category", "Unit")]
    public class StandardTraceFallbackLoggerTests : IDisposable {
        private HashSetTraceListener traceListener;

        public StandardTraceFallbackLoggerTests() {
            this.traceListener = new HashSetTraceListener();
            Trace.Listeners.Add(this.traceListener);
        }

        public void Dispose() {
            Trace.Listeners.Remove(this.traceListener);
            this.traceListener = null;
        }

        /// <summary>
        /// The standard trace fallback logger should write to trace.
        /// </summary>
        [Fact(DisplayName = "Should_WriteToTrace")]
        public void Should_WriteToTrace() {
            // Setup
            var fallbackLogger = new StandardTraceFallbackLogger();
            var target = A.Fake<ITarget>();

            // Execute
            fallbackLogger.Log(null, target, new LogEvent { Message = "Hello, StandardTraceFallbackLogger!" });
            fallbackLogger.Log(null, target, new LogEvent { Message = "Hello, StandardTraceFallbackLogger Line Two!" });

            // Verify
            Assert.True(this.traceListener.Messages.Any(m => m.Contains("Hello, StandardTraceFallbackLogger!")));
            Assert.True(this.traceListener.Messages.Any(m => m.Contains("Hello, StandardTraceFallbackLogger Line Two!")));
        }

        /// <summary>
        /// A simple message should be written to trace.
        /// </summary>
        [Fact(DisplayName = "Should_WriteSimpleMessageToTrace")]
        public void Should_WriteSimpleMessageToTrace() {
            // Setup
            var fallbackLogger = new StandardTraceFallbackLogger();
            var target = A.Fake<ITarget>();

            // Execute
            fallbackLogger.Log("Hello, Simple Message!");

            // Verify
            Assert.True(this.traceListener.Messages.Any(m => m.Contains("Hello, Simple Message!")));
        }

        /// <summary>
        /// A formatted message should be written to trace.
        /// </summary>
        [Fact(DisplayName = "Should_WriteFormattedMessageToTrace")]
        public void Should_WriteFormattedMessageToTrace() {
            // Setup
            var fallbackLogger = new StandardTraceFallbackLogger();
            var target = A.Fake<ITarget>();

            // Execute
            fallbackLogger.Log("Hello, Formatted {0}!", "Message");

            // Verify
            Assert.True(this.traceListener.Messages.Any(m => m.Contains("Hello, Formatted Message!")));
        }
    }
}