/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;

namespace NuLog.Samples.CustomizeSamples.S3_1_ExtendingTheLogger
{
    /// <summary>
    /// An example illustrating a very simple implementation of a custom target.  The narration
    /// of this example can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/3.1-Extending-the-Logger
    /// </summary>
    public class ExtendingTheLoggerSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public ExtendingTheLoggerSample(string section, string sample) : base(section, sample) { }

        #endregion Sample Wiring

        // Logging example
        public override void ExecuteSample()
        {
            // Load the configuration
            using (var factory = new LoggerFactory("CustomizeSamples/S3_1_ExtendingTheLogger/NuLog.json"))
            {
                var logger = factory.Logger();

                // Test the extension method
                logger.LogNow("I will be the default blue", "consoleColorBlue");
                logger.LogNow("I will be an overridden green", ConsoleColor.DarkGreen, ConsoleColor.White, "consoleColorBlue");
            }
        }
    }
}