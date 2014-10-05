using NuLog.Dispatch;
using NuLog.MetaData;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NuLog.Logger
{
    public abstract class LoggerBase
    {
        public ICollection<string> DefaultTags { get; private set; }
        private LogEventDispatcher Dispatcher { get; set; }

        public IMetaDataProvider MetaDataProvider { get; set; }

        public LoggerBase(LogEventDispatcher dispatcher)
        {
            Dispatcher = dispatcher;
            DefaultTags = new List<string>();
        }

        public LoggerBase(LogEventDispatcher dispatcher, ICollection<string> defaultTags)
        {
            Dispatcher = dispatcher;
            DefaultTags = defaultTags;
        }

        public void Log(LogEvent logEventInfo)
        {
            var stackFrame = Dispatcher.Debug
                ? new StackFrame(1)
                : null;

            Dispatcher.Enqueue(() =>
            {
                logEventInfo.LoggingStackFrame = stackFrame;
                logEventInfo.Tags = GetTags(logEventInfo.Tags, stackFrame, logEventInfo.Exception != null);
                Dispatcher.Log(logEventInfo);
            });
        }

        public void Log(string message, params string[] tags)
        {
            var stackFrame = Dispatcher.Debug
                ? new StackFrame(1)
                : null;

            Dispatcher.Enqueue(() => Dispatcher.Log(new LogEvent
            {
                Message = message,
                LoggingStackFrame = stackFrame,
                Tags = GetTags(tags, stackFrame),
                MetaData = GetMetaData()
            }));
        }

        public void Log(string message, IDictionary<string, object> metaData, params string[] tags)
        {
            var stackFrame = Dispatcher.Debug
                ? new StackFrame(1)
                : null;

            Dispatcher.Enqueue(() => Dispatcher.Log(new LogEvent
            {
                Message = message,
                LoggingStackFrame = stackFrame,
                MetaData = GetMetaData(metaData),
                Tags = GetTags(tags, stackFrame)
            }));
        }

        public void Log(string message, Exception exception, params string[] tags)
        {
            var stackFrame = new StackFrame(1);
            Dispatcher.Enqueue(() => Dispatcher.Log(new LogEvent
            {
                Message = message,
                LoggingStackFrame = stackFrame,
                MetaData = GetMetaData(),
                Exception = exception,
                Tags = GetTags(tags, stackFrame, true)
            }));
        }

        public void Log(string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            var stackFrame = new StackFrame(1);
            Dispatcher.Enqueue(() => Dispatcher.Log(new LogEvent
            {
                Message = message,
                LoggingStackFrame = stackFrame,
                Exception = exception,
                MetaData = GetMetaData(metaData),
                Tags = GetTags(tags, stackFrame, true)
            }));
        }

        public void LogNow(LogEvent logEventInfo)
        {
            var stackFrame = Dispatcher.Debug
                ? new StackFrame(1)
                : null;

            logEventInfo.LoggingStackFrame = stackFrame;
            logEventInfo.Tags = GetTags(logEventInfo.Tags, stackFrame, logEventInfo.Exception != null);
            Dispatcher.LogNow(logEventInfo);
        }

        public void LogNow(string message, params string[] tags)
        {
            var stackFrame = Dispatcher.Debug
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

        public void LogNow(string message, IDictionary<string, object> metaData, params string[] tags)
        {
            var stackFrame = Dispatcher.Debug
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

        #region Helpers

        private IDictionary<string, object> GetMetaData(IDictionary<string, object> mainMetaData = null)
        {
            var metaData = new Dictionary<string, object>();

            if (MetaDataProvider != null)
            {
                var providedMetaData = MetaDataProvider.ProvideMetaData();
                if (providedMetaData != null)
                    foreach (var key in providedMetaData.Keys)
                        metaData[key] = providedMetaData[key];
            }

            if (mainMetaData != null)
                foreach (var key in mainMetaData.Keys)
                    metaData[key] = mainMetaData[key];

            return metaData;
        }

        private ICollection<string> GetTags(IEnumerable<string> tags, StackFrame loggingStackFrame = null, bool hasException = false, params string[] extraTags)
        {
            var combinedTags = new List<string>();

            combinedTags.AddRange(DefaultTags);
            combinedTags.AddRange(tags);
            combinedTags.AddRange(extraTags);

            if (hasException && combinedTags.Contains("exception") == false)
                combinedTags.Add("exception");

            if (loggingStackFrame != null)
            {
                var method = loggingStackFrame.GetMethod();
                var stackFrameTags = new List<string> { method.DeclaringType.FullName, method.Name };
                combinedTags.AddRange(stackFrameTags.Where(_ => !combinedTags.Contains(_)));
            }

            return combinedTags;
        }

        #endregion

    }
}
