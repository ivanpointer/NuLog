/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.LogEvents;
using System;

namespace NuLog.CLI.Benchmarking {

    /// <summary>
    /// A dummy target for performance testing - we need to see the raw results of the engine, not
    /// the individual target.
    /// </summary>
    public class DummyTarget : ITarget {
        public string Name { get; set; }

        public void Configure(TargetConfig config) {
            // noop
        }

        public void Dispose() {
            // noop
        }

        public void Write(LogEvent logEvent) {
            // noop
        }
    }
}