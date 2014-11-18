/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/7/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Dispatch;
using NuLog.MetaData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NuLog
{
    /// <summary>
    /// Defines the basic, common functions for loggers.  The base logger.
    /// </summary>
    public abstract class LoggerBase
    {

        #region Constants

        /// <summary>
        /// The default tag used for exceptions
        /// </summary>
        public const string ExceptionTag = "exception";
        
        #endregion

        #region Methods and Constructors

        private LogEventDispatcher Dispatcher { get; set; }

        /// <summary>
        /// The default tags to be applied to all log events passing through the logger
        /// </summary>
        public ICollection<string> DefaultTags { get; private set; }

        /// <summary>
        /// A meta data provider for providing meta data for log events that pass through the logger
        /// </summary>
        public IMetaDataProvider MetaDataProvider { get; set; }
        
        /// <summary>
        /// Builds the standard base logger, attached to the given dispatcher
        /// </summary>
        /// <param name="dispatcher">The dispatcher attached to this logger</param>
        public LoggerBase(LogEventDispatcher dispatcher)
        {
            Dispatcher = dispatcher;
            DefaultTags = new List<string>();
        }

        /// <summary>
        /// Builds the standard base logger, attached to the given dispatcher, with the given default tags
        /// </summary>
        /// <param name="dispatcher">The dispatcher attached to this logger</param>
        /// <param name="defaultTags">The default tags assigned to this logger</param>
        public LoggerBase(LogEventDispatcher dispatcher, ICollection<string> defaultTags)
        {
            Dispatcher = dispatcher;
            DefaultTags = defaultTags;
        }

        #endregion

        #region Asynchronous Logging
        /* The strategy for asynchronous logging is to keep the load of generating
         * the log event to a minimum and to push as much work as is reasonably
         * possible off onto the dispatcher.  This is done by putting the efforts
         * of building the log event into an action which is queued into the
         * dispatcher.  The only bit that can't be put into the action is building
         * the stack frame for the log event.  This is minimized by having the
         * framework default to a "non-debug" mode, where the stack frame isn't
         * needed, making the stack frame "optional".
         * 
         * In a future release, the stack frame aquizition should be moved into
         * a separate function and wrapped in a compiler directive, so that
         * a "portable class library" can be built for the logging framework,
         * which is a future goal of the framework. */

        /// <summary>
        /// Logs the given log event, hinting to the framework that the event should be handled asynchronously
        /// </summary>
        /// <param name="logEventInfo">The log event to log</param>
        public void Log(LogEvent logEventInfo)
        {
            var stackFrame = Dispatcher.LoggingConfig.Debug
                ? new StackFrame(1)
                : null;

            Dispatcher.Log(() =>
            {
                logEventInfo.LoggingStackFrame = stackFrame;
                logEventInfo.Tags = GetTags(logEventInfo.Tags, stackFrame, logEventInfo.Exception != null);
                Dispatcher.Log(logEventInfo);
            });
        }

        /// <summary>
        /// Creates a log event with the given message and tags, and logs
        /// said log event, hinting to the framework that the event should
        /// be handled asynchronously
        /// </summary>
        /// <param name="message">The message for the log event</param>
        /// <param name="tags">The tags for the log event</param>
        public void Log(string message, params string[] tags)
        {
            var stackFrame = Dispatcher.LoggingConfig.Debug
                ? new StackFrame(1)
                : null;

            Dispatcher.Log(() => Dispatcher.Log(new LogEvent
            {
                Message = message,
                LoggingStackFrame = stackFrame,
                Tags = GetTags(tags, stackFrame),
                MetaData = GetMetaData()
            }));
        }

        /// <summary>
        /// Creates a log event using tyhe given message, meta data and tags
        /// and logs said log event, hinting to the framework that the event
        /// should be handled asynchronously
        /// </summary>
        /// <param name="message">The message for the log event</param>
        /// <param name="metaData">The meta data associated with the log event</param>
        /// <param name="tags">The tags assigned to the log event</param>
        public void Log(string message, IDictionary<string, object> metaData, params string[] tags)
        {
            var stackFrame = Dispatcher.LoggingConfig.Debug
                ? new StackFrame(1)
                : null;

            Dispatcher.Log(() => Dispatcher.Log(new LogEvent
            {
                Message = message,
                LoggingStackFrame = stackFrame,
                MetaData = GetMetaData(metaData),
                Tags = GetTags(tags, stackFrame)
            }));
        }

        /// <summary>
        /// Creates a log event with the given message, exception and tags
        /// and logs said log event, hinting to the framework that the event
        /// should be handled asynchronously
        /// </summary>
        /// <param name="message">The message for the log event</param>
        /// <param name="exception">The exception associated with the log event</param>
        /// <param name="tags">The tags assigned to the log event</param>
        public void Log(string message, Exception exception, params string[] tags)
        {
            // Stack frames are included by default for log events associated with an exception
            var stackFrame = new StackFrame(1);

            Dispatcher.Log(() => Dispatcher.Log(new LogEvent
            {
                Message = message,
                LoggingStackFrame = stackFrame,
                MetaData = GetMetaData(),
                Exception = exception,
                Tags = GetTags(tags, stackFrame, true)
            }));
        }

        /// <summary>
        /// Creates a log event with the given message, exception, meta data and tags
        /// and logs said log event, hinting to the framework that the event should
        /// be handled asynchronously
        /// </summary>
        /// <param name="message">The message for the log event</param>
        /// <param name="exception">The exception associated with the log event</param>
        /// <param name="metaData">The meta data associated with the log event</param>
        /// <param name="tags">The tags assigned to the log event</param>
        public void Log(string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            // Stack frames are included by default for log events associated with an exception
            var stackFrame = new StackFrame(1);
            Dispatcher.Log(() => Dispatcher.Log(new LogEvent
            {
                Message = message,
                LoggingStackFrame = stackFrame,
                Exception = exception,
                MetaData = GetMetaData(metaData),
                Tags = GetTags(tags, stackFrame, true)
            }));
        }

        #endregion

        #region Synchronous Logging

        /// <summary>
        /// Logs the given log event, hinting to the framework that the event should be handled synchronously
        /// </summary>
        /// <param name="logEventInfo">The log event to log</param>
        public void LogNow(LogEvent logEventInfo)
        {
            var stackFrame = Dispatcher.LoggingConfig.Debug
                ? new StackFrame(1)
                : null;

            logEventInfo.LoggingStackFrame = stackFrame;
            logEventInfo.Tags = GetTags(logEventInfo.Tags, stackFrame, logEventInfo.Exception != null);
            Dispatcher.LogNow(logEventInfo);
        }

        /// <summary>
        /// Creates a log event with the given message and tags and logs it, hinting to the
        /// framework that the event should be handled synchronously
        /// </summary>
        /// <param name="message">The message for the log event</param>
        /// <param name="tags">The tags assigned to the log event</param>
        public void LogNow(string message, params string[] tags)
        {
            var stackFrame = Dispatcher.LoggingConfig.Debug
                ? new StackFrame(1)
                : null;

            Dispatcher.LogNow(new LogEvent
            {
                Message = message,
                LoggingStackFrame = stackFrame,
                MetaData = GetMetaData(),
                Tags = GetTags(tags, stackFrame)
            });
        }

        /// <summary>
        /// Creates a log event with the given message, meta data and tags, and logs it, hinting to
        /// the framework that the log event should be handled synchronously
        /// </summary>
        /// <param name="message">The message for the log event</param>
        /// <param name="metaData">The meta data associated with the log event</param>
        /// <param name="tags">The tags assigned to the log event</param>
        public void LogNow(string message, IDictionary<string, object> metaData, params string[] tags)
        {
            var stackFrame = Dispatcher.LoggingConfig.Debug
                ? new StackFrame(1)
                : null;

            Dispatcher.LogNow(new LogEvent
            {
                Message = message,
                LoggingStackFrame = stackFrame,
                MetaData = GetMetaData(metaData),
                Tags = GetTags(tags, stackFrame)
            });
        }

        /// <summary>
        /// Creates a log event with the given message, exception and tags, and logs it, hinting to
        /// the framework that the log event should be handled synchronously
        /// </summary>
        /// <param name="message">The message for the log event</param>
        /// <param name="exception">The exception associated with the log event</param>
        /// <param name="tags">The tags assigned to the log event</param>
        public void LogNow(string message, Exception exception, params string[] tags)
        {
            var stackFrame = new StackFrame(1);
            Dispatcher.LogNow(new LogEvent
            {
                Message = message,
                LoggingStackFrame = stackFrame,
                Exception = exception,
                MetaData = GetMetaData(),
                Tags = GetTags(tags, stackFrame, true)
            });
        }

        /// <summary>
        /// Creates a log event with the given message, exception, meta data and tags, and logs it,
        /// hinting to the framework that the log event hould be handled synchronously
        /// </summary>
        /// <param name="message">The message for the log event</param>
        /// <param name="exception">The exception associated with the log event</param>
        /// <param name="metaData">The meta data associated with the log event</param>
        /// <param name="tags">The tags assigned to the log event</param>
        public void LogNow(string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            var stackFrame = new StackFrame(1);
            Dispatcher.LogNow(new LogEvent
            {
                Message = message,
                LoggingStackFrame = stackFrame,
                Exception = exception,
                MetaData = GetMetaData(metaData),
                Tags = GetTags(tags, stackFrame, true)
            });
        }

        #endregion

        #region Helpers

        // Retrieves the meta data from the meta data provider (if assigned), and merges it with the passed meta data, returning the congealed meta data
        private IDictionary<string, object> GetMetaData(IDictionary<string, object> mainMetaData = null)
        {
            var metaData = new Dictionary<string, object>();

            // Check the meta data provider for meta data
            if (MetaDataProvider != null)
            {
                var providedMetaData = MetaDataProvider.ProvideMetaData();
                if (providedMetaData != null)
                    foreach (var key in providedMetaData.Keys)
                        metaData[key] = providedMetaData[key];
            }

            // Merge any found meta data with the provided meta data
            if (mainMetaData != null)
                foreach (var key in mainMetaData.Keys)
                    metaData[key] = mainMetaData[key];

            // Return the congealed meta data
            return metaData;
        }

        // Builds a congealed list of tags for a log event using the provided tags, stack frame, exception information and extra tags
        private ICollection<string> GetTags(IEnumerable<string> tags, StackFrame loggingStackFrame = null, bool hasException = false, params string[] extraTags)
        {
            var combinedTags = new List<string>();

            // Bring together the know groups of tags
            combinedTags.AddRange(DefaultTags);
            combinedTags.AddRange(tags);
            combinedTags.AddRange(extraTags);

            // Add an exception tag if an exception is associated
            if (hasException && combinedTags.Contains(ExceptionTag) == false)
                combinedTags.Add(ExceptionTag);

            // Add stack frame information if it is provided
            if (loggingStackFrame != null)
            {
                var method = loggingStackFrame.GetMethod();
                var stackFrameTags = new List<string> { method.DeclaringType.FullName, method.Name };
                combinedTags.AddRange(stackFrameTags.Where(_ => !combinedTags.Contains(_)));
            }

            // Return the distinct list of combined tags
            return combinedTags.Distinct().ToArray();
        }

        #endregion

    }
}
