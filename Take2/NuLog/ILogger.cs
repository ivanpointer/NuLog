/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.Collections.Generic;

namespace NuLog
{
    /// <summary>
    /// Defines the expected behavior of a logger.
    /// </summary>
    public interface ILogger
    {
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
        void Log(string message, Exception exception, params string[] tags);

        /// <summary>
        /// Log a message, and don't return until it's been dispatched (fire and wait).
        /// </summary>
        void LogNow(string message, Exception exception, params string[] tags);

        /// <summary>
        /// Log a message, and return immediately, before the message has been dispatched (fire and forget).
        /// </summary>
        void Log(string message, Exception exception, Dictionary<string, object> metaData = null, params string[] tags);

        /// <summary>
        /// Log a message, and don't return until it's been dispatched (fire and wait).
        /// </summary>
        void LogNow(string message, Exception exception, Dictionary<string, object> metaData = null, params string[] tags);
    }
}