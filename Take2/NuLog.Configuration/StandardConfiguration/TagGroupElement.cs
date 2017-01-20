/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.ComponentModel;
using System.Configuration;

namespace NuLog.Configuration.StandardConfiguration
{
    /// <summary>
    /// A tag group element of the standard config.
    /// </summary>
    public class TagGroupElement : ConfigurationElement
    {
        [ConfigurationProperty("baseTag", IsKey = true, IsRequired = true)]
        public string BaseTag
        {
            get
            {
                return base["baseTag"] as string;
            }
        }

        [ConfigurationProperty("aliases", IsRequired = true), TypeConverter(typeof(TargetNameTypeConverter))]
        public string[] Aliases
        {
            get
            {
                return (string[])base["aliases"];
            }
        }
    }
}