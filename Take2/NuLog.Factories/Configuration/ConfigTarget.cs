﻿/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;

namespace NuLog.Factories.Configuration
{
    /// <summary>
    /// Represents a single target in the configuration.
    /// </summary>
    public class ConfigTarget
    {
        /// <summary>
        /// The name of the target; used to identify the target in the rules.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of the target; identifies to the factory which target to instantiate.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Additional properties of the target.
        /// </summary>
        public IDictionary<string, string> Properties { get; set; }
    }
}