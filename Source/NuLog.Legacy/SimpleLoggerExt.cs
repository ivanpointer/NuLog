/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/10/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using System;
using System.Collections.Generic;

namespace NuLog.Legacy
{
    /// <summary>
    /// An enumeration representing the traditional log levels of legacy logging frameworks
    /// </summary>
    public enum LogLevel
    {
        UNINDICATED,
        TRACE,
        DEBUG,
        INFO,
        WARN,
        ERROR,
        FATAL
    }

    /// <summary>
    /// An extension class which adds the traditional log level methods to
    /// the logger
    /// </summary>
    public static class SimpleLoggerExt
    {
        // A map used to map the log level enumeration to a string tag
        private static readonly IDictionary<LogLevel, string> _levelTags = new Dictionary<LogLevel, string>
        {
            { LogLevel.UNINDICATED, "unindicated" },
            { LogLevel.TRACE, "trace" },
            { LogLevel.DEBUG, "debug" },
            { LogLevel.INFO, "info" },
            { LogLevel.WARN, "warn" },
            { LogLevel.ERROR, "error" },
            { LogLevel.FATAL, "fatal" }
        };

        #region Trace Functions

        /// <summary>
        /// Simulates the "trace" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="logEvent">The log event being logged</param>
        public static void Trace(this LoggerBase simpleLogger, LogEvent logEvent)
        {
            LevelLog(simpleLogger, LogLevel.TRACE, logEvent);
        }

        /// <summary>
        /// Simulates the "trace" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Trace(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.TRACE, message, tags);
        }

