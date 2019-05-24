/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using System.Collections.Generic;
using Xunit;

namespace NuLog.Tests.Unit {

    /// <summary>
    /// Documents (and verifies) the expected behavior of the log manager.
    /// </summary>
    [Trait("Category", "Unit")]
    public class LogManagerTests {

        /// <summary>
        /// The log manager should call its logger factory to get a logger.
        /// </summary>
        [Fact(DisplayName = "Should_GetLogger")]
        public void Should_GetLogger() {
            // Setup
            var factory = A.Fake<ILoggerFactory>();
            LogManager.SetFactory(factory);

            // Execute
            var logger = LogManager.GetLogger();

            // Verify
            A.CallTo(() => factory.GetLogger(A<IMetaDataProvider>.Ignored, A<IEnumerable<string>>.Ignored))
                .MustHaveHappened();
        }

        /// <summary>
        /// The log manager should pass the given meta data provider to the factory when getting a logger.
        /// </summary>
        [Fact(DisplayName = "Should_GetLoggerWithMetaData")]
        public void Should_GetLoggerWithMetaData() {
            // Setup
            var metaDataProvider = A.Fake<IMetaDataProvider>();
            var factory = A.Fake<ILoggerFactory>();
            LogManager.SetFactory(factory);

            // Execute
            var logger = LogManager.GetLogger(metaDataProvider);

            // Verify
            A.CallTo(() => factory.GetLogger(metaDataProvider, A<IEnumerable<string>>.Ignored))
                .MustHaveHappened();
        }

        /// <summary>
        /// The log manager should pass the given default tags to the factory when getting a logger.
        /// </summary>
        [Fact(DisplayName = "Should_GetLoggerWithDefaultTags")]
        public void Should_GetLoggerWithDefaultTags() {
            // Setup
            var factory = A.Fake<ILoggerFactory>();
            LogManager.SetFactory(factory);

            var defaultTags = new string[] { "one_tag" };

            // Execute
            var logger = LogManager.GetLogger(null, defaultTags);

            // Verify
            A.CallTo(() => factory.GetLogger(A<IMetaDataProvider>.Ignored, A<IEnumerable<string>>.That.Contains("one_tag")))
                .MustHaveHappened();
        }

        /// <summary>
        /// The log manager should include the calling class name (full name) as a default tag when
        /// calling the factory to get the logger.
        /// </summary>
        [Fact(DisplayName = "Should_IncludeClassAsDefaultTag")]
        public void Should_IncludeClassAsDefaultTag() {
            // Setup
            var factory = A.Fake<ILoggerFactory>();
            LogManager.SetFactory(factory);

            // Execute
            var logger = LogManager.GetLogger(null, null);

            // Verify
            A.CallTo(() => factory.GetLogger(A<IMetaDataProvider>.Ignored, A<IEnumerable<string>>.That.Contains("NuLog.Tests.Unit.LogManagerTests")))
                .MustHaveHappened();
        }

        /// <summary>
        /// The log manager should include the calling class name (full name) as a default tag when
        /// calling the factory to get the logger.
        /// </summary>
        [Fact(DisplayName = "Should_IncludeClassAsDefaultTagWithOthers")]
        public void Should_IncludeClassAsDefaultTagWithOthers() {
            // Setup
            var factory = A.Fake<ILoggerFactory>();
            LogManager.SetFactory(factory);

            // Execute
            var logger = LogManager.GetLogger(null, "one_tag");

            // Verify
            A.CallTo(() => factory.GetLogger(A<IMetaDataProvider>.Ignored, A<IEnumerable<string>>.That.Contains("one_tag")))
                .MustHaveHappened();
            A.CallTo(() => factory.GetLogger(A<IMetaDataProvider>.Ignored, A<IEnumerable<string>>.That.Contains("NuLog.Tests.Unit.LogManagerTests")))
                .MustHaveHappened();
            A.CallTo(() => factory.GetLogger(A<IMetaDataProvider>.Ignored, A<IEnumerable<string>>.Ignored))
                .MustHaveHappened(1, Times.Exactly);
        }

        /// <summary>
        /// The manager should dispose the factory when it shuts down.
        /// </summary>
        [Fact(DisplayName = "Should_DisposeFactoryOnShutdown")]
        public void Should_DisposeFactoryOnShutdown() {
            // Setup
            var factory = A.Fake<ILoggerFactory>();
            LogManager.SetFactory(factory);

            // Execute
            LogManager.Shutdown();

            // Verify
            A.CallTo(() => factory.Dispose()).MustHaveHappened();
        }

        /// <summary>
        /// The manager should use the standard factory, if one isn't given.
        /// </summary>
        [Fact(DisplayName = "Should_ImplementStandardFactoryIfNotGiven")]
        public void Should_ImplementStandardFactoryIfNotGiven() {
            // Setup / Execute
            var logger = LogManager.GetLogger();
            Assert.NotNull(logger);
        }
    }
}