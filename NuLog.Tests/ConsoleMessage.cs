/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;

namespace NuLog.Tests {

    /// <summary>
    /// Stores a console message, including the text, and the colors at the time of writing.
    /// </summary>
    public class ConsoleMessage {

        /// <summary>
        /// The background color at the time the message was written.
        /// </summary>
        public ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// The foreground color at the time the message was written.
        /// </summary>
        public ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// The message that was written.
        /// </summary>
        public string Message { get; set; }
    }
}