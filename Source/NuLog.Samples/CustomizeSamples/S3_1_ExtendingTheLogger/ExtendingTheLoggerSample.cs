/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/11/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NuLog.Samples.CustomizeSamples.S3_1_ExtendingTheLogger
{
    /// <summary>
    /// An example illustarting a very simple implementaion of a custom target.  The narration
    /// of this example can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/3.1-Extending-the-Logger
    /// </summary>
    public class ExtendingTheLoggerSample : SampleBase
    {

        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public ExtendingTheLoggerSample(string section, string sample) : base(section, sample) { }

        #endregion

        // Logging example
        public override void ExecuteSample(Arguments args)
        {
            // Load the configuration
            LoggerFactory.Initialize("CustomizeSamples/S3_1_ExtendingTheLogger/NuLog.json");
            var logger = LoggerFactory.GetLogger();

            // Test the extension method
            logger.LogNow("I will be the default blue", "consoleColorBlue");
            logger.LogNow("I will be an overriden green", ConsoleColor.DarkGreen, ConsoleColor.White, "consoleColorBlue");
        }
    }
}
