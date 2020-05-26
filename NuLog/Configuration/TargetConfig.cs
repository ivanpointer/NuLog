/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */
/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace NuLog.Configuration
{
    /// <summary>
    /// Represents a single target in the configuration.
    /// </summary>
    public class TargetConfig
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
        /// The internal instance of the properties.
        /// </summary>
        private IDictionary<string, object> _properties;

        /// <summary>
        /// Additional properties of the target.
        /// </summary>
        public IDictionary<string, object> Properties
        {
            get
            {
                return _properties;
            }
            set
            {
                // If we've got a null value, just assign it through
                if (value == null)
                {
                    _properties = null;
                    return;
                }

                // Take a copy of the dictionary as a thread-safe dictionary, and make it case insensitive.
                _properties = new ConcurrentDictionary<string, object>(value, StringComparer.InvariantCultureIgnoreCase);
            }
        }
    }
}