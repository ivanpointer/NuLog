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

namespace NuLog.Samples.CustomizeSamples.S6_1_CustomLayout
{
    /// <summary>
    /// An example illustarting a very simple implementaion of a custom layout.  The narration
    /// of this example can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/6.1-Creating-a-Custom-Layout
    /// </summary>
    public class CustomLayoutSample : SampleBase
    {

        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public CustomLayoutSample(string section, string sample) : base(section, sample) { }

        #endregion

        // Logging example
        public override void ExecuteSample()
        {
            // Load the configuration
            LoggerFactory.Initialize("CustomizeSamples/S6_1_CustomLayout/NuLog.json");

            // Execute our samples
            var logger = LoggerFactory.GetLogger();
            logger.LogNow("Custom layout: JSON");
        }
    }
}
