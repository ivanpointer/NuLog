﻿/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.FallbackLoggers;
using NuLog.LogEvents;
using System.IO;
using Xunit;

namespace NuLog.Tests.Unit.FallbackLoggers
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the standard file fallback logger.
    /// </summary>
    [Trait("Category", "Unit")]
    public class StandardFileFallbackLoggerTests
    {
        /// <summary>
        /// The standard file fallback logger should write to a file.
        /// </summary>
        [Fact(DisplayName = "Should_WriteToFile")]
        public void Should_WriteToFile()
        {
            try
            {
                // Setup
                var fallbackLogger = new StandardFileFallbackLogger("Should_WriteToFile.txt");
                var target = A.Fake<ITarget>();

                // Execute
                fallbackLogger.Log(null, target, new LogEvent { Message = "Hello, StandardFileFallbackLogger!" });
                fallbackLogger.Log(null, target, new LogEvent { Message = "Hello, StandardFileFallbackLogger Line Two!" });

                // Verify
                var text = File.ReadAllText("Should_WriteToFile.txt");
                Assert.Contains("Hello, StandardFileFallbackLogger!", text);
                Assert.Contains("Hello, StandardFileFallbackLogger Line Two!", text);
            }
            finally
            {
                if (File.Exists("Should_WriteToFile.txt"))
                {
                    File.Delete("Should_WriteToFile.txt");
                }
            }
        }
    }
}