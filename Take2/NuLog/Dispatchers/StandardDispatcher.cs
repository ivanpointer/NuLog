/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Dispatchers.TagRouters;
using NuLog.Loggers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

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
        /// The fallback logger for recording exceptions that happen within the dispatcher and targets.
        /// </summary>
        private readonly IFallbackLogger fallbackLogger;

        /// <summary>
        /// A queue for storing log events to be dispatched.
        /// </summary>
        private readonly ConcurrentQueue<ILogEvent> logEventQueue;

        /// <summary>
        /// The timer for processing the log queue.
        /// </summary>
        private Timer logEventQueueTimer;

        /// <summary>
        /// Used to prevent new log events from being enqueued after disposal has been initiated.
        /// </summary>
        private bool isDisposing;

        /// <summary>
        /// Instantiates a new instance of this standard dispatcher, with the given tag router for
        /// determining which targets to send events to, based on their tags.
        /// </summary>
        public StandardDispatcher(IEnumerable<ITarget> targets, ITagRouter tagRouter, IFallbackLogger fallbackLogger)
        {
            this.targets = targets;

            this.tagRouter = tagRouter;

            this.fallbackLogger = fallbackLogger;

            this.logEventQueue = new ConcurrentQueue<ILogEvent>();

            this.logEventQueueTimer = new Timer(OnLogQueueTimerElapsed, this, 200, 200);
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
                    try
                    {
                        // We found it, tell the log event to write itself to the target
                        logEvent.WriteTo(target);
                    }
                    catch (Exception cause)
                    {
                        // There was a problem writing to the target, report the error
                        FallbackLog(cause, target, logEvent);
                    }
                }
            }
        }

        private void FallbackLog(Exception exception, ITarget target, ILogEvent logEvent)
        {
            try
            {
                this.fallbackLogger.Log(exception, target, logEvent);
            }
            catch (Exception cause)
            {
                Trace.TraceError("Failure writing exception to fallback logger: {0}\r\nOriginal Exception: {1}", cause, exception);
            }
        }

        public void EnqueueForDispatch(ILogEvent logEvent)
        {
            // Make sure that we're still accepting log events
            if (isDisposing)
            {
                throw new InvalidOperationException("This dispatcher has been disposed, and cannot enqueue new log events.");
            }

            // Add the log event to the queue
            logEventQueue.Enqueue(logEvent);
        }

        #region Disposal

        public void Dispose()
        {
            // Signal a true disposal
            Dispose(true);

            // Tell the GC that we've got it
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The bulk of the clean-up code is implemented in Dispose(bool)
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Signal that we're coming down
                isDisposing = true;

                // Dispose and null out the timer
                this.logEventQueueTimer.Dispose();
                this.logEventQueueTimer = null;
            }

            // Flush the queue
            ProcessLogQueue();

            // Dispose the targets
            foreach (var target in this.targets)
            {
                target.Dispose();
            }
        }

        #endregion Disposal

        #region LogEvent Queue Management

        /// <summary>
        /// Each "tick" of the log queue timer.
        /// </summary>
        private static void OnLogQueueTimerElapsed(object dispatcherInstance)
        {
            var dispatcher = dispatcherInstance as StandardDispatcher;
            dispatcher.ProcessLogQueue();
        }

        /// <summary>
        /// Works through the log queue, dispatching each one.
        /// </summary>
        protected void ProcessLogQueue()
        {
            ILogEvent logEvent;
            while (this.logEventQueue.TryDequeue(out logEvent))
            {
                DispatchNow(logEvent);
            }
        }

        #endregion LogEvent Queue Management
    }
}