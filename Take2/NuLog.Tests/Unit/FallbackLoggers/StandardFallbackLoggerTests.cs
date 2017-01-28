/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.Dispatchers;
using NuLog.FallbackLoggers;
using NuLog.LogEvents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace NuLog.Tests.Unit.FallbackLoggers
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the standard fallback logger.
    /// </summary>
    [Trait("Category", "Unit")]
    public class StandardFallbackLoggerTests : IDisposable
    {
        private readonly List<string> testFiles;

        public StandardFallbackLoggerTests()
        {
            this.testFiles = new List<string>();
        }

        /// <summary>
        /// The fallback logger should write the log event to file.
        /// </summary>
        [Fact(DisplayName = "Should_WriteLogEvent")]
        public void Should_WriteLogEvent()
        {
            // Setup
            var fallbackLogger = GetFallbackLogger("Should_WriteLogEvent.txt");
            var target = A.Fake<ITarget>();

            // Execute
            fallbackLogger.Log(null, target, new LogEvent
            {
                Message = "Fallback Logger!"
            });

            // Verify
            var text = File.ReadAllText("Should_WriteLogEvent.txt");
            Assert.Contains("Fallback Logger!", text);
        }

        /// <summary>
        /// The fallback logger should write multiple log events to file.
        /// </summary>
        [Fact(DisplayName = "Should_WriteMultipleEvents")]
        public void Should_WriteMultipleEvents()
        {
            // Setup
            var fallbackLogger = GetFallbackLogger("Should_WriteMultipleEvents.txt");
            var target = A.Fake<ITarget>();

            // Execute
            fallbackLogger.Log(null, target, new LogEvent
            {
                Message = "Fallback Logger!"
            });
            fallbackLogger.Log(null, target, new LogEvent
            {
                Message = "Fallback Logger Two!"
            });

            // Verify
            var text = File.ReadAllText("Should_WriteMultipleEvents.txt");
            Assert.Contains("Fallback Logger!", text);
            Assert.Contains("Fallback Logger Two!", text);
        }

        /// <summary>
        /// The fallback logger should write the current date/time stamp at the start of the log message.
        /// </summary>
        [Fact(DisplayName = "Should_StartWithDateTimeStamp")]
        public void Should_StartWithDateTimeStamp()
        {
            // Setup
            var fallbackLogger = GetFallbackLogger("Should_StartWithDateTimeStamp.txt");
            var target = A.Fake<ITarget>();

            // Execute
            fallbackLogger.Log(null, target, new LogEvent
            {
                Message = "Fallback Logger!"
            });

            // Verify
            var text = File.ReadAllText("Should_StartWithDateTimeStamp.txt");
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
        public void Should_IncludeTargetNameAndType()
        {
            // Setup
            var fallbackLogger = GetFallbackLogger("Should_IncludeTargetNameAndType.txt");

            var target = A.Fake<ITarget>();
            target.Name = "fakeTarget";

            // Execute
            fallbackLogger.Log(null, target, new LogEvent
            {
                Message = "Target Type and Name!"
            });

            // Verify
            var text = File.ReadAllText("Should_IncludeTargetNameAndType.txt");
            Assert.Contains("fakeTarget", text);
            Assert.Contains(target.GetType().FullName, text);
        }

        /// <summary>
        /// The fallback logger should include the exception in the output.
        /// </summary>
        [Fact(DisplayName = "Should_IncludeExceptionInformation")]
        public void Should_IncludeExceptionInformation()
        {
            // Setup
            var fallbackLogger = GetFallbackLogger("Should_IncludeExceptionInformation.txt");
            var target = A.Fake<ITarget>();

            var exception = new InvalidOperationException("A Fake Exception");

            // Execute
            fallbackLogger.Log(exception, target, new LogEvent
            {
                Message = "Exception Information!"
            });

            // Verify
            var text = File.ReadAllText("Should_IncludeExceptionInformation.txt");
            Assert.Contains(exception.ToString(), text);
        }

        /// <summary>
        /// The fallback logger should include the tags on the log event.
        /// </summary>
        [Fact(DisplayName = "Should_IncludeTags")]
        public void Should_IncludeTags()
        {
            // Setup
            var fallbackLogger = GetFallbackLogger("Should_IncludeTags.txt");
            var target = A.Fake<ITarget>();

            // Execute
            fallbackLogger.Log(null, target, new LogEvent
            {
                Message = "Include Tags!",
                Tags = new string[] { "one_tag", "two_tag", "red_tag", "blue_tag" }
            });

            // Verify
            var text = File.ReadAllText("Should_IncludeTags.txt");
            Assert.Contains("one_tag,two_tag,red_tag,blue_tag", text);
        }

        /// <summary>
        /// The fallback logger should include the log event exception message.
        /// </summary>
        [Fact(DisplayName = "Should_IncludeLogEventExceptionMessage")]
        public void Should_IncludeLogEventExceptionMessage()
        {
            // Setup
            var fallbackLogger = GetFallbackLogger("Should_IncludeLogEventExceptionMessage.txt");
            var target = A.Fake<ITarget>();

            // Execute
            fallbackLogger.Log(null, target, new LogEvent
            {
                Message = "Include Exception Message!",
                Exception = new Exception("Log Event Exception Message!")
            });

            // Verify
            var text = File.ReadAllText("Should_IncludeLogEventExceptionMessage.txt");
            Assert.Contains("LogEvent Exception: \"Log Event Exception Message!\"", text);
        }

        /// <summary>
        /// Gets the fallback logger under test.
        /// </summary>
        protected IFallbackLogger GetFallbackLogger(string filePath)
        {
            this.testFiles.Add(filePath);
            return new StandardFallbackLogger(filePath);
        }

        public void Dispose()
        {
            foreach (var file in testFiles)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }
    }
}