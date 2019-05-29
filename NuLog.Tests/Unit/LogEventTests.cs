/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.LogEvents;
using Xunit;

namespace NuLog.Tests.Unit {

    [Trait("Category", "Unit")]
    public class LogEventTests {

        [Fact(DisplayName = "Should_Dispose")]
        public void Should_Dispose() {
            // A log event shouldn't blow up on dispose
            var logEvent = new LogEvent();
            logEvent.Dispose();
        }
    }
}