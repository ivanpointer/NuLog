/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

namespace NuLog.Samples.CustomizeSamples.S5_1_ConfigurationExtenders
{
    /// <summary>
    /// An example illustrating a very simple implementation of a custom target.  The narration
    /// of this example can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/5.1-Implementing-a-Configuration-Extender
    /// </summary>
    public class ConfigurationExtendersSample : SampleBase
    {
        // Wiring for the sample program (menu wiring)
        public ConfigurationExtendersSample(string section, string sample) : base(section, sample)
        {
        }

        // Logging example
        public override void ExecuteSample()
        {
            using (var factory = new LoggerFactory("CustomizeSamples/S5_1_ConfigurationExtenders/NuLog.json"))
            {
                var logger = factory.Logger();

                // Execute our samples
                ExecuteJSONConfigSample(logger);
                PauseSample();
                ExecuteMEFSample(logger);
            }
        }

        // Example showing setting a config extender in the JSON
        private void ExecuteJSONConfigSample(LoggerBase logger)
        {
            // Test our configuration extender
            logger.LogNow("Hello, custom config!");
        }

        // Example showing setting a config extender using MEF
        private void ExecuteMEFSample(LoggerBase logger)
        {
            // Test our configuration extender
            logger.LogNow("Hello, custom config using MEF!", "mefCustomConsole");
        }
    }
}