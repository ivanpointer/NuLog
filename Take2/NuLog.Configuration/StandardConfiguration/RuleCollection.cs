/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;
using System.Configuration;

namespace NuLog.Configuration.StandardConfiguration
{
    /// <summary>
    /// A configuration collection of rules.
    /// </summary>
    [ConfigurationCollection(typeof(RuleElement), AddItemName = "rule", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class RuleCollection : ConfigurationElementCollection, IEnumerable<RuleElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RuleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            // Well, we don't have a key.... The element can be the key itself!
            return element;
        }

        IEnumerator<RuleElement> IEnumerable<RuleElement>.GetEnumerator()
        {
            for (var lp = 0; lp < base.Count; lp++)
            {
                yield return base.BaseGet(lp) as RuleElement;
            }
        }
    }
}