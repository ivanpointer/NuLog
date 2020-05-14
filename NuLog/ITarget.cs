/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.LogEvents;
using System;

namespace NuLog {

    /// <summary>
    /// Defines the expected behavior of a target.
    /// </summary>
    public interface ITarget : IDisposable {

        /// <summary>
        /// The name of this target, which is used to identify this target in the various rules.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Tells the target to configure itself with the given config.
        /// </summary>
        void Configure(TargetConfig config);

        /// <summary>
        /// Write the given log event.
        /// </summary>
        void Write(LogEvent logEvent);
    }
}