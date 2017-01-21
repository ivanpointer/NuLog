﻿/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;

namespace NuLog.Factories.Configuration
{
    /// <summary>
    /// The configuration needed by the factories to build out the parts of NuLog.
    /// </summary>
    public class Config
    {
        /// <summary>
        /// The rules within this configuration.
        /// </summary>
        public ICollection<ConfigRule> Rules { get; set; }

        /// <summary>
        /// The tag groups within this configuration.
        /// </summary>
        public ICollection<TagGroup> TagGroups { get; set; }
    }
}