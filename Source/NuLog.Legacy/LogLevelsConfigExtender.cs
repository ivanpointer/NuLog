/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/10/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Configuration;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace NuLog.Legacy
{
    /// <summary>
    /// A configuration extender that adds tag groups to the configuration to simulate the log levels of
    /// traditional logging frameworks
    /// </summary>
    [Export(typeof(ILoggingConfigExtender))]
    public class LogLevelsConfigExtender : ILoggingConfigExtender
    {
        // The collection of tag groups to add to the configuration
        private static readonly ICollection<TagGroupConfig> _tagGroupConfigs = new List<TagGroupConfig>
        {
            new TagGroupConfig("trace", "debug", "info", "warn", "error", "fatal"),
            new TagGroupConfig("debug", "info", "warn", "error", "fatal"),
            new TagGroupConfig("info", "warn", "error", "fatal"),
            new TagGroupConfig("warn", "error", "fatal"),
            new TagGroupConfig("error", "fatal")
        };

        /// <summary>
        /// Updates the given configuration, adding the traditional log levels as tag groups to the configuration
        /// </summary>
        /// <param name="loggingConfig">The logging config to update</param>
        public void UpdateConfig(LoggingConfig loggingConfig)
        {
            if (loggingConfig != null)
            {
                if (loggingConfig.TagGroups == null)
                    loggingConfig.TagGroups = new List<TagGroupConfig>();

                var missingTagGroups = _tagGroupConfigs.Where(nx => loggingConfig.TagGroups.Any(ox => ox.Tag == nx.Tag) == false);
                foreach (var missingTagGroup in missingTagGroups)
                    loggingConfig.TagGroups.Add(missingTagGroup);
            }
        }
    }
}