/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.LogEvents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace NuLog.Loggers {

    /// <summary>
    /// The standard logger.
    /// </summary>
    public class StandardLogger : ILogger {

        /// <summary>
        /// The tag to add when logging with an exception.
        /// </summary>
        private const string exceptionTag = "exception";

        /// <summary>
        /// The dispatcher this logger is to send log events to.
        /// </summary>
        protected readonly IDispatcher Dispatcher;

        /// <summary>
        /// The tag normalizer to use when processing tags.
        /// </summary>
        protected readonly ITagNormalizer TagNormalizer;

        /// <summary>
        /// The meta data provider for this logger.
        /// </summary>
        protected readonly IMetaDataProvider MetaDataProvider;

        /// <summary>
        /// Default tags to add to every log event originating from this logger.
        /// </summary>
        protected readonly ICollection<string> DefaultTags;

        /// <summary>
        /// Default meta data to add to every log event originating from this logger.
        /// </summary>
        protected readonly IDictionary<string, object> DefaultMetaData;

        /// <summary>
        /// When True, instructs the logger to include the stack frame in the generated log event.
        /// </summary>
        public bool IncludeStackFrame { get; set; }

        /// <summary>
        /// Sets up a new instance of the standard logger.
        /// </summary>
        public StandardLogger(IDispatcher dispatcher, ITagNormalizer tagNormalizer, IMetaDataProvider metaDataProvider, IEnumerable<string> defaultTags = null, IDictionary<string, object> defaultMetaData = null, bool includeStackFrame = false) {
            Dispatcher = dispatcher;

            MetaDataProvider = metaDataProvider;

            TagNormalizer = tagNormalizer;

            DefaultTags = tagNormalizer.NormalizeTags(defaultTags);

            DefaultMetaData = defaultMetaData;

            IncludeStackFrame = includeStackFrame;
        }

        public void Log(string message, params string[] tags) {
            Dispatcher.EnqueueForDispatch(BuildLogEvent(message, null, null, tags));
        }

        public void LogNow(string message, params string[] tags) {
            Dispatcher.DispatchNow(BuildLogEvent(message, null, null, tags));
        }

        public void Log(string message, Dictionary<string, object> metaData = null, params string[] tags) {
            Dispatcher.EnqueueForDispatch(BuildLogEvent(message, null, metaData, tags));
        }

        public void LogNow(string message, Dictionary<string, object> metaData = null, params string[] tags) {
            Dispatcher.DispatchNow(BuildLogEvent(message, null, metaData, tags));
        }

        public void Log(Exception exception, string message, params string[] tags) {
            Dispatcher.EnqueueForDispatch(BuildLogEvent(message, exception, null, tags));
        }

        public void LogNow(Exception exception, string message, params string[] tags) {
            Dispatcher.DispatchNow(BuildLogEvent(message, exception, null, tags));
        }

        public void Log(Exception exception, string message, Dictionary<string, object> metaData = null, params string[] tags) {
            Dispatcher.EnqueueForDispatch(BuildLogEvent(message, exception, metaData, tags));
        }

        public void LogNow(Exception exception, string message, Dictionary<string, object> metaData = null, params string[] tags) {
            Dispatcher.DispatchNow(BuildLogEvent(message, exception, metaData, tags));
        }

        /// <summary>
        /// Build and return a new log event, setting the default values on it.
        /// </summary>
        protected virtual LogEvent BuildLogEvent(string message, Exception exception, Dictionary<string, object> metaData, string[] tags) {
            return new LogEvent {
                Message = message,
                Exception = exception,
                Tags = GetTags(tags, exception != null),
                MetaData = GetMetaData(metaData),
                DateLogged = DateTime.UtcNow,
                Thread = Thread.CurrentThread,
                LoggingStackFrame = this.IncludeStackFrame ? new StackFrame(2, false) : null
            };
        }

        /// <summary>
        /// Mixes the given tags, with the default tags for this logger, and returns the mix.
        /// </summary>
        protected virtual ICollection<string> GetTags(ICollection<string> givenTags, bool hasException) {
            // Check if we need to tack on an exception tag
            ICollection<string> tags;

            // Figure out/calculate the tags to return. Call the normalizer for any tags that haven't
            // been run through yet.
            if (!HasTags(givenTags) && !HasTags(DefaultTags)) {
                tags = new List<string>();
            } else if (!HasTags(DefaultTags)) {
                tags = TagNormalizer.NormalizeTags(givenTags);
            } else if (!HasTags(givenTags)) {
                tags = new List<string>();
                foreach (var tag in DefaultTags) {
                    tags.Add(tag);
                }
            } else {
                tags = new List<string>();
                foreach (var tag in DefaultTags) {
                    tags.Add(tag);
                }
                foreach (var tag in givenTags) {
                    tags.Add(tag);
                }
                tags = TagNormalizer.NormalizeTags(tags);
            }

            // If we've got an exception, add on the exception tag
            if (hasException) {
                tags.Add(exceptionTag);
            }

            // Return the tags
            return tags;
        }

        /// <summary>
        /// Indicates if the given set of tags has any tags in it.
        /// </summary>
        protected static bool HasTags(IEnumerable<string> tags) {
            return tags != null && tags.Any();
        }

        /// <summary>
        /// Consults the meta data provider for this logger, if it has one, and combines the meta
        /// data from the provider, with the given meta data.
        ///
        /// Given meta data takes priority over meta data from a provider.
        /// </summary>
        protected IDictionary<string, object> GetMetaData(IDictionary<string, object> givenMetaData) {
            // Our own copy so that we don't modify the given.
            var metaData = new Dictionary<string, object>();

            // Start with any default meta data
            AddMetaData(DefaultMetaData, metaData);

            // Try to get the meta data from our provider
            var providedMetaData = MetaDataProvider != null
                ? MetaDataProvider.ProvideMetaData()
                : null;

            // Add the provided meta data
            AddMetaData(providedMetaData, metaData);

            // Add the given meta data
            AddMetaData(givenMetaData, metaData);

            // Return our new, merged copy
            return metaData;
        }

        /// <summary>
        /// Adds the source meta data to the target meta data.
        /// </summary>
        protected static void AddMetaData(IDictionary<string, object> sourceMetaData, IDictionary<string, object> targetMetaData) {
            if (sourceMetaData != null) {
                foreach (var item in sourceMetaData) {
                    targetMetaData[item.Key] = item.Value;
                }
            }
        }

        /// <summary>
        /// Determines if meta data exists in the given meta data.
        /// </summary>
        protected static bool HasMetaData(IDictionary<string, object> metaData) {
            return metaData != null && metaData.Count > 0;
        }
    }
}