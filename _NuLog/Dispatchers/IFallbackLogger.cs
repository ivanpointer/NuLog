/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;

namespace NuLog.Dispatchers {

    /// <summary>
    /// Defines the expected behavior of a fallback logger. The fallback logger is logged to when a
    /// failure occurs in one of the targets. The fallback logger needs to have internal error
    /// trapping mechanisms so that no further exceptions leak from the logging system.
    /// </summary>
    public interface IFallbackLogger {

        /// <summary>
        /// Logs an exception that occurred while writing to the given target.
        /// </summary>
        void Log(Exception exception, ITarget target, ILogEvent logEvent);

        /// <summary>
        /// Logs an arbitrary message. Will format the message, if arguments are given, otherwise,
        /// will just write it direct.
        /// </summary>
        void Log(string message, params object[] args);
    }
}