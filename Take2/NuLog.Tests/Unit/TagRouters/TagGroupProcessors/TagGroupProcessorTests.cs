/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Dispatchers.TagRouters;
using NuLog.TagRouters;
using NuLog.TagRouters.TagGroupProcessors;
using System.Collections.Generic;
using Xunit;

namespace NuLog.Tests.Unit.TagRouters.TagGroupProcessors {

    /// <summary>
    /// Documents the expected behavior of a tag group processor.
    /// </summary>
    [Trait("Category", "Unit")]
    public class TagGroupProcessorTests {

        /// <summary>
        /// The tag group processor should always include the tag itself.
        /// </summary>
        [Fact(DisplayName = "Should_IncludeSelf")]
        public void Should_IncludeSelf() {
            // Setup
            var processor = GetTagGroupProcessor(null);

            // Execute
            var tags = processor.GetAliases("meself");

            // Verify
            Assert.Contains("meself", tags);
        }

        /// <summary>
        /// Check one simple rule.
        /// </summary>
        [Fact(DisplayName = "Should_IncludeSimpleAlias")]
        public void Should_IncludeSimpleAlias() {
            // Setup
            var tagGroups = new List<TagGroup>
            {
                new TagGroup
                {
                    BaseTag = "error",
                    Aliases = new string[] { "fatal" }
                }
            };
            var processor = GetTagGroupProcessor(tagGroups);

            // Execute
            var tags = processor.GetAliases("fatal");

            // Verify
            Assert.Contains("error", tags);
        }

        /// <summary>
        /// Check multiple rules.
        /// </summary>
        [Fact(DisplayName = "Should_ReturnMultipleAliases")]
        public void Should_ReturnMultipleAliases() {
            // Setup
            var tagGroups = new List<TagGroup>
            {
                new TagGroup
                {
                    BaseTag = "warn",
                    Aliases = new string[] { "error", "fatal" }
                }, new TagGroup
                {
                    BaseTag = "error",
                    Aliases = new string[] { "fatal" }
                }
            };
            var processor = GetTagGroupProcessor(tagGroups);

            // Execute
            var tags = processor.GetAliases("fatal");

            // Verify
            Assert.Contains("fatal", tags);
            Assert.Contains("error", tags);
            Assert.Contains("warn", tags);
        }

        #region Internals

        /// <summary>
        /// Provides a new instance of the tag group processor under test.
        /// </summary>
        protected ITagGroupProcessor GetTagGroupProcessor(IEnumerable<TagGroup> tagGroups) {
            return new StandardTagGroupProcessor(tagGroups);
        }

        #endregion Internals
    }
}