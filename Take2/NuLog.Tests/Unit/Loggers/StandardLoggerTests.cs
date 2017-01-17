﻿/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace NuLog.Tests.Unit.Loggers
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the standard logger.
    /// </summary>
    [Trait("Category", "Unit")]
    public class StandardLoggerTests
    {
        #region Simple Log Event Properties Tests

        // Tests around making sure that the logger sets the minimum expected properties on the log event.

        /// <summary>
        /// The logger should send a message to the dispatcher, for deferred dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_LogMessage_Later")]
        public void Should_LogMessage_Later()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var logger = GetLogger(dispatcher);

            // Execute
            logger.Log("Hello, World!");

            // Validate
            Assert.Equal(1, dispatcher.EnqueueForDispatchEvents.Count);
            var logEventLater = dispatcher.EnqueueForDispatchEvents.Single();
            Assert.Equal("Hello, World!", logEventLater.Message);
        }

        /// <summary>
        /// The logger should send a message to the dispatcher, for immediate dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_LogMessage_Now")]
        public void Should_LogMessage_Now()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var logger = GetLogger(dispatcher);

            // Execute
            logger.LogNow("Hello, World!");

            // Validate
            Assert.Equal(1, dispatcher.DispatchNowEvents.Count);
            var logEventNow = dispatcher.DispatchNowEvents.Single();
            Assert.Equal("Hello, World!", logEventNow.Message);
        }

        /// <summary>
        /// The logger should set the date logged on the log event, for deferred dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_SetDateLoggedUtc_Later")]
        public void Should_SetDateLoggedUtc_Later()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var logger = GetLogger(dispatcher);

            // Execute
            var nowUtc = DateTime.UtcNow;
            logger.Log("Timeless!");

            // Validate - Check a range, because a few ticks will happen. Allow for clock sliding, etc.
            var minDateTime = nowUtc.AddMilliseconds(-10);
            var maxDateTime = nowUtc.AddMilliseconds(10);
            var logEvent = dispatcher.EnqueueForDispatchEvents.Single();
            Assert.True(minDateTime <= logEvent.DateLogged && logEvent.DateLogged <= maxDateTime);
        }

        /// <summary>
        /// The logger should set the date logged on the log event, for immediate dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_SetDateLoggedUtc_Now")]
        public void Should_SetDateLoggedUtc_Now()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var logger = GetLogger(dispatcher);

            // Execute
            var nowUtc = DateTime.UtcNow;
            logger.LogNow("Timeless!");

            // Validate - Check a range, because a few ticks will happen. Allow for clock sliding, etc.
            var minDateTime = nowUtc.AddMilliseconds(-10);
            var maxDateTime = nowUtc.AddMilliseconds(10);
            var logEvent = dispatcher.DispatchNowEvents.Single();
            Assert.True(minDateTime <= logEvent.DateLogged && logEvent.DateLogged <= maxDateTime);
        }

        /// <summary>
        /// The logger should set the originating thread on the log event, for deferred dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_SetThread_Later")]
        public void Should_SetThread_Later()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var logger = GetLogger(dispatcher);

            // Execute
            logger.Log("Local!");

            // Validate
            var myThread = Thread.CurrentThread;
            var logEvent = dispatcher.EnqueueForDispatchEvents.Single();
            Assert.Equal(myThread, logEvent.Thread);
        }

        /// <summary>
        /// The logger should set the originating thread on the log event, for immediate dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_SetThread_Now")]
        public void Should_SetThread_Now()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var logger = GetLogger(dispatcher);

            // Execute
            logger.LogNow("Local!");

            // Validate
            var myThread = Thread.CurrentThread;
            var logEvent = dispatcher.DispatchNowEvents.Single();
            Assert.Equal(myThread, logEvent.Thread);
        }

        /// <summary>
        /// The logger should set the originating stack frame on the log event, for deferred dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_SetStackFrame_Later")]
        public void Should_SetStackFrame_Later()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var logger = GetLogger(dispatcher);

            // Execute
            logger.Log("I've been framed!");

            // Validate
            var logEvent = dispatcher.EnqueueForDispatchEvents.Single();
            Assert.NotNull(logEvent.LoggingStackFrame);
            Assert.Equal("Should_SetStackFrame_Later", logEvent.LoggingStackFrame.GetMethod().Name);
        }

        /// <summary>
        /// The logger should set the originating stack frame on the log event, for immediate dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_SetStackFrame_Now")]
        public void Should_SetStackFrame_Now()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var logger = GetLogger(dispatcher);

            // Execute
            logger.LogNow("I've been framed!");

            // Validate
            var logEvent = dispatcher.DispatchNowEvents.Single();
            Assert.NotNull(logEvent.LoggingStackFrame);
            Assert.Equal("Should_SetStackFrame_Now", logEvent.LoggingStackFrame.GetMethod().Name);
        }

        #endregion Simple Log Event Properties Tests

        #region Meta Data Tests

        /// <summary>
        /// Should set the given meta data onto the log event, for deferred dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_SetMetaData_Later")]
        public void Should_SetMetaData_Later()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var logger = GetLogger(dispatcher);
            var dob = new DateTime(1809, 2, 12);
            var metaData = new Dictionary<string, object>
            {
                { "MyDOB", dob },
                { "Birthplace", "Hodgenville, KY" }
            };

            // Execute
            logger.Log("Hello, World!", metaData);

            // Validate
            Assert.Equal(1, dispatcher.EnqueueForDispatchEvents.Count);
            var logEvent = dispatcher.EnqueueForDispatchEvents.Single();
            Assert.Equal(2, logEvent.MetaData.Keys.Count);
            Assert.Equal(dob, logEvent.MetaData["MyDOB"]);
            Assert.Equal("Hodgenville, KY", logEvent.MetaData["Birthplace"]);
        }

        /// <summary>
        /// Should set the given meta data onto the log event, for immediate dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_SetMetaData_Now")]
        public void Should_SetMetaData_Now()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var logger = GetLogger(dispatcher);
            var dob = new DateTime(1809, 2, 12);
            var metaData = new Dictionary<string, object>
            {
                { "MyDOB", dob },
                { "Birthplace", "Hodgenville, KY" }
            };

            // Execute
            logger.LogNow("Hello, World!", metaData);

            // Validate
            Assert.Equal(1, dispatcher.DispatchNowEvents.Count);
            var logEvent = dispatcher.DispatchNowEvents.Single();
            Assert.Equal(2, logEvent.MetaData.Keys.Count);
            Assert.Equal(dob, logEvent.MetaData["MyDOB"]);
            Assert.Equal("Hodgenville, KY", logEvent.MetaData["Birthplace"]);
        }

        #endregion Meta Data Tests

        #region Exception Tests

        /// <summary>
        /// Should set the given exception into the log event, for deferred dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_SetException_Later")]
        public void Should_SetException_Later()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var logger = GetLogger(dispatcher);
            var exception = new Exception("Caught me!");

            // Execute
            logger.Log("Hello, World!", exception);

            // Validate
            var logEvent = dispatcher.EnqueueForDispatchEvents.Single();
            Assert.Equal(exception, logEvent.Exception);
        }

        /// <summary>
        /// Should set the given exception into the log event, for immediate dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_SetException_Now")]
        public void Should_SetException_Now()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var logger = GetLogger(dispatcher);
            var exception = new Exception("Caught me!");

            // Execute
            logger.LogNow("Hello, World!", exception);

            // Validate
            var logEvent = dispatcher.DispatchNowEvents.Single();
            Assert.Equal(exception, logEvent.Exception);
        }

        /// <summary>
        /// Should set the given exception, and meta data into the log event, for deferred dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_SetExceptionAndMetaData_Later")]
        public void Should_SetExceptionAndMetaData_Later()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var logger = GetLogger(dispatcher);
            var exception = new Exception("Caught me!");
            var dob = new DateTime(1809, 2, 12);
            var metaData = new Dictionary<string, object>
            {
                { "MyDOB", dob },
                { "Birthplace", "Hodgenville, KY" }
            };

            // Execute
            logger.Log("Hello, World!", exception, metaData);

            // Validate
            var logEvent = dispatcher.EnqueueForDispatchEvents.Single();
            Assert.Equal(exception, logEvent.Exception);
            Assert.Equal(2, logEvent.MetaData.Keys.Count);
            Assert.Equal(dob, logEvent.MetaData["MyDOB"]);
            Assert.Equal("Hodgenville, KY", logEvent.MetaData["Birthplace"]);
        }

        /// <summary>
        /// Should set the given exception, and meta data into the log event, for immediate dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_SetExceptionAndMetaData_Now")]
        public void Should_SetExceptionAndMetaData_Now()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var logger = GetLogger(dispatcher);
            var exception = new Exception("Caught me!");
            var dob = new DateTime(1809, 2, 12);
            var metaData = new Dictionary<string, object>
            {
                { "MyDOB", dob },
                { "Birthplace", "Hodgenville, KY" }
            };

            // Execute
            logger.LogNow("Hello, World!", exception, metaData);

            // Validate
            var logEvent = dispatcher.DispatchNowEvents.Single();
            Assert.Equal(exception, logEvent.Exception);
            Assert.Equal(2, logEvent.MetaData.Keys.Count);
            Assert.Equal(dob, logEvent.MetaData["MyDOB"]);
            Assert.Equal("Hodgenville, KY", logEvent.MetaData["Birthplace"]);
        }

        #endregion Exception Tests

        #region Tags - Simple Tests

        /// <summary>
        /// Should set the given tags into the log event, for deferred dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_SetTags_Later")]
        public void Should_SetTags_Later()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var logger = GetLogger(dispatcher);

            // Execute
            logger.Log("Hello, World!", "george", "willy", "fred");

            // Validate
            var logEvent = dispatcher.EnqueueForDispatchEvents.Single();
            Assert.Equal(3, logEvent.Tags.Count());
            Assert.Contains("george", logEvent.Tags);
            Assert.Contains("willy", logEvent.Tags);
            Assert.Contains("fred", logEvent.Tags);
        }

        /// <summary>
        /// Should set the given tags into the log event, for immediate dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_SetTags_Now")]
        public void Should_SetTags_Now()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var logger = GetLogger(dispatcher);

            // Execute
            logger.LogNow("Hello, World!", "george", "willy", "fred");

            // Validate
            var logEvent = dispatcher.DispatchNowEvents.Single();
            Assert.Equal(3, logEvent.Tags.Count());
            Assert.Contains("george", logEvent.Tags);
            Assert.Contains("willy", logEvent.Tags);
            Assert.Contains("fred", logEvent.Tags);
        }

        /// <summary>
        /// Should set the given tags, and meta data into the log event, for deferred dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_SetTagsAndMetaData_Later")]
        public void Should_SetTagsAndMetaData_Later()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var logger = GetLogger(dispatcher);
            var dob = new DateTime(1809, 2, 12);
            var metaData = new Dictionary<string, object>
            {
                { "MyDOB", dob },
                { "Birthplace", "Hodgenville, KY" }
            };

            // Execute
            logger.Log("Hello, World!", metaData, "george", "willy", "fred");

            // Validate
            var logEvent = dispatcher.EnqueueForDispatchEvents.Single();
            Assert.Equal(3, logEvent.Tags.Count());
            Assert.Contains("george", logEvent.Tags);
            Assert.Contains("willy", logEvent.Tags);
            Assert.Contains("fred", logEvent.Tags);
            Assert.Equal(2, logEvent.MetaData.Keys.Count);
            Assert.Equal(dob, logEvent.MetaData["MyDOB"]);
            Assert.Equal("Hodgenville, KY", logEvent.MetaData["Birthplace"]);
        }

        /// <summary>
        /// Should set the given tags, and meta data into the log event, for immediate dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_SetTagsAndMetaData_Now")]
        public void Should_SetTagsAndMetaData_Now()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var logger = GetLogger(dispatcher);
            var dob = new DateTime(1809, 2, 12);
            var metaData = new Dictionary<string, object>
            {
                { "MyDOB", dob },
                { "Birthplace", "Hodgenville, KY" }
            };

            // Execute
            logger.LogNow("Hello, World!", metaData, "george", "willy", "fred");

            // Validate
            var logEvent = dispatcher.DispatchNowEvents.Single();
            Assert.Equal(3, logEvent.Tags.Count());
            Assert.Contains("george", logEvent.Tags);
            Assert.Contains("willy", logEvent.Tags);
            Assert.Contains("fred", logEvent.Tags);
            Assert.Equal(2, logEvent.MetaData.Keys.Count);
            Assert.Equal(dob, logEvent.MetaData["MyDOB"]);
            Assert.Equal("Hodgenville, KY", logEvent.MetaData["Birthplace"]);
        }

        /// <summary>
        /// Should set the given tags, and exception into the log event, for deferred dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_SetTagsAndException_Later")]
        public void Should_SetTagsAndException_Later()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var logger = GetLogger(dispatcher);
            var exception = new Exception("Caught me!");

            // Execute
            logger.Log("Hello, World!", exception, "george", "willy", "fred");

            // Validate
            var logEvent = dispatcher.EnqueueForDispatchEvents.Single();
            Assert.Equal(3, logEvent.Tags.Count());
            Assert.Contains("george", logEvent.Tags);
            Assert.Contains("willy", logEvent.Tags);
            Assert.Contains("fred", logEvent.Tags);
            Assert.Equal(exception, logEvent.Exception);
        }

        /// <summary>
        /// Should set the given tags, and exception into the log event, for immediate dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_SetTagsAndException_Now")]
        public void Should_SetTagsAndException_Now()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var logger = GetLogger(dispatcher);
            var exception = new Exception("Caught me!");

            // Execute
            logger.LogNow("Hello, World!", exception, "george", "willy", "fred");

            // Validate
            var logEvent = dispatcher.DispatchNowEvents.Single();
            Assert.Equal(3, logEvent.Tags.Count());
            Assert.Contains("george", logEvent.Tags);
            Assert.Contains("willy", logEvent.Tags);
            Assert.Contains("fred", logEvent.Tags);
            Assert.Equal(exception, logEvent.Exception);
        }

        /// <summary>
        /// Should set the given tags, exception and meta data into the log event, for deferred dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_SetTagsExceptionAndMetaData_Later")]
        public void Should_SetTagsExceptionAndMetaData_Later()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var logger = GetLogger(dispatcher);
            var exception = new Exception("Caught me!");
            var dob = new DateTime(1809, 2, 12);
            var metaData = new Dictionary<string, object>
            {
                { "MyDOB", dob },
                { "Birthplace", "Hodgenville, KY" }
            };

            // Execute
            logger.Log("Hello, World!", exception, metaData, "george", "willy", "fred");

            // Validate
            var logEvent = dispatcher.EnqueueForDispatchEvents.Single();
            Assert.Equal(3, logEvent.Tags.Count());
            Assert.Contains("george", logEvent.Tags);
            Assert.Contains("willy", logEvent.Tags);
            Assert.Contains("fred", logEvent.Tags);
            Assert.Equal(exception, logEvent.Exception);
            Assert.Equal(2, logEvent.MetaData.Keys.Count);
            Assert.Equal(dob, logEvent.MetaData["MyDOB"]);
            Assert.Equal("Hodgenville, KY", logEvent.MetaData["Birthplace"]);
        }

        /// <summary>
        /// Should set the given tags, exception and meta data into the log event, for immediate dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_SetTagsExceptionAndMetaData_Now")]
        public void Should_SetTagsExceptionAndMetaData_Now()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var logger = GetLogger(dispatcher);
            var exception = new Exception("Caught me!");
            var dob = new DateTime(1809, 2, 12);
            var metaData = new Dictionary<string, object>
            {
                { "MyDOB", dob },
                { "Birthplace", "Hodgenville, KY" }
            };

            // Execute
            logger.LogNow("Hello, World!", exception, metaData, "george", "willy", "fred");

            // Validate
            var logEvent = dispatcher.DispatchNowEvents.Single();
            Assert.Equal(3, logEvent.Tags.Count());
            Assert.Contains("george", logEvent.Tags);
            Assert.Contains("willy", logEvent.Tags);
            Assert.Contains("fred", logEvent.Tags);
            Assert.Equal(exception, logEvent.Exception);
            Assert.Equal(2, logEvent.MetaData.Keys.Count);
            Assert.Equal(dob, logEvent.MetaData["MyDOB"]);
            Assert.Equal("Hodgenville, KY", logEvent.MetaData["Birthplace"]);
        }

        #endregion Tags - Simple Tests

        #region Tags - Advanced Tests

        /// <summary>
        /// The default tags on the logger should be included with the log event, for deferred dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_IncludeDefaultTags_Later")]
        public void Should_IncludeDefaultTags_Later()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var defaultTags = new string[] { "one_tag", "two_tag" };
            var logger = GetLogger(dispatcher, defaultTags);

            // Execute
            logger.Log("Hello, World!", "red_tag", "blue_tag");

            // Validate
            var logEvent = dispatcher.EnqueueForDispatchEvents.Single();
            Assert.Equal(4, logEvent.Tags.Count());
            Assert.Contains("one_tag", logEvent.Tags);
            Assert.Contains("two_tag", logEvent.Tags);
            Assert.Contains("red_tag", logEvent.Tags);
            Assert.Contains("blue_tag", logEvent.Tags);
        }

        /// <summary>
        /// The default tags on the logger should be included with the log event, for immediate dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_IncludeDefaultTags_Now")]
        public void Should_IncludeDefaultTags_Now()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var defaultTags = new string[] { "one_tag", "two_tag" };
            var logger = GetLogger(dispatcher, defaultTags);

            // Execute
            logger.LogNow("Hello, World!", "red_tag", "blue_tag");

            // Validate
            var logEvent = dispatcher.DispatchNowEvents.Single();
            Assert.Equal(4, logEvent.Tags.Count());
            Assert.Contains("one_tag", logEvent.Tags);
            Assert.Contains("two_tag", logEvent.Tags);
            Assert.Contains("red_tag", logEvent.Tags);
            Assert.Contains("blue_tag", logEvent.Tags);
        }

        /// <summary>
        /// The default tags on the logger should be included with the log event, even if no tags are
        /// given by the caller, for deferred dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_IncludeOnlyDefaultTags_Later")]
        public void Should_IncludeOnlyDefaultTags_Later()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var defaultTags = new string[] { "one_tag", "two_tag" };
            var logger = GetLogger(dispatcher, defaultTags);

            // Execute
            logger.Log("Hello, World!");

            // Validate
            var logEvent = dispatcher.EnqueueForDispatchEvents.Single();
            Assert.Equal(2, logEvent.Tags.Count());
            Assert.Contains("one_tag", logEvent.Tags);
            Assert.Contains("two_tag", logEvent.Tags);
        }

        /// <summary>
        /// The default tags on the logger should be included with the log event, even if no tags are
        /// given by the caller, for immediate dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_IncludeOnlyDefaultTags_Now")]
        public void Should_IncludeOnlyDefaultTags_Now()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var defaultTags = new string[] { "one_tag", "two_tag" };
            var logger = GetLogger(dispatcher, defaultTags);

            // Execute
            logger.LogNow("Hello, World!");

            // Validate
            var logEvent = dispatcher.DispatchNowEvents.Single();
            Assert.Equal(2, logEvent.Tags.Count());
            Assert.Contains("one_tag", logEvent.Tags);
            Assert.Contains("two_tag", logEvent.Tags);
        }

        /// <summary>
        /// The logger should call the tag normalizer when only sending tags from the caller, for
        /// deferred dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_NormalizeTags_Later")]
        public void Should_NormalizeTags_Later()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var fakeNormalizer = A.Fake<ITagNormalizer>();
            var logger = GetLogger(dispatcher, null, fakeNormalizer);

            // Execute
            logger.Log("Hello, World!", "one_fish", "two_fish");

            // Validate
            A.CallTo(() => fakeNormalizer.NormalizeTags(A<IEnumerable<string>>.Ignored)).MustHaveHappened();
        }

        /// <summary>
        /// The logger should call the tag normalizer when only sending tags from the caller, for
        /// immediate dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_NormalizeTags_Now")]
        public void Should_NormalizeTags_Now()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var fakeNormalizer = A.Fake<ITagNormalizer>();
            var logger = GetLogger(dispatcher, null, fakeNormalizer);

            // Execute
            logger.LogNow("Hello, World!", "one_fish", "two_fish");

            // Validate
            A.CallTo(() => fakeNormalizer.NormalizeTags(A<IEnumerable<string>>.Ignored)).MustHaveHappened();
        }

        /// <summary>
        /// The logger shouldn't call the tag normalizer when only sending tags from default tags,
        /// except on construction, for deferred dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_NormalizeDefaultTags_WithCall_Later")]
        public void Should_NormalizeDefaultTags_WithCall_Later()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var fakeNormalizer = A.Fake<ITagNormalizer>();
            var logger = GetLogger(dispatcher, new string[] { "one_tag", "two_tag" }, fakeNormalizer);

            // Execute
            logger.Log("Hello, World!");

            // Validate
            A.CallTo(() => fakeNormalizer.NormalizeTags(A<IEnumerable<string>>.Ignored)).MustHaveHappened(Repeated.AtLeast.Once);
        }

        /// <summary>
        /// The logger shouldn't call the tag normalizer when only sending tags from default tags,
        /// except on construction, for immediate dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_NormalizeDefaultTags_WithCall_Now")]
        public void Should_NormalizeDefaultTags_WithCall_Now()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var fakeNormalizer = A.Fake<ITagNormalizer>();
            var logger = GetLogger(dispatcher, new string[] { "one_tag", "two_tag" }, fakeNormalizer);

            // Execute
            logger.LogNow("Hello, World!");

            // Validate
            A.CallTo(() => fakeNormalizer.NormalizeTags(A<IEnumerable<string>>.Ignored)).MustHaveHappened(Repeated.AtLeast.Once);
        }

        /// <summary>
        /// The logger shouldn't call the tag normalizer when only sending tags from default tags,
        /// except on construction, for deferred dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_NormalizeDefaultTags_WithCallWithTags_Later")]
        public void Should_NormalizeDefaultTags_WithCallWithTags_Later()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var fakeNormalizer = A.Fake<ITagNormalizer>();
            var logger = GetLogger(dispatcher, new string[] { "one_tag", "two_tag" }, fakeNormalizer);

            // Execute
            logger.Log("Hello, World!", "red_tag", "blue_tag");

            // Validate
            A.CallTo(() => fakeNormalizer.NormalizeTags(A<IEnumerable<string>>.Ignored)).MustHaveHappened(Repeated.AtLeast.Once);
        }

        /// <summary>
        /// The logger shouldn't call the tag normalizer when only sending tags from default tags,
        /// except on construction, for immediate dispatch.
        /// </summary>
        [Fact(DisplayName = "Should_NormalizeDefaultTags_WithCallWithTags_Now")]
        public void Should_NormalizeDefaultTags_WithCallWithTags_Now()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var fakeNormalizer = A.Fake<ITagNormalizer>();
            var logger = GetLogger(dispatcher, new string[] { "one_tag", "two_tag" }, fakeNormalizer);

            // Execute
            logger.LogNow("Hello, World!", "red_tag", "blue_tag");

            // Validate
            A.CallTo(() => fakeNormalizer.NormalizeTags(A<IEnumerable<string>>.Ignored)).MustHaveHappened(Repeated.AtLeast.Once);
        }

        #endregion Tags - Advanced Tests

        #region Meta Data Provider Tests

        /// <summary>
        /// If a meta data provider is given to the logger, it should call it when sending a log message.
        /// </summary>
        [Fact(DisplayName = "Should_CallMetaDataProvider_Later")]
        public void Should_CallMetaDataProvider_Later()
        {
            // Setup
            var dispatcher = new StubDispatcher<LogEvent>();
            var metaDataProvider = A.Fake<IMetaDataProvider>();
            var logger = GetLogger(dispatcher, null, null, metaDataProvider);

            // Execute
            logger.Log("Hello, World!");

            // Validate The call to the meta data provider should be reasonably idempotent,
            // therefore, we cannot expect it to be called exactly once, but we can expect at least once.
            A.CallTo(() => metaDataProvider.ProvideMetaData()).MustHaveHappened(Repeated.AtLeast.Once);
        }

        // TODO - CONTINUE TO BUILD THIS OUT
        // - should actually get meta data from the provider - perhaps replace the test above with
        //   this check?
        // - should mix meta data from provider, and log call
        // - should overwrite meta data from provider, with conflicting meta data in call

        #endregion Meta Data Provider Tests

        /// <summary>
        /// Gets the logger under test.
        /// </summary>
        protected ILogger GetLogger(IDispatcher dispatcher, IEnumerable<string> defaultTags = null, ITagNormalizer tagNormalizer = null, IMetaDataProvider metaDataProvider = null)
        {
            tagNormalizer = tagNormalizer ?? new StandardTagNormalizer();
            return new StandardLogger(dispatcher, tagNormalizer, metaDataProvider, defaultTags);
        }
    }

    /// <summary>
    /// A stub dispatcher for testing the logger.
    /// </summary>
    internal class StubDispatcher<TLogEvent> : IDispatcher where TLogEvent : ILogEvent
    {
        public List<TLogEvent> DispatchNowEvents { get; set; }

        public List<TLogEvent> EnqueueForDispatchEvents { get; set; }

        public bool Disposed { get; private set; }

        public StubDispatcher()
        {
            DispatchNowEvents = new List<TLogEvent>();

            EnqueueForDispatchEvents = new List<TLogEvent>();
        }

        public void DispatchNow(ILogEvent logEvent)
        {
            DispatchNowEvents.Add((TLogEvent)logEvent);
        }

        public void Dispose()
        {
            Disposed = true;
        }

        public void EnqueueForDispatch(ILogEvent logEvent)
        {
            EnqueueForDispatchEvents.Add((TLogEvent)logEvent);
        }
    }
}