/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.LogEvents;
using NuLog.Targets;
using System.Diagnostics;

namespace NuLogSnippets.Docs.CustomTargets {

    public class HelloWorldShortTarget : TargetBase {

        public override void Write(LogEvent logEvent) {
            Debug.WriteLine(logEvent.Message);
        }
    }
}