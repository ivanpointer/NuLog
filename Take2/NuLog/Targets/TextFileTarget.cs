/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.LogEvents;
using System.IO;

namespace NuLog.Targets {

    /// <summary>
    /// A target for writing log events to a text file.
    /// </summary>
    public class TextFileTarget : LayoutTargetBase {

        /// <summary>
        /// The path of the file to write to.
        /// </summary>
        private string filePath;

        public override void Write(LogEvent logEvent) {
            var message = Layout.Format(logEvent);
            File.AppendAllText(filePath, message);
        }

        public override void Configure(TargetConfig config) {
            filePath = GetProperty<string>(config, "path");

            // Let the base configure itself
            base.Configure(config);
        }
    }
}