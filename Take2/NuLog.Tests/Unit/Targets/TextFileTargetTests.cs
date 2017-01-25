/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.Configuration;
using NuLog.LogEvents;
using NuLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace NuLog.Tests.Unit.Targets
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the text file target.
    /// </summary>
    [Trait("Category", "Unit")]
    public class TextFileTargetTests : IDisposable
    {
        private readonly List<string> testFiles;

        public TextFileTargetTests()
        {
            this.testFiles = new List<string>();
        }

        /// <summary>
        /// The text file target should write the message to the file.
        /// </summary>
        [Fact(DisplayName = "Should_WriteText")]
        public void Should_WriteText()
        {
            // Setup
            var target = new TextFileTarget();
            var layout = A.Fake<ILayout>();
            target.Configure(GetTargetConfig("Should_WriteText.txt"));
            target.SetLayout(layout);
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Should write text!");

            // Execute
            target.Write(new LogEvent { Message = "Should write text!" });

            // Verify
            var text = File.ReadAllText("Should_WriteText.txt");
            Assert.Equal("Should write text!", text);
        }

        /// <summary>
        /// The text file target should use the layout.
        /// </summary>
        [Fact(DisplayName = "Should_UseLayout")]
        public void Should_UseLayout()
        {
            // Setup
            var target = new TextFileTarget();
            var layout = A.Fake<ILayout>();
            target.Configure(GetTargetConfig("Should_UseLayout.txt"));
            target.SetLayout(layout);
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Should use layout!");

            // Execute
            target.Write(new LogEvent { Message = "Hello, world!" });

            // Verify
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored)).MustHaveHappened();
            var text = File.ReadAllText("Should_UseLayout.txt");
            Assert.Equal("Should use layout!", text);
        }

        private TargetConfig GetTargetConfig(string filePath)
        {
            testFiles.Add(filePath);

            return new TargetConfig
            {
                Properties = new Dictionary<string, object>
                {
                    { "path", filePath }
                }
            };
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