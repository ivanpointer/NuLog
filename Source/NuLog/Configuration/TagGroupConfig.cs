using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Configuration
{
    public class TagGroupConfig
    {
        public TagGroupConfig()
        {

        }

        public TagGroupConfig(string tag, params string[] childTags)
        {
            Tag = tag;
            ChildTags = childTags;
        }

        public string Tag { get; set; }
        public ICollection<string> ChildTags { get; set; }
    }
}
