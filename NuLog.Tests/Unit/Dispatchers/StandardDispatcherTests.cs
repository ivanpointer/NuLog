﻿/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.Dispatchers;
using NuLog.Dispatchers.TagRouters;
using NuLog.LogEvents;
using NuLog.Loggers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Xunit;

namespace NuLog.Tests.Unit.Dispatchers {

    /// <summary>
    /// Tests to document the expected behavior of the standard dispatcher.
    /// </summary>
    [Trait("Category", "Unit")]
    public class StandardDispatcherTests {

        /// <summary>
        /// The dispatcher should ask the tag router about which target to dispatch to, and tell the
        /// log event to write itself to the target. The call should be made on the same thread.
        /// </summary>
        [Fact(DisplayName = "Should_DispatchNow")]
        public void Should_DispatchNow() {
            // Setup
            var logEvent = new StubLogEvent {
                Tags = new string[] { "my_tag" }
            };
            var target = FakeTarget("fake_target");
            var tagRouter = FakeTagRouter();
            var dispatcher = GetDispatcher(new ITarget[] { target }, tagRouter);
            var currentThreadId = Thread.CurrentThread.ManagedThreadId;

            // Have our fake router return our fake target's name, when given our fake log event's tags
            A.CallTo(() => tagRouter.Route(logEvent.Tags))
                .Returns(new string[] { target.Name });

            // Execute
            dispatcher.DispatchNow(logEvent);

            // Validate
            Assert.Equal(currentThreadId, logEvent.ManagedThreadId);
        }

        /// <summary>
        /// The dispatcher should defer/delay the call to have the log event write itself to the target.
        /// </summary>
        [Fact(DisplayName = "Should_DispatchLater")]
        public void Should_DispatchLater() {
            // Setup
            var logEvent = new StubLogEvent {
                Tags = new string[] { "my_tag" }
            };
            var target = FakeTarget("fake_target");
            var tagRouter = FakeTagRouter();
            var dispatcher = GetDispatcher(new ITarget[] { target }, tagRouter);
            var currentThreadId = Thread.CurrentThread.ManagedThreadId;

            // Have our fake router return our fake target's name, when given our fake log event's tags
            A.CallTo(() => tagRouter.Route(logEvent.Tags))
                .Returns(new string[] { target.Name });

            // Execute
            dispatcher.EnqueueForDispatch(logEvent);

            // Because we're threaded, wait for the event to be handled - give up after 5 seconds.
            var sw = new Stopwatch();
            sw.Start();
            while (logEvent.Dispatched == false && sw.ElapsedMilliseconds < 5000) {
                Thread.Sleep(50);
            }

            // Validate
            Assert.True(logEvent.Dispatched);
            Assert.NotEqual(0, logEvent.ManagedThreadId);
            Assert.NotEqual(currentThreadId, logEvent.ManagedThreadId);
        }

        /// <summary>
        /// To help enforce good behavior, a dispatcher should refuse to enqueue new log events after
        /// being disposed.
        /// </summary>
        [Fact(DisplayName = "Should_RefuseNewLogEventsAfterDispose")]
        public void Should_RefuseNewLogEventsAfterDispose() {
            // Setup
            var logEvent = new StubLogEvent {
                Tags = new string[] { "my_tag" }
            };
            var target = FakeTarget("fake_target");
            var tagRouter = FakeTagRouter();
            var dispatcher = GetDispatcher(new ITarget[] { target }, tagRouter);
            var currentThreadId = Thread.CurrentThread.ManagedThreadId;

            // Have our fake router return our fake target's name, when given our fake log event's tags
            A.CallTo(() => tagRouter.Route(logEvent.Tags))
                .Returns(new string[] { target.Name });

            // Dispose our dispatcher
            dispatcher.Dispose();

            // Execute / Verify
            Assert.Throws<InvalidOperationException>(() => {
                dispatcher.EnqueueForDispatch(logEvent);
            });
        }

        /// <summary>
        /// The dispatcher should dispose its targets, when it is disposed.
        /// </summary>
        [Fact(DisplayName = "Should_DisposeTargetsOnDispose")]
        public void Should_DisposeTargetsOnDispose() {
            // Setup
            var target = FakeTarget("fake_target");
            var tagRouter = FakeTagRouter();
            var dispatcher = GetDispatcher(new ITarget[] { target }, tagRouter);

            // Execute
            dispatcher.Dispose();

            // Verify
            A.CallTo(() => target.Dispose()).MustHaveHappened();
        }

