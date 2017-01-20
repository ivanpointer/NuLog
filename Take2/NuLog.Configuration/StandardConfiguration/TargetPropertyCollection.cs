/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;
using System.Configuration;

namespace NuLog.Configuration.StandardConfiguration
{
    [ConfigurationCollection(typeof(TargetPropertyElement), CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class TargetPropertyCollection : ConfigurationElementCollection, IEnumerable<TargetPropertyElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TargetPropertyElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TargetPropertyElement)element).Name;
        }

        IEnumerator<TargetPropertyElement> IEnumerable<TargetPropertyElement>.GetEnumerator()
        {
            for (var lp = 0; lp < base.Count; lp++)
            {
                yield return base.BaseGet(lp) as TargetPropertyElement;
            }
        }
    }
}