/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

namespace NuLog.Samples.CustomizeSamples.S2_5_AsynchronousLoggingInTheTarget
{
    /// <summary>
    /// An example illustrating a very simple implementation of a custom target.  The narration
    /// of this example can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/2.5-Asynchronous-Logging-in-the-Target
    /// </summary>
    public class AsynchronousLoggingTargetSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public AsynchronousLoggingTargetSample(string section, string sample) : base(section, sample) { }

        #endregion Sample Wiring

        // Logging example
        public override void ExecuteSample()
        {
            // Load this sample's configuration
            var factory = new LoggerFactory("CustomizeSamples/S2_5_AsynchronousLoggingInTheTarget/NuLog.json");
            var logger = factory.Logger();

            // Showcase our asynchronous logging
            for (int lp = 0; lp < 100; lp++)
                logger.Log(string.Format("Message {0}", lp), lp % 2 == 0 ? "consoleColorBlue" : "consoleColorRed");
        }
    }
}