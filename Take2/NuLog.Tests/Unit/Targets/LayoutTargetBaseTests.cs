/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.LogEvents;
using NuLog.Targets;
using System;
using Xunit;

namespace NuLog.Tests.Unit.Targets
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the layout target base.
    /// </summary>
    [Trait("Category", "Unit")]
    public class LayoutTargetBaseTests
    {
        /// <summary>
        /// The layout target base use the given layout.
        /// </summary>
        [Fact(DisplayName = "Should_LoadGivenLayout")]
        public void Should_LoadGivenLayout()
        {
            // Setup
            var layout = A.Fake<ILayout>();
            var target = new DummyLayoutTarget();

            // Execute
            target.SetLayout(layout);

            // Verify
            Assert.Equal(layout, target.GetAssignedLayout());
        }
    }

    /// <summary>
    /// A dummy class for testing out the functionality of the layout target base.
    /// </summary>
    internal class DummyLayoutTarget : LayoutTargetBase
    {
        /// <summary>
        /// Expose the protected ILayout for this layout target.
        /// </summary>
        public ILayout GetAssignedLayout()
        {
            return Layout;
        }

        public override void Write(LogEvent logEvent)
        {
            throw new NotImplementedException();
        }
    }
}