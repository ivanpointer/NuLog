/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.Dispatchers;
using NuLog.Dispatchers.TagRouters;
using NuLog.Loggers;
using System.Collections.Generic;
using Xunit;

namespace NuLog.Tests.Unit.Dispatchers
{
    /// <summary>
    /// Tests to document the expected behavior of the standard dispatcher.
    /// </summary>
    [Trait("Category", "Unit")]
    public class StandardDispatcherTests
    {
        /// <summary>
        /// The dispatcher should ask the tag router about which targets to dispatch to.
        /// </summary>
        [Fact(DisplayName = "Should_ConsultTagRouter")]
        public void Should_ConsultTagRouter()
        {
            // Setup
            var logEvent = FakeLogEvent("my_tag");
            var target = FakeTarget("fake_target");
            var tagRouter = FakeTagRouter();
            var dispatcher = GetDispatcher(new ITarget[] { target }, tagRouter);

            // Have our fake router return our fake target's name, when given our fake log event's tags
            A.CallTo(() => tagRouter.Route(logEvent.Tags))
                .Returns(new string[] { target.Name });

            // Execute
            dispatcher.DispatchNow(logEvent);

            // Validate
            A.CallTo(() => logEvent.WriteTo(target)).MustHaveHappened();
        }

        /// <summary>
        /// Build a faked log event.
        /// </summary>
        protected static ILogEvent FakeLogEvent(params string[] tags)
        {
            var logEvent = A.Fake<ILogEvent>();
            logEvent.Tags = tags;
            return logEvent;
        }

        /// <summary>
        /// Setup a fake target.
        /// </summary>
        protected static ITarget FakeTarget(string name)
        {
            var target = A.Fake<ITarget>();
            target.Name = name;
            return target;
        }

        /// <summary>
        /// Setup a fake router.
        /// </summary>
        protected static ITagRouter FakeTagRouter()
        {
            return A.Fake<ITagRouter>();
        }

        protected static IDispatcher GetDispatcher(IEnumerable<ITarget> targets, ITagRouter tagRouter)
        {
            return new StandardDispatcher(targets, tagRouter);
        }
    }
}