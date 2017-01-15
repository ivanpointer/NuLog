/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.Collections.Generic;

namespace NuLog.Samples.CustomizeSamples.S2_4_ShuttingDownTheTarget
{
    /// <summary>
    /// An example illustrating a very simple implementation of a custom target.  The narration
    /// of this example can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/2.4-Shutting-Down-the-Target
    /// </summary>
    public class ShuttingDownTheTargetSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public ShuttingDownTheTargetSample(string section, string sample) : base(section, sample) { }

        #endregion Sample Wiring

        // Logging example
        public override void ExecuteSample()
        {
            using (var factory = new LoggerFactory("CustomizeSamples/S2_4_ShuttingDownTheTarget/NuLog.json"))
            {
                var logger = factory.Logger();

                // Showcase our new color
                Console.Out.WriteLine("Before the color");
                logger.LogNow("I am striped with shock!", "consoleColorRed");
                logger.LogNow("I'm seeing stars!", "consoleColorBlue");
                logger.LogNow("I'm feeling a bit green...", new Dictionary<string, object> {
                { ColorConsoleTarget.BackgroundColorMeta, ConsoleColor.White },
                { ColorConsoleTarget.ForegroundColorMeta, ConsoleColor.DarkGreen }
            }, "consoleColorRed");
                Console.Out.WriteLine("Dull again...");

                // Exiting the scope of the "using" statement will shutdown the factory - this will ripple to shutting down the target
            }
        }
    }
}