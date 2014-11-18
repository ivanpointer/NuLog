/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/11/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using System;
using System.Collections.Generic;

namespace NuLog.Samples.CustomizeSamples.S2_3_AddingMetaData
{
    /// <summary>
    /// An example illustarting a very simple implementaion of a custom target.  The narration
    /// of this example can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/2.3-Adding-Meta-Data
    /// </summary>
    public class AddingMetaDataSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public AddingMetaDataSample(string section, string sample) : base(section, sample) { }

        #endregion

        // Logging example
        public override void ExecuteSample()
        {
            LoggerFactory.Initialize("CustomizeSamples/S2_3_AddingMetaData/NuLog.json");
            var logger = LoggerFactory.GetLogger();

            // Showcase our new color
            Console.Out.WriteLine("Before the color");
            logger.LogNow("I am striped with shock!", "consoleColorRed");
            logger.LogNow("I'm seeing stars!", "consoleColorBlue");

            logger.LogNow("I'm feeling a bit green...", new Dictionary<string, object> {
                { ColorConsoleTarget.BackgroundColorMeta, ConsoleColor.White },
                { ColorConsoleTarget.ForegroundColorMeta, ConsoleColor.DarkGreen }
            }, "consoleColorRed");

            Console.Out.WriteLine("Dull again...");
        }
    }
}
