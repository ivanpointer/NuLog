/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.LogEvents;
using NuLog.Targets;
using System;
using System.Diagnostics;
using Xunit;

namespace NuLog.Tests.Unit.Targets
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the event log target.
    /// </summary>
    [Trait("Category", "Unit")]
    public class EventLogTargetTests
    {
        /// <summary>
        /// The event log target should write an event.
        /// </summary>
        [Theory(DisplayName = "Should_WriteEvent")]
        [InlineData("Hello, LogEventTarget!")]
        [InlineData("Hello, LogEventTarget Two!")]
        public void Should_WriteEvent(string logEventMessage)
        {
            // Setup
            IEventLog eventLog;
            var target = GetEventLogTarget(out eventLog);

            var layout = A.Fake<ILayout>();
            var layoutFactory = A.Fake<ILayoutFactory>();
            A.CallTo(() => layoutFactory.GetLayout(A<string>.Ignored))
                .Returns(layout);
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns(logEventMessage);

            target.Configure(null, layoutFactory);

            // Execute
            target.Write(new LogEvent());

            // Verify
            A.CallTo(() => eventLog.WriteEntry(A<string>.Ignored, logEventMessage, A<EventLogEntryType>.Ignored))
                .MustHaveHappened();
        }

        /// <summary>
        /// The log event target should write the configured source.
        /// </summary>
        [Theory(DisplayName = "Should_WriteSource")]
        [InlineData("TestEventLogTargetSource")]
        [InlineData("TestEventLogTargetSourceTwo")]
        public void Should_WriteSource(string source)
        {
            // Setup
            IEventLog eventLog;
            var target = GetEventLogTarget(out eventLog);

            var layout = A.Fake<ILayout>();
            var layoutFactory = A.Fake<ILayoutFactory>();
            A.CallTo(() => layoutFactory.GetLayout(A<string>.Ignored))
                .Returns(layout);
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Testing event log source.");

            // Execute
            var config = TargetConfigBuilder.Start()
                .Add("source", source)
                .Add("sourceLog", "HelloSourceLog")
                .Build();
            target.Configure(config);
            target.Configure(config, layoutFactory);
            target.Write(new LogEvent());

            // Verify
            A.CallTo(() => eventLog.WriteEntry(source, A<string>.Ignored, A<EventLogEntryType>.Ignored))
                .MustHaveHappened();
        }

        /// <summary>
        /// </summary>
        [Fact(DisplayName = "Should_UseLayout")]
        public void Should_UseLayout()
        {
            // Setup
            IEventLog eventLog;
            var target = GetEventLogTarget(out eventLog);

            var layout = A.Fake<ILayout>();
            var layoutFactory = A.Fake<ILayoutFactory>();
            A.CallTo(() => layoutFactory.GetLayout(A<string>.Ignored))
                .Returns(layout);
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Hello, Formatted!");

            target.Configure(null, layoutFactory);

            // Execute
            target.Write(new LogEvent { Message = "Hello, World!" });

            // Verify
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored)).MustHaveHappened();
            A.CallTo(() => eventLog.WriteEntry(A<string>.Ignored, "Hello, Formatted!", A<EventLogEntryType>.Ignored))
                .MustHaveHappened();
        }

        /// <summary>
        /// The target should read the event log entry type from the config.
        /// </summary>
        [Theory(DisplayName = "Should_WriteEventLogEntryType")]
        [InlineData("Error", EventLogEntryType.Error)]
        [InlineData("Warning", EventLogEntryType.Warning)]
        [InlineData("Information", EventLogEntryType.Information)]
        [InlineData("SuccessAudit", EventLogEntryType.SuccessAudit)]
        [InlineData("FailureAudit", EventLogEntryType.FailureAudit)]
        public void Should_WriteEventLogEntryType(string entryTypeString, EventLogEntryType entryType)
        {
            // Setup
            IEventLog eventLog;
            var target = GetEventLogTarget(out eventLog);

            var layout = A.Fake<ILayout>();
            var layoutFactory = A.Fake<ILayoutFactory>();
            A.CallTo(() => layoutFactory.GetLayout(A<string>.Ignored))
                .Returns(layout);
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Testing event log entry type.");

            // Execute
            var config = TargetConfigBuilder.Start()
                .Add("entryType", entryTypeString)
                .Add("source", "HelloSource")
                .Add("sourceLog", "HelloSourceLog")
                .Build();
            target.Configure(config);
            target.Configure(config, layoutFactory);
            target.Write(new LogEvent());

            // Verify
            A.CallTo(() => eventLog.WriteEntry(A<string>.Ignored, A<string>.Ignored, entryType))
                .MustHaveHappened();
        }

        /// <summary>
        /// The entry type should default to info if not configured.
        /// </summary>
        [Fact(DisplayName = "Should_DefaultEntryTypeInfo")]
        public void Should_DefaultEntryTypeInfo()
        {
            // Setup
            IEventLog eventLog;
            var target = GetEventLogTarget(out eventLog);

            var layout = A.Fake<ILayout>();
            var layoutFactory = A.Fake<ILayoutFactory>();
            A.CallTo(() => layoutFactory.GetLayout(A<string>.Ignored))
                .Returns(layout);
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Hello, Formatted!");

            var logger = new ConsoleTarget();
            logger.Configure(null, layoutFactory);

            var config = TargetConfigBuilder.Start()
                .Add("layout", "${Message}")
                .Add("source", "HelloSource")
                .Add("sourceLog", "HelloSourceLog")
                .Build();

            // Execute
            target.Configure(config);
            target.Configure(config, layoutFactory);
            target.Write(new LogEvent { Message = "Testing event log entry type." });

            // Verify
            A.CallTo(() => eventLog.WriteEntry(A<string>.Ignored, A<string>.Ignored, EventLogEntryType.Information))
                .MustHaveHappened();
        }

        /// <summary>
        /// The target should check if the log source exists when configured.
        /// </summary>
        [Fact(DisplayName = "Should_CheckIfSourceExists")]
        public void Should_CheckIfSourceExists()
        {
            // Setup
            IEventLog eventLog;
            var target = GetEventLogTarget(out eventLog);

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("source", "HelloSource")
                .Add("sourceLog", "HelloSourceLog")
                .Build());

            // Verify
            A.CallTo(() => eventLog.SourceExists("HelloSource"))
                .MustHaveHappened();
        }

        /// <summary>
        /// The target should create the source if it doesn't exist.
        /// </summary>
        [Fact(DisplayName = "Should_CreateSourceIfNotExists")]
        public void Should_CreateSourceIfNotExists()
        {
            // Setup
            IEventLog eventLog;
            var target = GetEventLogTarget(out eventLog);

            A.CallTo(() => eventLog.SourceExists(A<string>.Ignored))
                .Returns(false);

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("source", "HelloSource")
                .Add("sourceLog", "HelloSourceLog")
                .Build());

            // Verify
            A.CallTo(() => eventLog.CreateEventSource("HelloSource", A<string>.Ignored))
                .MustHaveHappened();
        }

        /// <summary>
        /// The target shouldn't create the source if it exists.
        /// </summary>
        [Fact(DisplayName = "Should_NotCreateSourceIfExists")]
        public void Should_NotCreateSourceIfExists()
        {
            // Setup
            IEventLog eventLog;
            var target = GetEventLogTarget(out eventLog);

            A.CallTo(() => eventLog.SourceExists(A<string>.Ignored))
                .Returns(true);

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("source", "HelloSource")
                .Add("sourceLog", "HelloSourceLog")
                .Build());

            // Verify
            A.CallTo(() => eventLog.CreateEventSource(A<string>.Ignored, A<string>.Ignored))
                .MustNotHaveHappened();
        }

        /// <summary>
        /// The target should load the source log from config, for creating the source.
        /// </summary>
        [Theory(DisplayName = "Should_LoadSourceLogFromConfig")]
        [InlineData("HelloSourceLog")]
        [InlineData("HelloSourceLogTwo")]
        public void Should_LoadSourceLogFromConfig(string sourceLog)
        {
            // Setup
            IEventLog eventLog;
            var target = GetEventLogTarget(out eventLog);

            A.CallTo(() => eventLog.SourceExists(A<string>.Ignored))
                .Returns(false);

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("source", "HelloSource")
                .Add("sourceLog", sourceLog)
                .Build());

            // Verify
            A.CallTo(() => eventLog.CreateEventSource(A<string>.Ignored, sourceLog))
                .MustHaveHappened();
        }

        /// <summary>
        /// The target should require the source setting in the config.
        /// </summary>
        [Fact(DisplayName = "Should_RequireSourceInConfig")]
        public void Should_RequireSourceInConfig()
        {
            // Setup
            IEventLog eventLog;
            var target = GetEventLogTarget(out eventLog);

            // Execute / Verify
            Assert.Throws(typeof(InvalidOperationException), () =>
            {
                target.Configure(TargetConfigBuilder.Start()
                    .Add("sourceLog", "Hello")
                .Build());
            });
        }

        /// <summary>
        /// The target should default the source log to "Application", when not configured.
        /// </summary>
        [Fact(DisplayName = "Should_DefaultSourceLogToApplication")]
        public void Should_DefaultSourceLogToApplication()
        {
            // Setup
            IEventLog eventLog;
            var target = GetEventLogTarget(out eventLog);

            A.CallTo(() => eventLog.SourceExists(A<string>.Ignored))
                .Returns(false);

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("source", "HelloSource")
                .Build());

            // Verify
            A.CallTo(() => eventLog.CreateEventSource(A<string>.Ignored, "Application"))
                .MustHaveHappened();
        }

        protected EventLogTarget GetEventLogTarget(out IEventLog eventLog)
        {
            eventLog = A.Fake<IEventLog>();
            var target = new EventLogTarget(eventLog);
            return target;
        }
    }
}