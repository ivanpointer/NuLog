/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.LogEvents;
using NuLog.Targets;
using System.IO;
using Xunit;

namespace NuLog.Tests.Unit.Targets
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of a stream target.
    /// </summary>
    [Trait("Category", "Unit")]
    public class StreamTargetTests
    {
        /// <summary>
        /// Should leverage the layout to format a given log event.
        /// </summary>
        [Fact(DisplayName = "Should_CallLayout")]
        public void Should_CallLayout()
        {
            // Setup
            var layout = A.Fake<ILayout>();
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored)).Returns("Hello, World!");

            var ms = new MemoryStream();
            var target = GetStreamTarget(layout, ms);

            // Execute
            target.Write(new LogEvent());

            // Verify
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored)).MustHaveHappened();
        }

        /// <summary>
        /// A stream target should write to a stream.
        /// </summary>
        [Fact(DisplayName = "Should_WriteToStream")]
        public void Should_WriteToStream()
        {
            // Setup
            var layout = A.Fake<ILayout>();
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Hello, World!");

            var ms = new MemoryStream();
            var target = GetStreamTarget(layout, ms);

            // Execute
            target.Write(new LogEvent());

            // Verify
            ms.Position = 0;
            var sr = new StreamReader(ms);
            var wrote = sr.ReadToEnd();
            Assert.Equal("Hello, World!", wrote);
        }

        /// <summary>
        /// The target should close the given stream, if requested, when the target is disposed.
        /// </summary>
        [Fact(DisplayName = "Should_CloseStreamOnDispose")]
        public void Should_CloseStreamOnDispose()
        {
            // Setup
            var layout = A.Fake<ILayout>();
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Hello, World!");

            var ms = new MemoryStream();

            // Execute
            using (var target = GetStreamTarget(layout, ms, true))
            {
                target.Write(new LogEvent());
            }

            // Verify - make sure that we can no longer write to the memory stream
            Assert.False(ms.CanWrite);
        }

        /// <summary>
        /// The target should leave the given stream open when being disposed, if not told to close
        /// the given stream.
        /// </summary>
        [Fact(DisplayName = "Should_LeaveStreamOpenOnDispose")]
        public void Should_LeaveStreamOpenOnDispose()
        {
            // Setup
            var layout = A.Fake<ILayout>();
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Hello, World!");

            var ms = new MemoryStream();

            // Execute
            using (var target = GetStreamTarget(layout, ms, false))
            {
                target.Write(new LogEvent());
            }

            // Verify - make sure that we can no longer write to the memory stream
            Assert.True(ms.CanWrite);
        }

        /// <summary>
        /// Get a new instance of the stream target under test.
        /// </summary>
        private ITarget GetStreamTarget(ILayout layout, Stream stream, bool closeOnDispose = false)
        {
            return new StreamTarget("StreamTargetUnderTest", layout, stream, closeOnDispose);
        }
    }
}