/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;
using System.Diagnostics;

namespace NuLog.Tests {

    /// <summary>
    /// A trace listener that simply dumps trace messages into a hash set.
    /// </summary>
    internal class HashSetTraceListener : TraceListener {
        public HashSet<string> Messages { get; private set; }

        public HashSetTraceListener() {
            Messages = new HashSet<string>();
        }

        public override void Write(string message) {
            Messages.Add(message);
        }

        public override void WriteLine(string message) {
            Messages.Add(message);
        }
    }
}