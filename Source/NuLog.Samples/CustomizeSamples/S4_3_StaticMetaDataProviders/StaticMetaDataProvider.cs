/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/12/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.MetaData;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace NuLog.Samples.CustomizeSamples.S4_3_StaticMetaDataProviders
{
    /// <summary>
    /// Shows an example of a static meta data provider.  For the narration
    /// of this code, see the article:
    /// https://github.com/ivanpointer/NuLog/wiki/4.3-Static-Meta-Data-Providers
    /// </summary>
    public class StaticMetaDataProvider : IMetaDataProvider
    {
        #region Constants

        // Constants for our meta data keys
        public const string MachineNameMeta = "MachineName";
        public const string CPUUsageMeta = "CPUUsage";
        public const string RAMUsageMeta = "RAMUsage";

        #endregion

        // Members for providing as meta data
        private string _machineName;
        private PerformanceCounter _cpuCounter;
        private PerformanceCounter _ramCounter;

        // The default constructor, setting up what we need to provide the meta data
        public StaticMetaDataProvider()
        {
            _machineName = System.Environment.MachineName;

            _cpuCounter = new PerformanceCounter
            {
                CategoryName = "Processor",
                CounterName = "% Processor Time",
                InstanceName = "_Total"
            };

            _ramCounter = new PerformanceCounter()
            {
                CategoryName = "Memory",
                CounterName = "Available MBytes"
            };
        }

        // Our implementation for providing the meta data
        //  This meta data provides basic machine information
        public IDictionary<string, object> ProvideMetaData()
        {
            return new Dictionary<string, object>
            {
                { MachineNameMeta, _machineName },
                { CPUUsageMeta, _cpuCounter.NextValue() },
                { RAMUsageMeta, _ramCounter.NextValue() }
            };
        }
    }
}
