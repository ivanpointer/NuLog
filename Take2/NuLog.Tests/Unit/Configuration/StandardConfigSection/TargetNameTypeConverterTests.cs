/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration.StandardConfiguration;
using Xunit;

namespace NuLog.Tests.Unit.Configuration.StandardConfigSection
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the target name type converter.
    /// </summary>
    [Trait("Category", "Unit")]
    public class TargetNameTypeConverterTests
    {
        /// <summary>
        /// The tag list type converter should convert a list of target names to a single string.
        /// </summary>
        [Fact(DisplayName = "Should_ConvertToString")]
        public void Should_ConvertToString()
        {
            // Setup
            var converter = new TargetNameTypeConverter();

            // Execute
            var converted = converter.ConvertTo(new string[] { "one_target", "two_target", "red_target", "blue_target" }, typeof(string));

            // Verify
            Assert.Equal("one_target,two_target,red_target,blue_target", converted);
        }

        /// <summary>
        /// The tag list type converter should convert a string into a list of target names.
        /// </summary>
        [Fact(DisplayName = "Should_ConvertFromStringArray")]
        public void Should_ConvertFromStringArray()
        {
            // Setup
            var converter = new TargetNameTypeConverter();

            // Execute
            var converted = (string[])converter.ConvertFrom("one_target,two_target,red_target,blue_target");

            // Verify
            Assert.Equal(4, converted.Length);
            Assert.Contains("one_target", converted);
            Assert.Contains("two_target", converted);
            Assert.Contains("red_target", converted);
            Assert.Contains("blue_target", converted);
        }
    }
}