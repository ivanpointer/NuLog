/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.LogEvents;
using System;

namespace NuLog.Targets {

    public class ConsoleTarget : LayoutTargetBase {

        public override void Write(LogEvent logEvent) {
            var message = Layout.Format(logEvent);
            Console.Write(message);
        }
    }
}