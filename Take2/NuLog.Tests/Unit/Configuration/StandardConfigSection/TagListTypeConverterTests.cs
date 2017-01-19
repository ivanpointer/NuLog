/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration.StandardConfiguration;
using Xunit;

namespace NuLog.Tests.Unit.Configuration.StandardConfigSection
{
    /// <summary>
    /// Defines (and validates) the expected behavior of the tag list type converter.
    /// </summary>
    [Trait("Category", "Unit")]
    public class TagListTypeConverterTests
    {
        /// <summary>
        /// The tag list type converter should convert a list of tags to a single string.
        /// </summary>
        [Fact(DisplayName = "Should_ConvertToString")]
        public void Should_ConvertToString()
        {
            // Setup
            var converter = new TagListTypeConverter();

            // Execute
            var converted = converter.ConvertTo(new string[] { "one_tag", "two_tag", "red_tag", "blue_tag" }, typeof(string));

            // Verify
            Assert.Equal("one_tag,two_tag,red_tag,blue_tag", converted);
        }

        /// <summary>
        /// The tag list type converter should convert a string into a list of tags.
        /// </summary>
        [Fact(DisplayName = "Should_ConvertFromStringArray")]
        public void Should_ConvertFromStringArray()
        {
            // Setup
            var converter = new TagListTypeConverter();

            // Execute
            var converted = (string[])converter.ConvertFrom("one_tag,two_tag,red_tag,blue_tag");

            // Verify
            Assert.Equal(4, converted.Length);
            Assert.Contains("one_tag", converted);
            Assert.Contains("two_tag", converted);
            Assert.Contains("red_tag", converted);
            Assert.Contains("blue_tag", converted);
        }
    }
}