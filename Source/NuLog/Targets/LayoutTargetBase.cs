using NuLog.Configuration.Layouts;
using NuLog.Configuration.Targets;
using NuLog.Targets.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Targets
{
    public abstract class LayoutTargetBase : TargetBase
    {
        private static readonly object _configLock = new object();
        public ILayout Layout { get; protected set; }

        public override void Initialize(TargetConfig targetConfig, bool? synchronous = null)
        {
            base.Initialize(targetConfig, synchronous);
            if (targetConfig != null)
            {
                lock (_configLock)
                {
                    LayoutConfig layoutConfig = null;
                    if (typeof(LayoutTargetConfig).IsAssignableFrom(targetConfig.GetType()))
                    {
                        layoutConfig = ((LayoutTargetConfig)targetConfig).LayoutConfig;
                    }
                    else
                    {
                        var layoutConfigToken = targetConfig.Config["layout"];
                        layoutConfig = new LayoutConfig(layoutConfigToken);
                    }

                    Layout = LayoutFactory.BuildLayout(layoutConfig);
                }
            }
            else
            {
                Name = "trace";
                Layout = new StandardLayout();
            }
        }
    }
}
