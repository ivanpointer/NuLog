/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;

namespace NuLog.Samples.CustomizeSamples.S2_1_AddingColor
{
    /// <summary>
    /// An example illustrating a very simple implementation of a custom target.  The narration
    /// of this example can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/2.1-Adding-Color
    /// </summary>
    public class AddingColorSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public AddingColorSample(string section, string sample) : base(section, sample) { }

        #endregion Sample Wiring

        // Logging example
        public override void ExecuteSample()
        {
            var factory = new LoggerFactory("CustomizeSamples/S2_1_AddingColor/NuLog.json");
            var logger = factory.Logger();

            // Showcase our new color
            Console.Out.WriteLine("Before the color");
            logger.LogNow("I am fantabulous!");
            Console.Out.WriteLine("Dull again...");
        }
    }
}