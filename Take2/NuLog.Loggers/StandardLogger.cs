/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace NuLog.Loggers
{
    /// <summary>
    /// The standard logger.
    /// </summary>
    public class StandardLogger : ILogger
    {
        /// <summary>
        /// The dispatcher this logger is to send log events to.
        /// </summary>
        private readonly IDispatcher dispatcher;

        /// <summary>
        /// Sets up a new instance of the standard logger.
        /// </summary>
        public StandardLogger(IDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public void Log(string message, params string[] tags)
        {
            dispatcher.EnqueueForDispatch(BuildLogEvent(message, null, null, tags));
        }

        public void LogNow(string message, params string[] tags)
        {
            dispatcher.DispatchNow(BuildLogEvent(message, null, null, tags));
        }

        public void Log(string message, Dictionary<string, object> metaData = null, params string[] tags)
        {
            dispatcher.EnqueueForDispatch(BuildLogEvent(message, null, metaData, tags));
        }

        public void LogNow(string message, Dictionary<string, object> metaData = null, params string[] tags)
        {
            dispatcher.DispatchNow(BuildLogEvent(message, null, metaData, tags));
        }

        public void Log(string message, Exception exception, params string[] tags)
        {
            dispatcher.EnqueueForDispatch(BuildLogEvent(message, exception, null, tags));
        }

        public void LogNow(string message, Exception exception, params string[] tags)
        {
            dispatcher.DispatchNow(BuildLogEvent(message, exception, null, tags));
        }

        public void Log(string message, Exception exception, Dictionary<string, object> metaData = null, params string[] tags)
        {
            dispatcher.EnqueueForDispatch(BuildLogEvent(message, exception, metaData, tags));
        }

        public void LogNow(string message, Exception exception, Dictionary<string, object> metaData = null, params string[] tags)
        {
            dispatcher.DispatchNow(BuildLogEvent(message, exception, metaData, tags));
        }

        /// <summary>
        /// Build and return a new log event, setting the default values on it.
        /// </summary>
        private LogEvent BuildLogEvent(string message, Exception exception, Dictionary<string, object> metaData, string[] tags)
        {
            return new LogEvent
            {
                Message = message,
                Exception = exception,
                Tags = tags,
                MetaData = metaData,
                DateLogged = DateTime.UtcNow,
                Thread = Thread.CurrentThread,
                LoggingStackFrame = new StackFrame(2)
            };
        }
    }
}