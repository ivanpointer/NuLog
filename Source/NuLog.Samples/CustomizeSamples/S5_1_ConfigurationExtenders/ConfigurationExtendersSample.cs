/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/13/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NuLog.Samples.CustomizeSamples.S5_1_ConfigurationExtenders
{
    /// <summary>
    /// An example illustarting a very simple implementaion of a custom target.  The narration
    /// of this example can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/5.1-Implementing-a-Configuration-Extender
    /// </summary>
    public class ConfigurationExtendersSample : SampleBase
    {

        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public ConfigurationExtendersSample(string section, string sample) : base(section, sample) { }

        #endregion

        // Logging example
        public override void ExecuteSample()
        {
            // Load the configuration
            LoggerFactory.Initialize("CustomizeSamples/S5_1_ConfigurationExtenders/NuLog.json");

            // Execute our samples
            ExecuteJSONConfigSample();
            PauseSample();
            ExecuteMEFSample();
        }

        // Example showing setting a config extender in the JSON
        private void ExecuteJSONConfigSample()
        {
            // Get a hold of our logger
            var logger = LoggerFactory.GetLogger();

            // Test our configuration extender
            logger.LogNow("Hello, custom config!");
        }

        // Example showing setting a config extender using MEF
        private void ExecuteMEFSample()
        {
            // Get a hold of our logger
            var logger = LoggerFactory.GetLogger();

            // Test our configuration extender
            logger.LogNow("Hello, custom config using MEF!", "mefCustomConsole");
        }
    }
}
