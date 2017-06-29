/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog;
using NuLog.Configuration;
using NuLog.LogEvents;
using System.Diagnostics;

namespace NuLogSnippets.Docs.CustomTargets
{
    public class HelloWorldTarget : ITarget
    {
        public string Name { get; set; }

        public void Configure(TargetConfig config)
        {
            // Nothing to do
        }

        public void Dispose()
        {
            // Nothing to do
        }

        public void Write(LogEvent logEvent)
        {
            Debug.WriteLine(logEvent.Message);
        }
    }
}