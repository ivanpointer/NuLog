/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.LogEvents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        /// The tag normalizer to use when processing tags.
        /// </summary>
        private readonly ITagNormalizer tagNormalizer;

        /// <summary>
        /// The meta data provider for this logger.
        /// </summary>
        private readonly IMetaDataProvider metaDataProvider;

        /// <summary>
        /// Default tags to add to every log event originating from this logger.
        /// </summary>
        private readonly IEnumerable<string> defaultTags;

        /// <summary>
        /// Default meta data to add to every log event originating from this logger.
        /// </summary>
        private readonly IDictionary<string, object> defaultMetaData;

        /// <summary>
        /// Sets up a new instance of the standard logger.
        /// </summary>
        public StandardLogger(IDispatcher dispatcher, ITagNormalizer tagNormalizer, IMetaDataProvider metaDataProvider, IEnumerable<string> defaultTags = null, IDictionary<string, object> defaultMetaData = null)
        {
            this.dispatcher = dispatcher;

            this.metaDataProvider = metaDataProvider;

            this.tagNormalizer = tagNormalizer;

            this.defaultTags = tagNormalizer.NormalizeTags(defaultTags);

            this.defaultMetaData = defaultMetaData;
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
        protected virtual LogEvent BuildLogEvent(string message, Exception exception, Dictionary<string, object> metaData, string[] tags)
        {
            return new LogEvent
            {
                Message = message,
                Exception = exception,
                Tags = GetTags(tags),
                MetaData = GetMetaData(metaData),
                DateLogged = DateTime.UtcNow,
                Thread = Thread.CurrentThread,
                LoggingStackFrame = new StackFrame(2)
            };
        }

        /// <summary>
        /// Mixes the given tags, with the default tags for this logger, and returns the mix.
        /// </summary>
        protected virtual IEnumerable<string> GetTags(IEnumerable<string> givenTags)
        {
            // Figure out/calculate the tags to return. Call the normalizer for any tags that haven't
            // been run through yet.
            if (!HasTags(givenTags) && !HasTags(this.defaultTags))
            {
                return null;
            }
            else if (!HasTags(this.defaultTags))
            {
                return this.tagNormalizer.NormalizeTags(givenTags);
            }
            else if (!HasTags(givenTags))
            {
                return this.defaultTags;
            }
            else
            {
                var tags = defaultTags.Concat(givenTags);
                return this.tagNormalizer.NormalizeTags(tags);
            }
        }

        /// <summary>
        /// Indicates if the given set of tags has any tags in it.
        /// </summary>
        private static bool HasTags(IEnumerable<string> tags)
        {
            return tags != null && tags.Count() > 0;
        }

        /// <summary>
        /// Consults the meta data provider for this logger, if it has one, and combines the meta
        /// data from the provider, with the given meta data.
        ///
        /// Given meta data takes priority over meta data from a provider.
        /// </summary>
        protected IDictionary<string, object> GetMetaData(IDictionary<string, object> givenMetaData)
        {
            // Our own copy so that we don't modify the given.
            var metaData = new Dictionary<string, object>();

            // Start with any default meta data
            AddMetaData(this.defaultMetaData, metaData);

            // Try to get the meta data from our provider
            var providedMetaData = this.metaDataProvider != null
                ? this.metaDataProvider.ProvideMetaData()
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
        private static void AddMetaData(IDictionary<string, object> sourceMetaData, IDictionary<string, object> targetMetaData)
        {
            if (sourceMetaData != null)
            {
                foreach (var item in sourceMetaData)
                {
                    targetMetaData[item.Key] = item.Value;
                }
            }
        }

        /// <summary>
        /// Determines if meta data exists in the given meta data.
        /// </summary>
        private static bool HasMetaData(IDictionary<string, object> metaData)
        {
            return metaData != null && metaData.Count > 0;
        }
    }
}