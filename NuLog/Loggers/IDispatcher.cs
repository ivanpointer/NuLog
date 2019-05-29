/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;

namespace NuLog.Loggers {

    /// <summary>
    /// Defines the expected behavior of a log event dispatcher.
    ///
    /// Dispatchers are responsible for dispatching log events to a list of targets (presumably).
    /// </summary>
    public interface IDispatcher : IDisposable {

        /// <summary>
        /// Dispatch the given log event out to the log event targets, immediately. A call to
        /// </summary>
        /// <param name="logEvent">The log event to dispatch.</param>
        void DispatchNow(ILogEvent logEvent);

        /// <summary>
        /// Queues the given log event for later dispatch by the dispatcher.
        /// </summary>
        /// <param name="logEvent">The log event to queue for dispatch.</param>
        void EnqueueForDispatch(ILogEvent logEvent);
    }
}