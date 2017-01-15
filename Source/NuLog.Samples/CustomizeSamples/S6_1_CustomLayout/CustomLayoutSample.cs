/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

namespace NuLog.Samples.CustomizeSamples.S6_1_CustomLayout
{
    /// <summary>
    /// An example illustrating a very simple implementation of a custom layout.  The narration
    /// of this example can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/6.1-Creating-a-Custom-Layout
    /// </summary>
    public class CustomLayoutSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public CustomLayoutSample(string section, string sample) : base(section, sample) { }

        #endregion Sample Wiring

        // Logging example
        public override void ExecuteSample()
        {
            // Load the configuration
            using (var factory = new LoggerFactory("CustomizeSamples/S6_1_CustomLayout/NuLog.json"))
            {
                // Execute our samples
                var logger = factory.Logger();
                logger.LogNow("Custom layout: JSON");
            }
        }
    }
}