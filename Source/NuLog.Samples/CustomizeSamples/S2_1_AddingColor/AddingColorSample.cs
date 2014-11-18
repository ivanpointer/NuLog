/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/11/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using System;

namespace NuLog.Samples.CustomizeSamples.S2_1_AddingColor
{
    /// <summary>
    /// An example illustarting a very simple implementaion of a custom target.  The narration
    /// of this example can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/2.1-Adding-Color
    /// </summary>
    public class AddingColorSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public AddingColorSample(string section, string sample) : base(section, sample) { }

        #endregion

        // Logging example
        public override void ExecuteSample()
        {
            LoggerFactory.Initialize("CustomizeSamples/S2_1_AddingColor/NuLog.json");
            var logger = LoggerFactory.GetLogger();

            // Showcase our new color
            Console.Out.WriteLine("Before the color");
            logger.LogNow("I am fantabulous!");
            Console.Out.WriteLine("Dull again...");
        }
    }
}
