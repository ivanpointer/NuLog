﻿/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/12/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using System;

namespace NuLog.Samples.CustomizeSamples.S4_2_RuntimeMetaDataProviders
{
    /// <summary>
    /// An example illustarting a very simple implementaion of a custom target.  The narration
    /// of this example can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/4.2-Runtime-Meta-Data-Providers
    /// </summary>
    public class RuntimeMetaDataProvidersSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public RuntimeMetaDataProvidersSample(string section, string sample) : base(section, sample) { }

        #endregion Sample Wiring

        // Logging example
        public override void ExecuteSample()
        {
            // Load the configuration
            LoggerFactory.Initialize("CustomizeSamples/S4_2_RuntimeMetaDataProviders/NuLog.json");

            // Instanciate our runtime meta data provider
            var runtimeProvider = new RuntimeMetaDataProvider();

            // Get a hold of our logger
            var logger = LoggerFactory.GetLogger(runtimeProvider);

            // Log our information
            for (int lp = 0; lp < 5; lp++)
                logger.LogNow(String.Format("Runtime meta data provider test {0}", lp));
        }
    }
}