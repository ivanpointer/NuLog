/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.FallbackLoggers;
using NuLog.LogEvents;
using System;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace NuLog.Tests.Unit.FallbackLoggers
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the standard trace fallback logger.
    /// </summary>
    [Trait("Category", "Unit")]
    public class StandardTraceFallbackLoggerTests : IDisposable
    {
        private HashSetTraceListener traceListener;

        public StandardTraceFallbackLoggerTests()
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
        /// The standard trace fallback logger should write to trace.
        /// </summary>
        [Fact(DisplayName = "Should_WriteToTrace")]
        public void Should_WriteToTrace()
        {
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
    }
}