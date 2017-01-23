/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.LogEvents;
using NuLog.Targets;
using System.Collections.Generic;

namespace NuLog.Tests
{
    /// <summary>
    /// A dummy target to test the logger factory's ability to create new target instances.
    /// </summary>
    internal class DummyTarget : TargetBase
    {
        public int ConfigureCallCount { get; private set; }

        public ICollection<TargetConfig> ConfigsPassed { get; private set; }

        public ICollection<LogEvent> LogEventsPassed { get; set; }

        public DummyTarget()
        {
            this.ConfigsPassed = new List<TargetConfig>();

            this.LogEventsPassed = new List<LogEvent>();
        }

        public override void Configure(TargetConfig config)
        {
            ConfigureCallCount++;

            this.ConfigsPassed.Add(config);

            base.Configure(config);
        }

        public override void Write(LogEvent logEvent)
        {
            this.LogEventsPassed.Add(logEvent);
        }
    }
}