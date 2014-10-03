using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Configuration
{
    public interface ILoggingConfigExtender
    {
        void UpdateConfig(LoggingConfig loggingConfig);
    }
}
