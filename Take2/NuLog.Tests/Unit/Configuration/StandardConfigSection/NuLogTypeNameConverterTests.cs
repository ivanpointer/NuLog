/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration.StandardConfiguration;
using NuLog.Targets;
using System;
using Xunit;

namespace NuLog.Tests.Unit.Configuration.StandardConfigSection
{
    /// <summary>
    /// Documents (and validates) the expected behavior of the NuLog type name converter.
    /// </summary>
    [Trait("Category", "Unit")]
    public class NuLogTypeNameConverterTests
    {
        /// <summary>
        /// Should get the type, given a string.
        /// </summary>
        [Fact(DisplayName = "Should_ConvertFromString")]
        public void Should_ConvertFromString()
        {
            // Setup
            var converter = new NuLogTypeNameConverter();

            // Execute
            var type = converter.ConvertFrom("System.DateTime");

            // Verify
            Assert.Equal(typeof(DateTime), type);
        }

        /// <summary>
        /// Should get the type, given a string.
        /// </summary>
        [Fact(DisplayName = "Should_ConvertFromString_WithoutAssembly")]
        public void Should_ConvertFromString_WithoutAssembly()
        {
            // Setup
            var converter = new NuLogTypeNameConverter();

            // Execute
            var type = converter.ConvertFrom("NuLog.Targets.StreamTarget");

            // Verify
            Assert.Equal(typeof(StreamTarget), type);
        }

        /// <summary>
        /// Should convert a type to a string, with the assembly.
        /// </summary>
        [Fact(DisplayName = "Should_ConvertToString")]
        public void Should_ConvertToString()
        {
            // Setup
            var converter = new NuLogTypeNameConverter();

            // Execute
            var type = converter.ConvertTo(typeof(StreamTarget), typeof(string));

            // Verify
            Assert.Equal("NuLog.Targets.StreamTarget, NuLog.Targets", type);
        }
    }
}