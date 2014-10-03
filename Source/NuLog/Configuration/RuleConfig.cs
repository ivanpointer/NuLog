using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Configuration
{
    public class RuleConfig
    {
        public ICollection<string> Include { get; set; }
        public bool StrictInclude { get; set; }
        public ICollection<string> Exclude { get; set; }
        public ICollection<string> WriteTo { get; set; }
        public bool Final { get; set; }
    }
}