        /// <summary>
        /// The dispatcher should catch, report, and "stuff" exceptions thrown by the targets.
        ///
        /// Exceptions in the logger shouldn't interfere with the logging application.
        /// </summary>
        [Fact(DisplayName = "Should_EncapsulateTargetExceptionsLater")]
        public void Should_EncapsulateTargetExceptionsLater() {
            // Setup
            var target = FakeTarget("exceptional_target");
            A.CallTo(() => target.Write(A<LogEvent>.Ignored)).Throws(new Exception("Uh, yer target done broke!"));
            var tagRouter = FakeTagRouter();
            A.CallTo(() => tagRouter.Route(A<IEnumerable<string>>.Ignored)).Returns(new string[] { "exceptional_target" });
            var dispatcher = GetDispatcher(new ITarget[] { target }, tagRouter);

            // Execute / Verify (nothing should be thrown)
            dispatcher.EnqueueForDispatch(new LogEvent {
                Message = "A doomed event."
            });
            dispatcher.Dispose();
        }

        /// <summary>
        /// The dispatcher should catch, report, and "stuff" exceptions thrown by the targets.
        ///
        /// Exceptions in the logger shouldn't interfere with the logging application.
        /// </summary>
        [Fact(DisplayName = "Should_EncapsulateTargetExceptionsNow")]
        public void Should_EncapsulateTargetExceptionsNow() {
            // Setup
            var target = FakeTarget("exceptional_target");
            A.CallTo(() => target.Write(A<LogEvent>.Ignored)).Throws(new Exception("Uh, yer target done broke!"));
            var tagRouter = FakeTagRouter();
            A.CallTo(() => tagRouter.Route(A<IEnumerable<string>>.Ignored)).Returns(new string[] { "exceptional_target" });
            var dispatcher = GetDispatcher(new ITarget[] { target }, tagRouter);

            // Execute / Verify (nothing should be thrown)
            dispatcher.DispatchNow(new LogEvent {
                Message = "A doomed event."
            });
        }

        /// <summary>
        /// When an exception is thrown in NuLog, it should be logged to the fallback logger, instead
        /// of bubbling up.
        /// </summary>
        [Fact(DisplayName = "Should_FallbackLoggingLater")]
        public void Should_FallbackLoggingLater() {
            // Setup
            var fallbackLogger = A.Fake<IFallbackLogger>();
            var target = FakeTarget("exceptional_target");
            A.CallTo(() => target.Write(A<LogEvent>.Ignored)).Throws(new Exception("Uh, yer target done broke!"));
            var tagRouter = FakeTagRouter();
            A.CallTo(() => tagRouter.Route(A<IEnumerable<string>>.Ignored)).Returns(new string[] { "exceptional_target" });
            var dispatcher = GetDispatcher(new ITarget[] { target }, tagRouter, fallbackLogger);

            // Execute
            dispatcher.EnqueueForDispatch(new LogEvent {
                Message = "A doomed event."
            });
            dispatcher.Dispose();

            // Verify
            A.CallTo(() => fallbackLogger.Log(A<Exception>.That.Matches(m => m.Message == "Uh, yer target done broke!"), target, A<LogEvent>.That.Matches(m => m.Message == "A doomed event.")))
                .MustHaveHappened();
        }

        /// <summary>
        /// When an exception is thrown in NuLog, it should be logged to the fallback logger, instead
        /// of bubbling up.
        /// </summary>
        [Fact(DisplayName = "Should_FallbackLoggingNow")]
        public void Should_FallbackLoggingNow() {
            // Setup
            var fallbackLogger = A.Fake<IFallbackLogger>();
            var target = FakeTarget("exceptional_target");
            A.CallTo(() => target.Write(A<LogEvent>.Ignored)).Throws(new Exception("Uh, yer target done broke!"));
            var tagRouter = FakeTagRouter();
            A.CallTo(() => tagRouter.Route(A<IEnumerable<string>>.Ignored)).Returns(new string[] { "exceptional_target" });
            var dispatcher = GetDispatcher(new ITarget[] { target }, tagRouter, fallbackLogger);

            // Execute
            dispatcher.DispatchNow(new LogEvent {
                Message = "A doomed event."
            });

            // Verify
            A.CallTo(() => fallbackLogger.Log(A<Exception>.That.Matches(m => m.Message == "Uh, yer target done broke!"), target, A<LogEvent>.That.Matches(m => m.Message == "A doomed event.")))
                .MustHaveHappened();
        }

