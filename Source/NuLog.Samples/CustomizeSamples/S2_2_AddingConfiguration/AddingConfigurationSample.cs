/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;

namespace NuLog.Samples.CustomizeSamples.S2_2_AddingConfiguration
{
    /// <summary>
    /// An example illustrating a very simple implementation of a custom target.  The narration
    /// of this example can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/2.2-Adding-Configuration
    /// </summary>
    public class AddingConfigurationSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public AddingConfigurationSample(string section, string sample) : base(section, sample) { }

        #endregion Sample Wiring

        // Logging example
        public override void ExecuteSample()
        {
            var factory = new LoggerFactory("CustomizeSamples/S2_2_AddingConfiguration/NuLog.json");
            var logger = factory.Logger();

            // Showcase our new color
            Console.Out.WriteLine("Before the color");
            logger.LogNow("I am striped with shock!", "consoleColorRed");
            logger.LogNow("I'm seeing stars!", "consoleColorBlue");
            Console.Out.WriteLine("Dull again...");
        }
    }
}