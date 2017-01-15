using System;
using System.Collections.Generic;

namespace NuLog.Samples.CustomizeSamples.S3_1_ExtendingTheLogger
{
    /// <summary>
    /// Extends the LoggerBase with methods for overriding console colors.
    /// Uses meta data assigned to the log events to achieve this.
    /// </summary>
    public static class ColorConsoleLogerExt
    {
        /// <summary>
        /// Adds a background and foreground color meta data hook for the logger
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="logEvent">The log event to log</param>
        /// <param name="backgroundColor">The background color for the log event in the console</param>
        /// <param name="foregroundColor">The foreground color for the log event in the console</param>
        public static void Log(this LoggerBase simpleLogger, LogEvent logEvent, ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            AddColors(logEvent, backgroundColor, foregroundColor);

            simpleLogger.Log(logEvent);
        }

        /// <summary>
        /// Adds a background and foreground color meta data hook for the logger
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message to log</param>
        /// <param name="backgroundColor">The background color for the log event in the console</param>
        /// <param name="foregroundColor">The foreground color for the log event in the console</param>
        /// <param name="tags">Additional tags associated with the log event</param>
        public static void Log(this LoggerBase simpleLogger, string message, ConsoleColor backgroundColor, ConsoleColor foregroundColor, params string[] tags)
        {
            var metaData = AddColors(backgroundColor, foregroundColor);

            simpleLogger.Log(message, metaData, tags);
        }

        /// <summary>
        /// Adds a background and foreground color meta data hook for the logger
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message to log</param>
        /// <param name="backgroundColor">The background color for the log event in the console</param>
        /// <param name="foregroundColor">The foreground color for the log event in the console</param>
        /// <param name="metaData">The meta data to extend to include the colors for logging</param>
        /// <param name="tags">Additional tags associated with the log event</param>
        public static void Log(this LoggerBase simpleLogger, string message, ConsoleColor backgroundColor, ConsoleColor foregroundColor, IDictionary<string, object> metaData, params string[] tags)
        {
            metaData = AddColors(metaData, backgroundColor, foregroundColor);

            simpleLogger.Log(message, metaData, tags);
        }

        /// <summary>
        /// Adds a background and foreground color meta data hook for the logger
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message to log</param>
        /// <param name="exception">The exception to log</param>
        /// <param name="backgroundColor">The background color for the log event in the console</param>
        /// <param name="foregroundColor">The foreground color for the log event in the console</param>
        /// <param name="tags">Additional tags associated with the log event</param>
        public static void Log(this LoggerBase simpleLogger, string message, Exception exception, ConsoleColor backgroundColor, ConsoleColor foregroundColor, params string[] tags)
        {
            var metaData = AddColors(backgroundColor, foregroundColor);

            simpleLogger.Log(message, exception, metaData, tags);
        }

        /// <summary>
        /// Adds a background and foreground color meta data hook for the logger
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message to log</param>
        /// <param name="exception">The exception to log</param>
        /// <param name="backgroundColor">The background color for the log event in the console</param>
        /// <param name="foregroundColor">The foreground color for the log event in the console</param>
        /// <param name="metaData">The meta data to extend to include the colors for logging</param>
        /// <param name="tags">Additional tags associated with the log event</param>
        public static void Log(this LoggerBase simpleLogger, string message, Exception exception, ConsoleColor backgroundColor, ConsoleColor foregroundColor, IDictionary<string, object> metaData, params string[] tags)
        {
            metaData = AddColors(metaData, backgroundColor, foregroundColor);

            simpleLogger.Log(message, exception, metaData, tags);
        }

        /// <summary>
        /// Adds a background and foreground color meta data hook for the logger
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="logEvent">The log event to log</param>
        /// <param name="backgroundColor">The background color for the log event in the console</param>
        /// <param name="foregroundColor">The foreground color for the log event in the console</param>
        public static void LogNow(this LoggerBase simpleLogger, LogEvent logEvent, ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            AddColors(logEvent, backgroundColor, foregroundColor);

            simpleLogger.LogNow(logEvent);
        }

        /// <summary>
        /// Adds a background and foreground color meta data hook for the logger
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message to log</param>
        /// <param name="backgroundColor">The background color for the log event in the console</param>
        /// <param name="foregroundColor">The foreground color for the log event in the console</param>
        /// <param name="tags">Additional tags associated with the log event</param>
        public static void LogNow(this LoggerBase simpleLogger, string message, ConsoleColor backgroundColor, ConsoleColor foregroundColor, params string[] tags)
        {
            var metaData = AddColors(backgroundColor, foregroundColor);

            simpleLogger.LogNow(message, metaData, tags);
        }