        /// <summary>
        /// When the fallback logger fails, we should fall back to trace.
        /// </summary>
        [Fact(DisplayName = "Should_FallbackLoggerFailFallbackToTrace")]
        public void Should_FallbackLoggerFailFallbackToTrace() {
            // Setup
            var fallbackLogger = A.Fake<IFallbackLogger>();
            A.CallTo(() => fallbackLogger.Log(A<Exception>._, A<ITarget>._, A<ILogEvent>._))
                .Throws(new Exception("The fallback logger broke!"));

            var target = FakeTarget("exceptional_target");
            A.CallTo(() => target.Write(A<LogEvent>.Ignored)).Throws(new Exception("Uh, yer target done broke!"));

            var tagRouter = FakeTagRouter();
            A.CallTo(() => tagRouter.Route(A<IEnumerable<string>>.Ignored)).Returns(new string[] { "exceptional_target" });

            var dispatcher = GetDispatcher(new ITarget[] { target }, tagRouter, fallbackLogger);

            TraceListener traceListener = A.Fake<TraceListener>();
            Trace.Listeners.Add(traceListener);

            // Execute
            dispatcher.DispatchNow(new LogEvent {
                Message = "A doomed event."
            });

            // Verify
            A.CallTo(() => traceListener.TraceEvent(
                A<TraceEventCache>._,
                A<string>._,
                A<TraceEventType>.That.IsEqualTo(TraceEventType.Error),
                A<int>._,
                A<string>.That.Contains("Failure writing exception to fallback logger:"),
                A<object[]>._))
                    .MustHaveHappened(1, Times.Exactly);
        }

        /// <summary>
        /// When the fallback logger fails, we should fall back to trace - when there's an error in
        /// the targets list.
        /// </summary>
        [Fact(DisplayName = "Should_FallbackLoggerFailFallbackToTrace_FailureInTargetsList")]
        public void Should_FallbackLoggerFailFallbackToTrace_FailureInTargetsList() {
            // Setup
            var fallbackLogger = A.Fake<IFallbackLogger>();
            A.CallTo(() => fallbackLogger.Log(A<string>._, A<object[]>._))
                .Throws(new Exception("The fallback logger broke!"));

            var tagRouter = FakeTagRouter();
            A.CallTo(() => tagRouter.Route(A<IEnumerable<string>>.Ignored)).Throws(new Exception("Broken tag router!"));

            var dispatcher = GetDispatcher(new List<ITarget>(), tagRouter, fallbackLogger);

            TraceListener traceListener = A.Fake<TraceListener>();
            Trace.Listeners.Add(traceListener);

            // Execute
            dispatcher.DispatchNow(new LogEvent {
                Message = "A doomed event."
            });

            // Verify
            A.CallTo(() => traceListener.TraceEvent(
                A<TraceEventCache>._,
                A<string>._,
                A<TraceEventType>.That.IsEqualTo(TraceEventType.Error),
                A<int>._,
                A<string>.That.Contains("Failure writing message to fallback logger for cause:"),
                A<object[]>._))
                    .MustHaveHappened(1, Times.Exactly);
        }

        /// <summary>
        /// Setup a fake target.
        /// </summary>
        protected static ITarget FakeTarget(string name) {
            var target = A.Fake<ITarget>();
            target.Name = name;
            return target;
        }

        /// <summary>
        /// Setup a fake router.
        /// </summary>
        protected static ITagRouter FakeTagRouter() {
            return A.Fake<ITagRouter>();
        }

        /// <summary>
        /// Get a new instance of the dispatcher under test.
        /// </summary>
        protected static IDispatcher GetDispatcher(IEnumerable<ITarget> targets, ITagRouter tagRouter, IFallbackLogger fallbackLogger = null) {
            return new StandardDispatcher(targets, tagRouter, fallbackLogger ?? A.Fake<IFallbackLogger>());
        }
    }

    /// <summary>
    /// A stub log event for testing the behavior of the dispatcher under test.
    /// </summary>
    internal class StubLogEvent : LogEvent {
        public bool Dispatched { get; set; }

        public int ManagedThreadId { get; set; }

        public override void WriteTo(ITarget target) {
            this.ManagedThreadId = Thread.CurrentThread.ManagedThreadId;

            target.Write(this);

            Dispatched = true;
        }
    }
}