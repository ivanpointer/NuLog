/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;
using System.Configuration;

namespace NuLog.Configuration.StandardConfiguration
{
    /// <summary>
    /// A configuration collection of tag groups.
    /// </summary>
    [ConfigurationCollection(typeof(TagGroupElement), AddItemName = "group", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class TagGroupCollection : ConfigurationElementCollection, IEnumerable<TagGroupElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TagGroupElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TagGroupElement)element).BaseTag;
        }

        IEnumerator<TagGroupElement> IEnumerable<TagGroupElement>.GetEnumerator()
        {
            for (var lp = 0; lp < base.Count; lp++)
            {
                yield return base.BaseGet(lp) as TagGroupElement;
            }
        }
    }
}