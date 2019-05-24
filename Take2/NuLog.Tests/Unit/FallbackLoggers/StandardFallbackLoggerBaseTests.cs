/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.FallbackLoggers;
using NuLog.LogEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace NuLog.Tests.Unit.FallbackLoggers {

    /// <summary>
    /// Documents (and verifies) the expected behavior of the standard fallback logger base class (abstract).
    /// </summary>
    [Trait("Category", "Unit")]
    public class StandardFallbackLoggerBaseTests {

        /// <summary>
        /// The fallback logger should write the log event to file.
        /// </summary>
        [Fact(DisplayName = "Should_WriteLogEvent")]
        public void Should_WriteLogEvent() {
            // Setup
            var fallbackLogger = GetFallbackLogger();
            var target = A.Fake<ITarget>();

            // Execute
            fallbackLogger.Log(null, target, new LogEvent {
                Message = "Fallback Logger!"
            });

            // Verify
            var text = fallbackLogger.LoggedMessages.Single();
            Assert.Contains("Fallback Logger!", text);
        }

        /// <summary>
        /// The fallback logger should write multiple log events to file.
        /// </summary>
        [Fact(DisplayName = "Should_WriteMultipleEvents")]
        public void Should_WriteMultipleEvents() {
            // Setup
            var fallbackLogger = GetFallbackLogger();
            var target = A.Fake<ITarget>();

            // Execute
            fallbackLogger.Log(null, target, new LogEvent {
                Message = "Fallback Logger!"
            });
            fallbackLogger.Log(null, target, new LogEvent {
                Message = "Fallback Logger Two!"
            });

            // Verify
            var text = string.Join("\r\n", fallbackLogger.LoggedMessages);
            Assert.Contains("Fallback Logger!", text);
            Assert.Contains("Fallback Logger Two!", text);
        }

        /// <summary>
        /// The fallback logger should write the current date/time stamp at the start of the log message.
        /// </summary>
        [Fact(DisplayName = "Should_StartWithDateTimeStamp")]
        public void Should_StartWithDateTimeStamp() {
            // Setup
            var fallbackLogger = GetFallbackLogger();
            var target = A.Fake<ITarget>();

            // Execute
            fallbackLogger.Log(null, target, new LogEvent {
                Message = "Fallback Logger!"
            });

            // Verify
            var text = fallbackLogger.LoggedMessages.Single();
            var pattern = new Regex(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3} | .*$");
            var now = DateTime.Now;
            var midDateText = now.ToString("yyyy-MM-dd hh:mm");
            var maxDateText = now.AddSeconds(1).ToString("yyyy-MM-dd hh:mm");

            Assert.True(pattern.IsMatch(text), "Text loaded from file doesn't match regular expression for starting with a date time string.");
            Assert.True(text.StartsWith(midDateText) || text.StartsWith(maxDateText), "Text loaded from file doesn't appear to be prefixed with the current time.");
        }

        /// <summary>
        /// The fallback logger should include the target's name and type.
        /// </summary>
        [Fact(DisplayName = "Should_IncludeTargetNameAndType")]
        public void Should_IncludeTargetNameAndType() {
            // Setup
            var fallbackLogger = GetFallbackLogger();

            var target = A.Fake<ITarget>();
            target.Name = "fakeTarget";

            // Execute
            fallbackLogger.Log(null, target, new LogEvent {
                Message = "Target Type and Name!"
            });

            // Verify
            var text = fallbackLogger.LoggedMessages.Single();
            Assert.Contains("fakeTarget", text);
            Assert.Contains(target.GetType().FullName, text);
        }

        /// <summary>
        /// The fallback logger should include the exception in the output.
        /// </summary>
        [Fact(DisplayName = "Should_IncludeExceptionInformation")]
        public void Should_IncludeExceptionInformation() {
            // Setup
            var fallbackLogger = GetFallbackLogger();
            var target = A.Fake<ITarget>();

            var exception = new InvalidOperationException("A Fake Exception");

            // Execute
            fallbackLogger.Log(exception, target, new LogEvent {
                Message = "Exception Information!"
            });

            // Verify
            var text = fallbackLogger.LoggedMessages.Single();
            Assert.Contains(exception.ToString(), text);
        }

        /// <summary>
        /// The fallback logger should include the tags on the log event.
        /// </summary>
        [Fact(DisplayName = "Should_IncludeTags")]
        public void Should_IncludeTags() {
            // Setup
            var fallbackLogger = GetFallbackLogger();
            var target = A.Fake<ITarget>();

            // Execute
            fallbackLogger.Log(null, target, new LogEvent {
                Message = "Include Tags!",
                Tags = new string[] { "one_tag", "two_tag", "red_tag", "blue_tag" }
            });

            // Verify
            var text = fallbackLogger.LoggedMessages.Single();
            Assert.Contains("one_tag,two_tag,red_tag,blue_tag", text);
        }

        /// <summary>
        /// The fallback logger should include the log event exception message.
        /// </summary>
        [Fact(DisplayName = "Should_IncludeLogEventExceptionMessage")]
        public void Should_IncludeLogEventExceptionMessage() {
            // Setup
            var fallbackLogger = GetFallbackLogger();
            var target = A.Fake<ITarget>();

            // Execute
            fallbackLogger.Log(null, target, new LogEvent {
                Message = "Include Exception Message!",
                Exception = new Exception("Log Event Exception Message!")
            });

            // Verify
            var text = fallbackLogger.LoggedMessages.Single();
            Assert.Contains("LogEvent Exception: \"Log Event Exception Message!\"", text);
        }

        /// <summary>
        /// The fallback logger should write the current date/time stamp at the start of the log message.
        /// </summary>
        [Fact(DisplayName = "Should_SimpleMessageStartWithDateTimeStamp")]
        public void Should_SimpleMessageStartWithDateTimeStamp() {
            // Setup
            var fallbackLogger = GetFallbackLogger();
            var target = A.Fake<ITarget>();

            // Execute
            fallbackLogger.Log("Simple message!");

            // Verify
            var text = fallbackLogger.LoggedMessages.Single();
            var pattern = new Regex(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3} | .*$");
            var now = DateTime.Now;
            var midDateText = now.ToString("yyyy-MM-dd hh:mm");
            var maxDateText = now.AddSeconds(1).ToString("yyyy-MM-dd hh:mm");

            Assert.True(pattern.IsMatch(text), "Text loaded from file doesn't match regular expression for starting with a date time string.");
            Assert.True(text.StartsWith(midDateText) || text.StartsWith(maxDateText), "Text loaded from file doesn't appear to be prefixed with the current time.");
        }

        /// <summary>
        /// The fallback logger should write the current date/time stamp at the start of the log message.
        /// </summary>
        [Fact(DisplayName = "Should_FormattedMessageStartWithDateTimeStamp")]
        public void Should_FormattedMessageStartWithDateTimeStamp() {
            // Setup
            var fallbackLogger = GetFallbackLogger();
            var target = A.Fake<ITarget>();

            // Execute
            fallbackLogger.Log("Formatted {0}!", "message");

            // Verify
            var text = fallbackLogger.LoggedMessages.Single();
            var pattern = new Regex(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3} | .*$");
            var now = DateTime.Now;
            var midDateText = now.ToString("yyyy-MM-dd hh:mm");
            var maxDateText = now.AddSeconds(1).ToString("yyyy-MM-dd hh:mm");

            Assert.True(pattern.IsMatch(text), "Text loaded from file doesn't match regular expression for starting with a date time string.");
            Assert.True(text.StartsWith(midDateText) || text.StartsWith(maxDateText), "Text loaded from file doesn't appear to be prefixed with the current time.");
        }

        /// <summary>
        /// The fallback logger should format a simple message, with arguments.
        /// </summary>
        [Fact(DisplayName = "Should_FormatSimpleMessage")]
        public void Should_FormatSimpleMessage() {
            // Setup
            var fallbackLogger = GetFallbackLogger();
            var target = A.Fake<ITarget>();

            // Execute
            fallbackLogger.Log("Formatted {0}!", "message");

            // Verify
            var text = fallbackLogger.LoggedMessages.Single();
            Assert.Contains("Formatted message!", text);
        }

        /// <summary>
        /// Gets the fallback logger under test.
        /// </summary>
        internal DummyStandardFallbackLogger GetFallbackLogger() {
            return new DummyStandardFallbackLogger();
        }
    }

    /// <summary>
    /// A dummy class for testing the functionality of the standard fallback logger base class.
    /// </summary>
    internal class DummyStandardFallbackLogger : StandardFallbackLoggerBase {
        public List<string> LoggedMessages { get; private set; }

        public DummyStandardFallbackLogger() {
            LoggedMessages = new List<string>();
        }

        public override void Log(Exception exception, ITarget target, ILogEvent logEvent) {
            var formatted = FormatMessage(exception, target, logEvent);
            LoggedMessages.Add(formatted);
        }

        public override void Log(string message, params object[] args) {
            var formatted = FormatMessage(message, args);
            LoggedMessages.Add(formatted);
        }
    }
}