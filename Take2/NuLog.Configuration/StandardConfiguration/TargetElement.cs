/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.ComponentModel;
using System.Configuration;

namespace NuLog.Configuration.StandardConfiguration
{
    /// <summary>
    /// A target element of the standard config.
    /// </summary>
    public class TargetElement : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get
            {
                return base["name"] as string;
            }
        }

        [ConfigurationProperty("type"), TypeConverter(typeof(NuLogTypeNameConverter))]
        public Type Type
        {
            get
            {
                return (Type)base["type"];
            }
        }


        [ConfigurationProperty("properties")]
        public TargetPropertyCollection TargetProperties
        {
            get
            {
                return base["properties"] as TargetPropertyCollection;
            }
        }
    }
}