        /// <summary>
        /// Adds a background and foreground color meta data hook for the logger
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message to log</param>
        /// <param name="backgroundColor">The background color for the log event in the console</param>
        /// <param name="foregroundColor">The foreground color for the log event in the console</param>
        /// <param name="metaData">The meta data to extend to include the colors for logging</param>
        /// <param name="tags">Additional tags associated with the log event</param>
        public static void LogNow(this LoggerBase simpleLogger, string message, ConsoleColor backgroundColor, ConsoleColor foregroundColor, IDictionary<string, object> metaData, params string[] tags)
        {
            metaData = AddColors(metaData, backgroundColor, foregroundColor);

            simpleLogger.LogNow(message, metaData, tags);
        }

        /// <summary>
        /// Adds a background and foreground color meta data hook for the logger
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message to log</param>
        /// <param name="exception">The exception to log</param>
        /// <param name="backgroundColor">The background color for the log event in the console</param>
        /// <param name="foregroundColor">The foreground color for the log event in the console</param>
        /// <param name="tags">Additional tags associated with the log event</param>
        public static void LogNow(this LoggerBase simpleLogger, string message, Exception exception, ConsoleColor backgroundColor, ConsoleColor foregroundColor, params string[] tags)
        {
            var metaData = AddColors(backgroundColor, foregroundColor);

            simpleLogger.LogNow(message, exception, metaData, tags);
        }

        /// <summary>
        /// Adds a background and foreground color meta data hook for the logger
        /// </summary>
        /// <param name="simpleLogger">The logger class being extended</param>
        /// <param name="message">The message to log</param>
        /// <param name="exception">The exception to log</param>
        /// <param name="backgroundColor">The background color for the log event in the console</param>
        /// <param name="foregroundColor">The foreground color for the log event in the console</param>
        /// <param name="metaData">The meta data to extend to include the colors for logging</param>
        /// <param name="tags">Additional tags associated with the log event</param>
        public static void LogNow(this LoggerBase simpleLogger, string message, Exception exception, ConsoleColor backgroundColor, ConsoleColor foregroundColor, IDictionary<string, object> metaData, params string[] tags)
        {
            metaData = AddColors(metaData, backgroundColor, foregroundColor);

            simpleLogger.LogNow(message, exception, metaData, tags);
        }

        #region Helpers

        /// <summary>
        /// Adds the meta data for the background and foreground color to the log event
        /// </summary>
        /// <param name="logEvent">The log event to log</param>
        /// <param name="backgroundColor">The background color for the log event in the console</param>
        /// <param name="foregroundColor">The foreground color for the log event in the console</param>
        public static void AddColors(LogEvent logEvent, ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            if (logEvent != null)
            {
                if (logEvent.MetaData == null)
                    logEvent.MetaData = new Dictionary<string, object>();

                logEvent.MetaData[ColorConsoleTarget.BackgroundColorMeta] = backgroundColor;
                logEvent.MetaData[ColorConsoleTarget.ForegroundColorMeta] = foregroundColor;
            }
        }

        /// <summary>
        /// Adds the meta data for the background and foreground color to the given meta data dictionary.  If the meta data provided is null, a new one is generated and returned.
        /// </summary>
        /// <param name="metaData">The meta data to extend to include the colors for logging</param>
        /// <param name="backgroundColor">The background color for the log event in the console</param>
        /// <param name="foregroundColor">The foreground color for the log event in the console</param>
        /// <returns>A dictionary containing the combined meta data, including the colors</returns>
        public static IDictionary<string, object> AddColors(IDictionary<string, object> metaData, ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            if (metaData == null)
                metaData = new Dictionary<string, object>();

            metaData[ColorConsoleTarget.BackgroundColorMeta] = backgroundColor;
            metaData[ColorConsoleTarget.ForegroundColorMeta] = foregroundColor;

            return metaData;
        }

        /// <summary>
        /// Creates a new meta data dictionary with entries for the given background and foreground colors
        /// </summary>
        /// <param name="backgroundColor">The background color for the log event in the console</param>
        /// <param name="foregroundColor">The foreground color for the log event in the console</param>
        /// <returns>A new dictionary containing meta data for the colors</returns>
        public static IDictionary<string, object> AddColors(ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            var metaData = new Dictionary<string, object>();

            metaData[ColorConsoleTarget.BackgroundColorMeta] = backgroundColor;
            metaData[ColorConsoleTarget.ForegroundColorMeta] = foregroundColor;

            return metaData;
        }

        #endregion Helpers
    }
}