/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Dispatchers.TagRouters;
using NuLog.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NuLog.Dispatchers
{
    /// <summary>
    /// The standard dispatcher
    /// </summary>
    public class StandardDispatcher : IDispatcher
    {
        /// <summary>
        /// The list of targets being dispatched to.
        /// </summary>
        private readonly IEnumerable<ITarget> targets;

        /// <summary>
        /// The tag router to use to determine which targets to route events to, based on the events tags.
        /// </summary>
        private readonly ITagRouter tagRouter;

        /// <summary>
        /// Instantiates a new instance of this standard dispatcher, with the given tag router for
        /// determining which targets to send events to, based on their tags.
        /// </summary>
        public StandardDispatcher(IEnumerable<ITarget> targets, ITagRouter tagRouter)
        {
            this.targets = targets;

            this.tagRouter = tagRouter;
        }

        public void DispatchNow(ILogEvent logEvent)
        {
            // Ask our tag router which targets we send to
            var targetNames = tagRouter.Route(logEvent.Tags);

            // For each target to dispatch to, do it
            foreach (var targetName in targetNames)
            {
                // Try to find the target by name
                var target = targets.FirstOrDefault(t => string.Equals(targetName, t.Name, StringComparison.OrdinalIgnoreCase));
                if (target != null)
                {
                    // We found it, tell the log event to write itself to the target
                    logEvent.WriteTo(target);
                }
            }
        }

        public void Dispatch(ILogEvent logEvent)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            // Nothing to do (yet). Eventually, this will shut down timers, flush queues, etc.
        }
    }
}