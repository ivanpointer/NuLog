using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog
{
    /// <summary>
    /// A set of constants used in the framework.
    /// These values are congegrated here, instead of hard-coding these
    /// values and sprinkling them throughout the rest of the code.
    /// </summary>
    public class ConstantsX
    {
        public class TraceLogger
        {
            public const string ConfigurationFailedUsingDefaultsMessage = "Configuration failed, using default configuration";
            public const string FailedConfigBuilderMessage = "Failed to configure using the provided config builder {0}";
            public const string NoTargetsUsingDefaultsMessage = "No targets configured, using default configuration";

            public const string ConfigCategory = "config";
        }
    }
}
