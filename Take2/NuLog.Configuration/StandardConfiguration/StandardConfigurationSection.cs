/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Configuration;

namespace NuLog.Configuration.StandardConfiguration
{
    /// <summary>
    /// The standard configuration section.
    /// </summary>
    public class StandardConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("rules")]
        public RuleCollection Rules
        {
            get
            {
                return base["rules"] as RuleCollection;
            }
        }
    }
}