/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.ComponentModel;
using System.Configuration;

namespace NuLog.Configuration.StandardConfiguration
{
    /// <summary>
    /// A rule element of the standard config.
    /// </summary>
    public class RuleElement : ConfigurationElement
    {
        [ConfigurationProperty("include"), TypeConverter(typeof(TagListTypeConverter))]
        public string[] Include
        {
            get
            {
                return (string[])base["include"];
            }
        }

        [ConfigurationProperty("exclude"), TypeConverter(typeof(TagListTypeConverter))]
        public string[] Excludes
        {
            get
            {
                return (string[])base["exclude"];
            }
        }

        [ConfigurationProperty("targets"), TypeConverter(typeof(TargetNameTypeConverter))]
        public string[] Targets
        {
            get
            {
                return (string[])base["targets"];
            }
        }

        [ConfigurationProperty("strictInclude"), TypeConverter(typeof(BooleanConverter))]
        public bool StrictInclude
        {
            get
            {
                return (bool)base["strictInclude"];
            }
        }

        [ConfigurationProperty("final"), TypeConverter(typeof(BooleanConverter))]
        public bool Final
        {
            get
            {
                return (bool)base["final"];
            }
        }
    }
}