﻿/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/11/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

namespace NuLog.Samples.CustomizeSamples.S1_3_MakingALayoutTarget
{
    /// <summary>
    /// An example illustarting a very simple implementaion of a custom target.  The narration
    /// of this example can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/1.2-A-Simple-Target
    /// </summary>
    public class MakingALayoutTargetSample : SampleBase
    {

        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public MakingALayoutTargetSample(string section, string sample) : base(section, sample) { }

        #endregion

        // Logging example
        public override void ExecuteSample(Arguments args)
        {
            LoggerFactory.Initialize("CustomizeSamples/S1_3_MakingALayoutTarget/NuLog.json");
            var logger = LoggerFactory.GetLogger();

            logger.LogNow("Our shiny new layout target!");
        }
    }
}