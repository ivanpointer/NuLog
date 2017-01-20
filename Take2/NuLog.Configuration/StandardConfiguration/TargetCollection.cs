/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;
using System.Configuration;

namespace NuLog.Configuration.StandardConfiguration
{
    /// <summary>
    /// A configuration collection of targets.
    /// </summary>
    [ConfigurationCollection(typeof(RuleElement), AddItemName = "target", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class TargetCollection : ConfigurationElementCollection, IEnumerable<TargetElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TargetElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TargetElement)element).Name;
        }

        IEnumerator<TargetElement> IEnumerable<TargetElement>.GetEnumerator()
        {
            for (var lp = 0; lp < base.Count; lp++)
            {
                yield return base.BaseGet(lp) as TargetElement;
            }
        }
    }
}