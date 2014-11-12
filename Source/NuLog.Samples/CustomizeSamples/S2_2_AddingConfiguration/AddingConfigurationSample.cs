/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/11/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using System;

namespace NuLog.Samples.CustomizeSamples.S2_2_AddingConfiguration
{
    /// <summary>
    /// An example illustarting a very simple implementaion of a custom target.  The narration
    /// of this example can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/2.2-Adding-Configuration
    /// </summary>
    public class AddingConfigurationSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public AddingConfigurationSample(string section, string sample) : base(section, sample) { }

        #endregion

        // Logging example
        public override void ExecuteSample(Arguments args)
        {
            LoggerFactory.Initialize("CustomizeSamples/S2_2_AddingConfiguration/NuLog.json");
            var logger = LoggerFactory.GetLogger();

            // Showcase our new color
            Console.Out.WriteLine("Before the color");
            logger.LogNow("I am striped with shock!", "consoleColorRed");
            logger.LogNow("I'm seeing stars!", "consoleColorBlue");
            Console.Out.WriteLine("Dull again...");
        }
    }
}
