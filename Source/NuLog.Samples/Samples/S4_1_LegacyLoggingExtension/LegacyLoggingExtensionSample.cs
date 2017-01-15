/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Legacy;

namespace NuLog.Samples.Samples.S4_1_LegacyLoggingExtension
{
    /// <summary>
    /// An example showing the legacy logging in action.  The narration of this sample can be found
    /// at:
    /// https://github.com/ivanpointer/NuLog/wiki/4.1-Using-the-Legacy-Logging-Extension
    /// </summary>
    public class LegacyLoggingExtensionSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public LegacyLoggingExtensionSample(string section, string sample) : base(section, sample) { }

        #endregion Sample Wiring

        // Example using the legacy logging extension
        public override void ExecuteSample()
        {
            using (var factory = new LoggerFactory("Samples/S4_1_LegacyLoggingExtension/NuLog.json"))
            {
                var logger = factory.Logger();

                logger.TraceNow("These are not the droids you are looking for.");
                logger.DebugNow("Hey, who turned out the lights!?");

                logger.InfoNow("Well, ain't that quaint?");
                logger.WarnNow("Is it supposed to do that?");
                logger.ErrorNow("Earl, you broke it!");
                logger.FatalNow("We're all going to die!!");

                logger.LevelLogNow(LogLevel.INFO, "There is always another way!");
            }
        }
    }
}