        /// <summary>
        /// Simulates the "trace" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Trace(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.TRACE, message, metaData, tags);
        }

        /// <summary>
        /// Simulates the "trace" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Trace(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.TRACE, message, exception, tags);
        }

        /// <summary>
        /// Simulates the "trace" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Trace(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.TRACE, message, exception, metaData, tags);
        }

        /// <summary>
        /// Simulates the "trace" log method of traditional logging frameworks.
        /// Logs the message synchronously.
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="logEvent">The log event being logged</param>
        public static void TraceNow(this LoggerBase simpleLogger, LogEvent logEvent)
        {
            LevelLogNow(simpleLogger, LogLevel.TRACE, logEvent);
        }

        /// <summary>
        /// Simulates the "trace" log method of traditional logging frameworks.
        /// Logs the message synchronously.
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void TraceNow(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.TRACE, message, tags);
        }

        /// <summary>
        /// Simulates the "trace" log method of traditional logging frameworks.
        /// Logs the message synchronously.
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void TraceNow(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.TRACE, message, metaData, tags);
        }

        /// <summary>
        /// Simulates the "trace" log method of traditional logging frameworks.
        /// Logs the message synchronously.
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void TraceNow(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.TRACE, message, exception, tags);
        }

        /// <summary>
        /// Simulates the "trace" log method of traditional logging frameworks.
        /// Logs the message synchronously.
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void TraceNow(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.TRACE, message, exception, metaData, tags);
        }

        #endregion Trace Functions

        #region Debug Functions

        /// <summary>
        /// Simulates the "debug" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="logEvent">The log event being logged</param>
        public static void Debug(this LoggerBase simpleLogger, LogEvent logEvent)
        {
            LevelLog(simpleLogger, LogLevel.DEBUG, logEvent);
        }

        /// <summary>
        /// Simulates the "debug" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Debug(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.DEBUG, message, tags);
        }

        /// <summary>
        /// Simulates the "debug" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Debug(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.DEBUG, message, metaData, tags);
        }

        /// <summary>
        /// Simulates the "debug" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Debug(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.DEBUG, message, exception, tags);
        }

        /// <summary>
        /// Simulates the "debug" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Debug(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.DEBUG, message, exception, metaData, tags);
        }

        /// <summary>
        /// Simulates the "debug" log method of traditional logging frameworks.
        /// Logs the message synchronously.
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="logEvent">The log event being logged</param>
        public static void DebugNow(this LoggerBase simpleLogger, LogEvent logEvent)
        {
            LevelLogNow(simpleLogger, LogLevel.DEBUG, logEvent);
        }

        /// <summary>
        /// Simulates the "debug" log method of traditional logging frameworks.
        /// Logs the message synchronously.
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void DebugNow(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.DEBUG, message, tags);
        }

        /// <summary>
        /// Simulates the "debug" log method of traditional logging frameworks.
        /// Logs the message synchronously.
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void DebugNow(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.DEBUG, message, metaData, tags);
        }

        /// <summary>
        /// Simulates the "debug" log method of traditional logging frameworks.
        /// Logs the message synchronously.
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void DebugNow(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.DEBUG, message, exception, tags);
        }

        /// <summary>
        /// Simulates the "debug" log method of traditional logging frameworks.
        /// Logs the message synchronously.
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void DebugNow(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.DEBUG, message, exception, metaData, tags);
        }

        #endregion Debug Functions

        #region Info Functions

        /// <summary>
        /// Simulates the "info" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="logEvent">The log event being logged</param>
        public static void Info(this LoggerBase simpleLogger, LogEvent logEvent)
        {
            LevelLog(simpleLogger, LogLevel.INFO, logEvent);
        }

        /// <summary>
        /// Simulates the "info" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Info(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.INFO, message, tags);
        }

        /// <summary>
        /// Simulates the "info" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Info(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.INFO, message, metaData, tags);
        }

        /// <summary>
        /// Simulates the "info" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Info(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.INFO, message, exception, tags);
        }

        /// <summary>
        /// Simulates the "info" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Info(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.INFO, message, exception, metaData, tags);
        }

        /// <summary>
        /// Simulates the "info" log method of traditional logging frameworks.
        /// Logs the message synchronously
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="logEvent">The log event being logged</param>
        public static void InfoNow(this LoggerBase simpleLogger, LogEvent logEvent)
        {
            LevelLogNow(simpleLogger, LogLevel.INFO, logEvent);
        }

        /// <summary>
        /// Simulates the "info" log method of traditional logging frameworks.
        /// Logs the message synchronously
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void InfoNow(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.INFO, message, tags);
        }

        /// <summary>
        /// Simulates the "info" log method of traditional logging frameworks.
        /// Logs the message synchronously
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void InfoNow(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.INFO, message, metaData, tags);
        }

        /// <summary>
        /// Simulates the "info" log method of traditional logging frameworks.
        /// Logs the message synchronously
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void InfoNow(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.INFO, message, exception, tags);
        }

        /// <summary>
        /// Simulates the "info" log method of traditional logging frameworks.
        /// Logs the message synchronously
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void InfoNow(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.INFO, message, exception, metaData, tags);
        }

        #endregion Info Functions

        #region Warn Functions

        /// <summary>
        /// Simulates the "warn" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="logEvent">The log event being logged</param>
        public static void Warn(this LoggerBase simpleLogger, LogEvent logEvent)
        {
            LevelLog(simpleLogger, LogLevel.WARN, logEvent);
        }

        /// <summary>
        /// Simulates the "warn" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Warn(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.WARN, message, tags);
        }

        /// <summary>
        /// Simulates the "warn" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Warn(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.WARN, message, metaData, tags);
        }

        /// <summary>
        /// Simulates the "warn" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Warn(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.WARN, message, exception, tags);
        }

        /// <summary>
        /// Simulates the "warn" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Warn(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.WARN, message, exception, metaData, tags);
        }

        /// <summary>
        /// Simulates the "warn" log method of traditional logging frameworks.
        /// Logs the message synchronously
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="logEvent">The log event being logged</param>
        public static void WarnNow(this LoggerBase simpleLogger, LogEvent logEvent)
        {
            LevelLogNow(simpleLogger, LogLevel.WARN, logEvent);
        }

        /// <summary>
        /// Simulates the "warn" log method of traditional logging frameworks.
        /// Logs the message synchronously
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void WarnNow(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.WARN, message, tags);
        }

        /// <summary>
        /// Simulates the "warn" log method of traditional logging frameworks.
        /// Logs the message synchronously
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void WarnNow(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.WARN, message, metaData, tags);
        }

        /// <summary>
        /// Simulates the "warn" log method of traditional logging frameworks.
        /// Logs the message synchronously
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void WarnNow(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.WARN, message, exception, tags);
        }

        /// <summary>
        /// Simulates the "warn" log method of traditional logging frameworks.
        /// Logs the message synchronously
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void WarnNow(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.WARN, message, exception, metaData, tags);
        }

        #endregion Warn Functions

        #region Error Functions

        /// <summary>
        /// Simulates the "error" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="logEvent">The log event being logged</param>
        public static void Error(this LoggerBase simpleLogger, LogEvent logEvent)
        {
            LevelLog(simpleLogger, LogLevel.ERROR, logEvent);
        }

        /// <summary>
        /// Simulates the "error" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Error(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.ERROR, message, tags);
        }

        /// <summary>
        /// Simulates the "error" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Error(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.ERROR, message, metaData, tags);
        }

        /// <summary>
        /// Simulates the "error" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Error(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.ERROR, message, exception, tags);
        }

        /// <summary>
        /// Simulates the "error" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Error(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.ERROR, message, exception, metaData, tags);
        }

        /// <summary>
        /// Simulates the "error" log method of traditional logging frameworks.
        /// Logs the message synchronously.
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="logEvent">The log event being logged</param>
        public static void ErrorNow(this LoggerBase simpleLogger, LogEvent logEvent)
        {
            LevelLogNow(simpleLogger, LogLevel.ERROR, logEvent);
        }

        /// <summary>
        /// Simulates the "error" log method of traditional logging frameworks.
        /// Logs the message synchronously.
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void ErrorNow(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.ERROR, message, tags);
        }

        /// <summary>
        /// Simulates the "error" log method of traditional logging frameworks.
        /// Logs the message synchronously.
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void ErrorNow(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.ERROR, message, metaData, tags);
        }

        /// <summary>
        /// Simulates the "error" log method of traditional logging frameworks.
        /// Logs the message synchronously.
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void ErrorNow(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.ERROR, message, exception, tags);
        }

        /// <summary>
        /// Simulates the "error" log method of traditional logging frameworks.
        /// Logs the message synchronously.
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void ErrorNow(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.ERROR, message, exception, metaData, tags);
        }

        #endregion Error Functions

        #region Fatal Functions

        /// <summary>
        /// Simulates the "fatal" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="logEvent">The log event being logged</param>
        public static void Fatal(this LoggerBase simpleLogger, LogEvent logEvent)
        {
            LevelLog(simpleLogger, LogLevel.FATAL, logEvent);
        }

        /// <summary>
        /// Simulates the "fatal" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Fatal(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.FATAL, message, tags);
        }

        /// <summary>
        /// Simulates the "fatal" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Fatal(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.FATAL, message, metaData, tags);
        }

        /// <summary>
        /// Simulates the "fatal" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Fatal(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.FATAL, message, exception, tags);
        }

        /// <summary>
        /// Simulates the "fatal" log method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void Fatal(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.FATAL, message, exception, metaData, tags);
        }

        /// <summary>
        /// Simulates the "fatal" log method of traditional logging frameworks.
        /// Logs the message synchronously
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="logEvent">The log event being logged</param>
        public static void FatalNow(this LoggerBase simpleLogger, LogEvent logEvent)
        {
            LevelLogNow(simpleLogger, LogLevel.FATAL, logEvent);
        }

        /// <summary>
        /// Simulates the "fatal" log method of traditional logging frameworks.
        /// Logs the message synchronously
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void FatalNow(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.FATAL, message, tags);
        }

        /// <summary>
        /// Simulates the "fatal" log method of traditional logging frameworks.
        /// Logs the message synchronously
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void FatalNow(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.FATAL, message, metaData, tags);
        }

        /// <summary>
        /// Simulates the "fatal" log method of traditional logging frameworks.
        /// Logs the message synchronously
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void FatalNow(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.FATAL, message, exception, tags);
        }

        /// <summary>
        /// Simulates the "fatal" log method of traditional logging frameworks.
        /// Logs the message synchronously
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void FatalNow(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.FATAL, message, exception, metaData, tags);
        }

        #endregion Fatal Functions

        #region Generic Functions

        /// <summary>
        /// Simulates the log level method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="level">The traditional/legacy log level to simulate</param>
        /// <param name="logEvent">The log event being logged</param>
        public static void LevelLog(this LoggerBase simpleLogger, LogLevel level, LogEvent logEvent)
        {
            AddLevelTag(logEvent, level);

            simpleLogger.Log(logEvent);
        }

        /// <summary>
        /// Simulates the log level method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="level">The traditional/legacy log level to simulate</param>
        /// <param name="message">The message being logged</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void LevelLog(this LoggerBase simpleLogger, LogLevel level, string message, params string[] tags)
        {
            tags = GetLevelTagList(tags, level);

            simpleLogger.Log(message, tags);
        }

        /// <summary>
        /// Simulates the log level method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="level">The traditional/legacy log level to simulate</param>
        /// <param name="message">The message being logged</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void LevelLog(this LoggerBase simpleLogger, LogLevel level, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            tags = GetLevelTagList(tags, level);

            simpleLogger.Log(message, metaData, tags);
        }

        /// <summary>
        /// Simulates the log level method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="level">The traditional/legacy log level to simulate</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void LevelLog(this LoggerBase simpleLogger, LogLevel level, string message, Exception exception, params string[] tags)
        {
            tags = GetLevelTagList(tags, level);

            simpleLogger.Log(message, exception, tags);
        }

        /// <summary>
        /// Simulates the log level method of traditional logging frameworks.
        /// Logs the message asynchronously, unless the synchronous override is set in the configuration
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="level">The traditional/legacy log level to simulate</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void LevelLog(this LoggerBase simpleLogger, LogLevel level, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            tags = GetLevelTagList(tags, level);

            simpleLogger.Log(message, exception, metaData, tags);
        }

        /// <summary>
        /// Simulates the log level method of traditional logging frameworks.
        /// Logs the message synchronously
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="level">The traditional/legacy log level to simulate</param>
        /// <param name="logEvent">The log event being logged</param>
        public static void LevelLogNow(this LoggerBase simpleLogger, LogLevel level, LogEvent logEvent)
        {
            AddLevelTag(logEvent, level);

            simpleLogger.LogNow(logEvent);
        }

        /// <summary>
        /// Simulates the log level method of traditional logging frameworks.
        /// Logs the message synchronously
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="level">The traditional/legacy log level to simulate</param>
        /// <param name="message">The message being logged</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void LevelLogNow(this LoggerBase simpleLogger, LogLevel level, string message, params string[] tags)
        {
            tags = GetLevelTagList(tags, level);

            simpleLogger.LogNow(message, tags);
        }

        /// <summary>
        /// Simulates the log level method of traditional logging frameworks.
        /// Logs the message synchronously
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="level">The traditional/legacy log level to simulate</param>
        /// <param name="message">The message being logged</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void LevelLogNow(this LoggerBase simpleLogger, LogLevel level, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            tags = GetLevelTagList(tags, level);

            simpleLogger.LogNow(message, metaData, tags);
        }

        /// <summary>
        /// Simulates the log level method of traditional logging frameworks.
        /// Logs the message synchronously
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="level">The traditional/legacy log level to simulate</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void LevelLogNow(this LoggerBase simpleLogger, LogLevel level, string message, Exception exception, params string[] tags)
        {
            tags = GetLevelTagList(tags, level);

            simpleLogger.LogNow(message, exception, tags);
        }

        /// <summary>
        /// Simulates the log level method of traditional logging frameworks.
        /// Logs the message synchronously
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="level">The traditional/legacy log level to simulate</param>
        /// <param name="message">The message being logged</param>
        /// <param name="exception">An exception to be assocaited with the log event</param>
        /// <param name="metaData">Meta data to be associated with the log event</param>
        /// <param name="tags">Any additinoal tags to associate with the log event</param>
        public static void LevelLogNow(this LoggerBase simpleLogger, LogLevel level, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            tags = GetLevelTagList(tags, level);

            simpleLogger.LogNow(message, exception, metaData, tags);
        }

        #endregion Generic Functions

        #region Helpers

        // Adds the appropraite tag to the log event to match the given log level
        private static void AddLevelTag(LogEvent logEvent, LogLevel level)
        {
            if (logEvent != null)
            {
                var levelTag = _levelTags[level];

                if (logEvent.Tags != null)
                    logEvent.Tags.Add(levelTag);
                else
                    logEvent.Tags = new List<string> { levelTag };
            }
        }

        // Adds the tag associated with the given level, to the given tag list and returns the updated list
        private static string[] GetLevelTagList(string[] tags, LogLevel level)
        {
            string levelTag = _levelTags[level];

            if (tags == null)
            {
                tags = (new List<string> { levelTag }).ToArray();
            }
            else
            {
                var tagList = new List<string>(tags);
                tagList.Add(levelTag);
                tags = tagList.ToArray();
            }

            return tags;
        }

        #endregion Helpers
    }
}