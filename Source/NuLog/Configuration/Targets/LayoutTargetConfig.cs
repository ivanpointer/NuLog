using Newtonsoft.Json.Linq;
using NuLog.Configuration.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Configuration.Targets
{
    public class LayoutTargetConfig : TargetConfig
    {
        public LayoutConfig LayoutConfig { get; set; }

        public LayoutTargetConfig(bool synchronous = false)
            : base(synchronous)
        {
            LayoutConfig = new LayoutConfig();
        }

        public LayoutTargetConfig(JToken jToken, bool synchronous = false)
            : base(jToken, synchronous)
        {
            LayoutConfig = new LayoutConfig();

            if (jToken != null)
            {
                LayoutConfig = new LayoutConfig(jToken);
            }
        }
    }
}
