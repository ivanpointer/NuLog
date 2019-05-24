/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.IO;

namespace NuLog.FallbackLoggers {

    /// <summary>
    /// The standard implementation of the fallback logger.
    /// </summary>
    public class StandardFileFallbackLogger : StandardFallbackLoggerBase {

        /// <summary>
        /// The path of the file being logged to.
        /// </summary>
        private readonly string filePath;

        public StandardFileFallbackLogger(string filePath) {
            this.filePath = filePath;
        }

        public override void Log(Exception exception, ITarget target, ILogEvent logEvent) {
            var formatted = FormatMessage(exception, target, logEvent);
            File.AppendAllText(this.filePath, formatted);
        }

        public override void Log(string message, params object[] args) {
            var formatted = FormatMessage(message, args);
            File.AppendAllText(this.filePath, formatted);
        }
    }
}