/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NuLog.Tests {

    /// <summary>
    /// A dummy text writer, which stores the messages written to it, and records the console's
    /// foreground and background colors at the time of writing.
    /// </summary>
    public class DummyTextWriter : TextWriter {
        public List<ConsoleMessage> ConsoleMessages { get; private set; }

        public DummyTextWriter() {
            ConsoleMessages = new List<ConsoleMessage>();
        }

        public override Encoding Encoding {
            get {
                return Encoding.Default;
            }
        }

        public override void Write(string value) {
            var message = new ConsoleMessage {
                BackgroundColor = Console.BackgroundColor,
                ForegroundColor = Console.ForegroundColor,
                Message = value
            };
            ConsoleMessages.Add(message);
        }
    }
}