/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/12/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NuLog.Samples.CustomizeSamples.S4_3_StaticMetaDataProviders
{
    /// <summary>
    /// An example illustarting a very simple implementaion of a custom target.  The narration
    /// of this example can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/4.3-Static-Meta-Data-Providers
    /// </summary>
    public class StaticMetaDataProvidersSample : SampleBase
    {

        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public StaticMetaDataProvidersSample(string section, string sample) : base(section, sample) { }

        #endregion

        // Logging example
        public override void ExecuteSample(Arguments args)
        {
            // Load the configuration
            LoggerFactory.Initialize("CustomizeSamples/S4_3_StaticMetaDataProviders/NuLog.json");

            // Get a hold of our logger
            var logger = LoggerFactory.GetLogger();

            // Log our information
            for (int lp = 0; lp < 5; lp++)
                logger.LogNow(String.Format("Static meta data provider test {0}", lp));
        }
    }
}
