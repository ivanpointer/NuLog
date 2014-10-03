using NuLog.Configuration.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Targets.Layouts
{
    public interface ILayout
    {
        void Initialize(LayoutConfig layoutConfig);

        string FormatLogEvent(LogEvent logEventInfo);
    }
}
