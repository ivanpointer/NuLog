/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.Activators.Layouts;
using NuLog.Layouts;
using Xunit;

namespace NuLog.Tests.Unit.Activators
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the various standard activators.
    /// </summary>
    [Trait("Category", "Unit")]
    public class StandardActivatorTests
    {
        /// <summary>
        /// Should build a new instance of a layout parser.
        /// </summary>
        [Fact(DisplayName = "Should_BuildStandardLayoutParser")]
        public void Should_BuildStandardLayoutParser()
        {
            // Setup
            var activator = new StandardLayoutParserActivator();

            // Execute
            var parser = activator.BuildNew();

            // Verify
            Assert.NotNull(parser);
        }

        /// <summary>
        /// Should build a new instance of a property parser.
        /// </summary>
        [Fact(DisplayName = "Should_BuildStandardPropertyParser")]
        public void Should_BuildStandardPropertyParser()
        {
            // Setup
            var activator = new StandardPropertyParserActivator();

            // Execute
            var parser = activator.BuildNew();

            // Verify
            Assert.NotNull(parser);
        }

        /// <summary>
        /// Should build a new instance of a layout.
        /// </summary>
        [Fact(DisplayName = "Should_BuildStandardLayout")]
        public void Should_BuildStandardLayout()
        {
            // Setup
            var propertyParser = A.Fake<IPropertyParser>();
            var activator = new StandardLayoutActivator(propertyParser);

            // Execute
            var layout = activator.BuildNew(null);

            // Verify
            Assert.NotNull(layout);
        }
    }
}