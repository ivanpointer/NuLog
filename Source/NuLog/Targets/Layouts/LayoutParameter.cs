using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Targets.Layouts
{
    public class LayoutParameter
    {
        public bool StaticText { get; set; }
        public string Text { get; set; }
        public bool Contingent { get; set; }
        public string FullName { get; set; }
        public ICollection<string> NameList { get; set; }
        public string Format { get; set; }

        public LayoutParameter()
        {
            StaticText = false;
            NameList = new List<string>();
        }
    }
}
