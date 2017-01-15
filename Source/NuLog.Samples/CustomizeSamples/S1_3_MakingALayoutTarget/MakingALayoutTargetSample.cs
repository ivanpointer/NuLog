/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

namespace NuLog.Samples.CustomizeSamples.S1_3_MakingALayoutTarget
{
    /// <summary>
    /// An example illustrating a very simple implementation of a custom target.  The narration
    /// of this example can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/1.2-A-Simple-Target
    /// </summary>
    public class MakingALayoutTargetSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public MakingALayoutTargetSample(string section, string sample) : base(section, sample) { }

        #endregion Sample Wiring

        // Logging example
        public override void ExecuteSample()
        {
            var factory = new LoggerFactory("CustomizeSamples/S1_3_MakingALayoutTarget/NuLog.json");
            var logger = factory.Logger();

            logger.LogNow("Our shiny new layout target!");
        }
    }
}