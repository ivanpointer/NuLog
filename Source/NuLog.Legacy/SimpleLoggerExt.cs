using NuLog.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Legacy
{
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

    public static class SimpleLoggerExt
    {
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

        public static void Trace(this LoggerBase simpleLogger, LogEvent logEventInfo)
        {
            LevelLog(simpleLogger, LogLevel.TRACE, logEventInfo);
        }

        public static void Trace(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.TRACE, message, tags);
        }

        public static void Trace(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.TRACE, message, metaData, tags);
        }

        public static void Trace(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.TRACE, message, exception, tags);
        }

        public static void Trace(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.TRACE, message, exception, metaData, tags);
        }

        public static void TraceNow(this LoggerBase simpleLogger, LogEvent logEventInfo)
        {
            LevelLogNow(simpleLogger, LogLevel.TRACE, logEventInfo);
        }

        public static void TraceNow(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.TRACE, message, tags);
        }

        public static void TraceNow(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.TRACE, message, metaData, tags);
        }

        public static void TraceNow(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.TRACE, message, exception, tags);
        }

        public static void TraceNow(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.TRACE, message, exception, metaData, tags);
        }

        #endregion

        #region Debug Functions

        public static void Debug(this LoggerBase simpleLogger, LogEvent logEventInfo)
        {
            LevelLog(simpleLogger, LogLevel.DEBUG, logEventInfo);
        }

        public static void Debug(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.DEBUG, message, tags);
        }

        public static void Debug(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.DEBUG, message, metaData, tags);
        }

        public static void Debug(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.DEBUG, message, exception, tags);
        }

        public static void Debug(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.DEBUG, message, exception, metaData, tags);
        }

        public static void DebugNow(this LoggerBase simpleLogger, LogEvent logEventInfo)
        {
            LevelLogNow(simpleLogger, LogLevel.DEBUG, logEventInfo);
        }

        public static void DebugNow(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.DEBUG, message, tags);
        }

        public static void DebugNow(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.DEBUG, message, metaData, tags);
        }

        public static void DebugNow(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.DEBUG, message, exception, tags);
        }

        public static void DebugNow(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.DEBUG, message, exception, metaData, tags);
        }

        #endregion

        #region Info Functions

        public static void Info(this LoggerBase simpleLogger, LogEvent logEventInfo)
        {
            LevelLog(simpleLogger, LogLevel.INFO, logEventInfo);
        }

        public static void Info(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.INFO, message, tags);
        }

        public static void Info(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.INFO, message, metaData, tags);
        }

        public static void Info(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.INFO, message, exception, tags);
        }

        public static void Info(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.INFO, message, exception, metaData, tags);
        }

        public static void InfoNow(this LoggerBase simpleLogger, LogEvent logEventInfo)
        {
            LevelLogNow(simpleLogger, LogLevel.INFO, logEventInfo);
        }

        public static void InfoNow(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.INFO, message, tags);
        }

        public static void InfoNow(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.INFO, message, metaData, tags);
        }

        public static void InfoNow(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.INFO, message, exception, tags);
        }

        public static void InfoNow(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.INFO, message, exception, metaData, tags);
        }

        #endregion

        #region Warn Functions

        public static void Warn(this LoggerBase simpleLogger, LogEvent logEventInfo)
        {
            LevelLog(simpleLogger, LogLevel.WARN, logEventInfo);
        }

        public static void Warn(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.WARN, message, tags);
        }

        public static void Warn(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.WARN, message, metaData, tags);
        }

        public static void Warn(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.WARN, message, exception, tags);
        }

        public static void Warn(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.WARN, message, exception, metaData, tags);
        }

        public static void WarnNow(this LoggerBase simpleLogger, LogEvent logEventInfo)
        {
            LevelLogNow(simpleLogger, LogLevel.WARN, logEventInfo);
        }

        public static void WarnNow(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.WARN, message, tags);
        }

        public static void WarnNow(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.WARN, message, metaData, tags);
        }

        public static void WarnNow(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.WARN, message, exception, tags);
        }

        public static void WarnNow(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.WARN, message, exception, metaData, tags);
        }

        #endregion

        #region Error Functions

        public static void Error(this LoggerBase simpleLogger, LogEvent logEventInfo)
        {
            LevelLog(simpleLogger, LogLevel.ERROR, logEventInfo);
        }

        public static void Error(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.ERROR, message, tags);
        }

        public static void Error(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.ERROR, message, metaData, tags);
        }

        public static void Error(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.ERROR, message, exception, tags);
        }

        public static void Error(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.ERROR, message, exception, metaData, tags);
        }

        public static void ErrorNow(this LoggerBase simpleLogger, LogEvent logEventInfo)
        {
            LevelLogNow(simpleLogger, LogLevel.ERROR, logEventInfo);
        }

        public static void ErrorNow(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.ERROR, message, tags);
        }

        public static void ErrorNow(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.ERROR, message, metaData, tags);
        }

        public static void ErrorNow(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.ERROR, message, exception, tags);
        }

        public static void ErrorNow(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.ERROR, message, exception, metaData, tags);
        }

        #endregion

        #region Fatal Functions

        public static void Fatal(this LoggerBase simpleLogger, LogEvent logEventInfo)
        {
            LevelLog(simpleLogger, LogLevel.FATAL, logEventInfo);
        }

        public static void Fatal(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.FATAL, message, tags);
        }

        public static void Fatal(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.FATAL, message, metaData, tags);
        }

        public static void Fatal(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.FATAL, message, exception, tags);
        }

        public static void Fatal(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLog(simpleLogger, LogLevel.FATAL, message, exception, metaData, tags);
        }

        public static void FatalNow(this LoggerBase simpleLogger, LogEvent logEventInfo)
        {
            LevelLogNow(simpleLogger, LogLevel.FATAL, logEventInfo);
        }

        public static void FatalNow(this LoggerBase simpleLogger, string message, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.FATAL, message, tags);
        }

        public static void FatalNow(this LoggerBase simpleLogger, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.FATAL, message, metaData, tags);
        }

        public static void FatalNow(this LoggerBase simpleLogger, string message, Exception exception, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.FATAL, message, exception, tags);
        }

        public static void FatalNow(this LoggerBase simpleLogger, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            LevelLogNow(simpleLogger, LogLevel.FATAL, message, exception, metaData, tags);
        }

        #endregion

        #region Generic Functions

        public static void LevelLog(this LoggerBase simpleLogger, LogLevel level, LogEvent logEventInfo)
        {
            AddLevelTag(logEventInfo, level);

            simpleLogger.Log(logEventInfo);
        }

        public static void LevelLog(this LoggerBase simpleLogger, LogLevel level, string message, params string[] tags)
        {
            tags = AddLevelTag(tags, level);

            simpleLogger.Log(message, tags);
        }

        public static void LevelLog(this LoggerBase simpleLogger, LogLevel level, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            tags = AddLevelTag(tags, level);

            simpleLogger.Log(message, metaData, tags);
        }

        public static void LevelLog(this LoggerBase simpleLogger, LogLevel level, string message, Exception exception, params string[] tags)
        {
            tags = AddLevelTag(tags, level);

            simpleLogger.Log(message, exception, tags);
        }

        public static void LevelLog(this LoggerBase simpleLogger, LogLevel level, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            tags = AddLevelTag(tags, level);

            simpleLogger.Log(message, exception, metaData, tags);
        }

        public static void LevelLogNow(this LoggerBase simpleLogger, LogLevel level, LogEvent logEventInfo)
        {
            AddLevelTag(logEventInfo, level);

            simpleLogger.LogNow(logEventInfo);
        }

        public static void LevelLogNow(this LoggerBase simpleLogger, LogLevel level, string message, params string[] tags)
        {
            tags = AddLevelTag(tags, level);

            simpleLogger.LogNow(message, tags);
        }

        public static void LevelLogNow(this LoggerBase simpleLogger, LogLevel level, string message, IDictionary<string, object> metaData, params string[] tags)
        {
            tags = AddLevelTag(tags, level);

            simpleLogger.LogNow(message, metaData, tags);
        }

        public static void LevelLogNow(this LoggerBase simpleLogger, LogLevel level, string message, Exception exception, params string[] tags)
        {
            tags = AddLevelTag(tags, level);

            simpleLogger.LogNow(message, exception, tags);
        }

        public static void LevelLogNow(this LoggerBase simpleLogger, LogLevel level, string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            tags = AddLevelTag(tags, level);

            simpleLogger.LogNow(message, exception, metaData, tags);
        }

        #endregion

        #region Helpers

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

        private static string[] AddLevelTag(string[] tags, LogLevel level)
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

        #endregion

    }
}
