/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.Linq;
using Xunit;

namespace NuLog.Tests.Unit {

    /// <summary>
    /// Documents the expected behavior of a tag normalizer.
    /// </summary>
    [Trait("Category", "Unit")]
    public class TagNormalizerTests {
        private readonly ITagNormalizer tagNormalizer;

        public TagNormalizerTests() {
            this.tagNormalizer = new StandardTagNormalizer();
        }

        /// <summary>
        /// The normalizer should trim white space.
        /// </summary>
        [Theory(DisplayName = "Should_TrimWhiteSpace")]
        [InlineData(" hello", "hello")]
        [InlineData("\thello", "hello")]
        [InlineData("\nhello", "hello")]
        [InlineData("world ", "world")]
        [InlineData("world\t", "world")]
        [InlineData("world\n", "world")]
        [InlineData(" \n  hello world  \t ", "hello world")]
        public void Should_TrimWhiteSpace(string tag, string expected) {
            // Execute
            var normalized = tagNormalizer.NormalizeTag(tag);

            // Verify
            Assert.Equal(expected, normalized);
        }

        /// <summary>
        /// Shouldn't allow empty tags.
        /// </summary>
        [Theory(DisplayName = "Should_DisallowEmptyTags")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" \r\n\t")]
        public void Should_DisallowEmptyTags(string tag) {
            // Execute
            Assert.Throws<InvalidOperationException>(() => {
                tagNormalizer.NormalizeTag(tag);
            });
        }

        [Fact(DisplayName = "Should_ConvertToLower")]
        public void Should_ConvertToLower() {
            // Execute
            var normalized = tagNormalizer.NormalizeTag("HELLO WORLD");

            // Verify
            Assert.Equal("hello world", normalized);
        }

        /// <summary>
        /// Should limit which characters are allowed in tags.
        /// </summary>
        [Theory(DisplayName = "Should_LimitTagCharacters")]
        [InlineData("abcdefghijklmnopqrstuvwxyz", true)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", true)]
        [InlineData("0123456789", true)]
        [InlineData("_.", true)]
        [InlineData("`~\\/<>,+=-:;'\"*!@#$?", false)]
        public void Should_LimitTagCharacters(string tag, bool shouldBeValid) {
            // This one's a bit harder to test, because this should be implemented as a white list,
            // meaning that the list of allowed characters is bounded, and the list of disallowed
            // characters, is not. In other words, we'll be able to ensure that all of the allowed
            // characters are allowed, but there is no limit to the disallowed characters, so there's
            // no practical way of testing that upper boundary.

            // Execute
            foreach (var chr in tag.ToArray()) {
                var chrStr = chr.ToString();
                if (shouldBeValid) {
                    tagNormalizer.NormalizeTag(chrStr);
                } else {
                    Assert.Throws<InvalidOperationException>(() => {
                        tagNormalizer.NormalizeTag(chrStr);
                    });
                }
            }
        }

        /// <summary>
        /// Should normalize multiple tags.
        /// </summary>
        [Fact(DisplayName = "Should_NormalizeMultipleTags")]
        public void Should_NormalizeMultipleTags() {
            // Execute
            var tags = tagNormalizer.NormalizeTags(new string[] { " hello", "WORLD" });

            // Verify
            Assert.Equal(2, tags.Count());
            Assert.Contains("hello", tags);
            Assert.Contains("world", tags);
        }

        /// <summary>
        /// The normalizer should deduplicate tags when used for a list of tags.
        /// </summary>
        [Fact(DisplayName = "Should_DeduplicateTags")]
        public void Should_DeduplicateTags() {
            // Execute
            var tags = tagNormalizer.NormalizeTags(new string[] { " hello", "WORLD", "HELLO  ", "  world  " });

            // Verify
            Assert.Equal(2, tags.Count());
            Assert.Contains("hello", tags);
            Assert.Contains("world", tags);
        }

        /// <summary>
        /// Shouldn't fail when given a null set of tags.
        /// </summary>
        [Fact(DisplayName = "Should_HandleNullTags")]
        public void Should_HandleNullTags() {
            // Verify - just do it
            tagNormalizer.NormalizeTags(null);
        }
    }
}