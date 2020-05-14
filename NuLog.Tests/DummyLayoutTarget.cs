/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.LogEvents;
using NuLog.Targets;
using System.Collections.Generic;

namespace NuLog.Tests {

    /// <summary>
    /// A dummy target to test the logger factory's ability to create new target instances.
    /// </summary>
    internal class DummyLayoutTarget : LayoutTargetBase {
        public int ConfigureCallCount { get; private set; }

        public ICollection<TargetConfig> ConfigsPassed { get; private set; }

        public ICollection<LogEvent> LogEventsPassed { get; set; }

        public DummyLayoutTarget() {
            this.ConfigsPassed = new List<TargetConfig>();

            this.LogEventsPassed = new List<LogEvent>();
        }

        /// <summary>
        /// Expose the protected layout in this layout target - to make sure it gets set during construction.
        /// </summary>
        public ILayout GetLayout() {
            return this.Layout;
        }

        public override void Configure(TargetConfig config) {
            ConfigureCallCount++;

            this.ConfigsPassed.Add(config);

            base.Configure(config);
        }

        public override void Write(LogEvent logEvent) {
            this.LogEventsPassed.Add(logEvent);
        }
    }
}