/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.LogEvents;
using System;

namespace NuLog.Targets {

    /// <summary>
    /// A target that writes to console, and can have custom colors.
    /// </summary>
    public class ColorConsoleTarget : LayoutTargetBase {

        /// <summary>
        /// Lock to keep colors straight during threaded writes.
        /// </summary>
        private static readonly object consoleLock = new object();

        /// <summary>
        /// Cache the console color type, for lookup.
        /// </summary>
        private static readonly Type consoleColorType = typeof(ConsoleColor);

        /// <summary>
        /// The background color to use.
        /// </summary>
        private ConsoleColor backgroundColor;

        /// <summary>
        /// The foreground color to use.
        /// </summary>
        private ConsoleColor foregroundColor;

        public ColorConsoleTarget() : base() {
            this.backgroundColor = Console.BackgroundColor;
            this.foregroundColor = Console.ForegroundColor;
        }

        public override void Write(LogEvent logEvent) {
            var message = Layout.Format(logEvent);

            lock (consoleLock) {
                Console.BackgroundColor = backgroundColor;
                Console.ForegroundColor = foregroundColor;
                Console.Write(message);
                Console.ResetColor();
            }
        }

        public override void Configure(TargetConfig config) {
            // Check for a background color in the config
            var bgColorName = GetProperty<string>(config, "background");
            if (!string.IsNullOrEmpty(bgColorName)) {
                backgroundColor = (ConsoleColor)Enum.Parse(consoleColorType, bgColorName);
            }

            // Check for a foreground color in the config
            var fgColorName = GetProperty<string>(config, "foreground");
            if (!string.IsNullOrEmpty(fgColorName)) {
                foregroundColor = (ConsoleColor)Enum.Parse(consoleColorType, fgColorName);
            }

            // Let the base do its config
            base.Configure(config);
        }
    }
}