/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

namespace NuLog.Samples.CustomizeSamples.S4_3_StaticMetaDataProviders
{
    /// <summary>
    /// An example illustrating a very simple implementation of a custom target.  The narration
    /// of this example can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/4.3-Static-Meta-Data-Providers
    /// </summary>
    public class StaticMetaDataProvidersSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public StaticMetaDataProvidersSample(string section, string sample) : base(section, sample) { }

        #endregion Sample Wiring

        // Logging example
        public override void ExecuteSample()
        {
            // Load the configuration
            using (var factory = new LoggerFactory("CustomizeSamples/S4_3_StaticMetaDataProviders/NuLog.json"))
            {
                // Get a hold of our logger
                var logger = factory.Logger();

                // Log our information
                for (int lp = 0; lp < 5; lp++)
                    logger.LogNow(string.Format("Static meta data provider test {0}", lp));
            }
        }
    }
}