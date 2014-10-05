using NuLog.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Legacy
{
    [Export(typeof(ILoggingConfigExtender))]
    public class LogLevelsConfigExtender : ILoggingConfigExtender
    {
        private static readonly ICollection<TagGroupConfig> _tagGroupConfigs = new List<TagGroupConfig>
        {
            new TagGroupConfig("trace", "debug", "info", "warn", "error", "fatal"),
            new TagGroupConfig("debug", "info", "warn", "error", "fatal"),
            new TagGroupConfig("info", "warn", "error", "fatal"),
            new TagGroupConfig("warn", "error", "fatal"),
            new TagGroupConfig("error", "fatal")
        };

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
