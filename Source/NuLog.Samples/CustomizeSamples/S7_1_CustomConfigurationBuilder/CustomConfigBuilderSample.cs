/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/16/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.SamplesLib.CustomizeSamples.S7_1_CustomConfigurationBuilder;

namespace NuLog.Samples.CustomizeSamples.S7_1_CustomConfigurationBuilder
{
    /// <summary>
    /// An example illustrating a very simple implementation of a custom configuration builder.  The narration
    /// of this example can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/7.1-Creating-a-Custom-Configuration-Builder
    /// </summary>
    public class CustomConfigBuilderSample : SampleBase
    {
        // Wiring for the sample program (menu wiring)
        public CustomConfigBuilderSample(string section, string sample) : base(section, sample) { }

        // Logging example
        public override void ExecuteSample()
        {
            ExecuteManualBuilder();
            PauseSample();
            ExecuteMEFBuilder();
        }

        // Example showing a manually provided config builder
        private void ExecuteManualBuilder()
        {
            // Load the configuration using our custom config builder
            var factory = new LoggerFactory(new MyCustomConfigBuilder());

            // Execute our custom config builder
            var logger = factory.Logger();
            logger.LogNow("Custom config builder!");
        }

        // Example showing a MEF provided config builder
        private void ExecuteMEFBuilder()
        {
            // Load the default configuration
            var factory = new LoggerFactory();

            // Execute our example showing the MEF config builder
            var logger = factory.Logger();
            logger.LogNow("MEF config builder!", MEFCustomConfigBuilder.RuleTag);
        }
    }
}