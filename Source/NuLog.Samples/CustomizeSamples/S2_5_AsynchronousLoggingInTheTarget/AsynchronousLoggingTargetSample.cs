/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/11/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NuLog.Samples.CustomizeSamples.S2_5_AsynchronousLoggingInTheTarget
{
    /// <summary>
    /// An example illustarting a very simple implementaion of a custom target.  The narration
    /// of this example can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/2.5-Asynchronous-Logging-in-the-Target
    /// </summary>
    public class AsynchronousLoggingTargetSample : SampleBase
    {

        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public AsynchronousLoggingTargetSample(string section, string sample) : base(section, sample) { }

        #endregion

        // Logging example
        public override void ExecuteSample(Arguments args)
        {
            LoggerFactory.Initialize("CustomizeSamples/S2_5_AsynchronousLoggingInTheTarget/NuLog.json");
            var logger = LoggerFactory.GetLogger();

            var sw = new Stopwatch();

            // Showcase our asynchronous logging
            for (int lp = 0; lp < 100; lp++)
                logger.Log(String.Format("Message {0}", lp), lp % 2 == 0 ? "consoleColorBlue" : "consoleColorRed");
        }
    }
}
