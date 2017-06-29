/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.LogEvents;
using NuLog.Targets;
using System.Diagnostics;

namespace NuLogSnippets.Docs.CustomTargets
{
    public class HelloLayoutTarget : LayoutTargetBase
    {
        public override void Write(LogEvent logEvent)
        {
            var formattedText = this.Layout.Format(logEvent);
            Debug.Write(formattedText);
        }
    }
}