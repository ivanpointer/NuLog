﻿/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.Collections.Generic;

namespace NuLog {

    /// <summary>
    /// Defines the expected behavior of a log event.
    /// </summary>
    public interface ILogEvent : IDisposable {

        /// <summary>
        /// The list of tags associated with the log event
        /// </summary>
        ICollection<string> Tags { get; set; }

        /// <summary>
        /// the log event is to write itself to the given target.
        ///
        /// This is pulled out this way to facilitate deferred processing of log events. For example,
        /// some CPU cycles may be saved if a log event isn't ever written to log.
        /// </summary>
        /// <param name="target">The target to write itself to.</param>
        void WriteTo(ITarget target);
    }
}