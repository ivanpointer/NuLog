/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.Collections.Generic;

namespace NuLog {

    /// <summary>
    /// Defines the expected behavior of a logger.
    /// </summary>
    public interface ILogger {

        /// <summary>
        /// When True, instructs the logger to include the stack frame in the generated log event.
        /// </summary>
        bool IncludeStackFrame { get; set; }

        /// <summary>
        /// Log a message, and return immediately, before the message has been dispatched (fire and forget).
        /// </summary>
        void Log(string message, params string[] tags);

        /// <summary>
        /// Log a message, and don't return until it's been dispatched (fire and wait).
        /// </summary>
        void LogNow(string message, params string[] tags);

        /// <summary>
        /// Log a message, and return immediately, before the message has been dispatched (fire and forget).
        /// </summary>
        void Log(string message, Dictionary<string, object> metaData = null, params string[] tags);

        /// <summary>
        /// Log a message, and don't return until it's been dispatched (fire and wait).
        /// </summary>
        void LogNow(string message, Dictionary<string, object> metaData = null, params string[] tags);

        /// <summary>
        /// Log a message, and return immediately, before the message has been dispatched (fire and forget).
        /// </summary>
        void Log(Exception exception, string message, params string[] tags);

        /// <summary>
        /// Log a message, and don't return until it's been dispatched (fire and wait).
        /// </summary>
        void LogNow(Exception exception, string message, params string[] tags);

        /// <summary>
        /// Log a message, and return immediately, before the message has been dispatched (fire and forget).
        /// </summary>
        void Log(Exception exception, string message, Dictionary<string, object> metaData = null, params string[] tags);

        /// <summary>
        /// Log a message, and don't return until it's been dispatched (fire and wait).
        /// </summary>
        void LogNow(Exception exception, string message, Dictionary<string, object> metaData = null, params string[] tags);
    }
}