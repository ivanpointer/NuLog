/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

namespace NuLog.Samples.CustomizeSamples.S4_2_RuntimeMetaDataProviders
{
    /// <summary>
    /// An example illustrating a very simple implementation of a custom target.  The narration
    /// of this example can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/4.2-Runtime-Meta-Data-Providers
    /// </summary>
    public class RuntimeMetaDataProvidersSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public RuntimeMetaDataProvidersSample(string section, string sample) : base(section, sample) { }

        #endregion Sample Wiring

        // Logging example
        public override void ExecuteSample()
        {
            // Load the configuration
            using (var factory = new LoggerFactory("CustomizeSamples/S4_2_RuntimeMetaDataProviders/NuLog.json"))
            {
                // Instantiate our runtime meta data provider
                var runtimeProvider = new RuntimeMetaDataProvider();

                // Get a hold of our logger
                var logger = factory.Logger(runtimeProvider);

                // Log our information
                for (int lp = 0; lp < 5; lp++)
                    logger.LogNow(string.Format("Runtime meta data provider test {0}", lp));
            }
        }
    }
}