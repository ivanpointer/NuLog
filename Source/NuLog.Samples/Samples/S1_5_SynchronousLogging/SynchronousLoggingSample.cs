/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

namespace NuLog.Samples.Samples.S1_5_SynchronousLogging
{
    /// <summary>
    /// An example showing the different ways synchronous logging can be implemented
    ///   in the framework.  The narration of this sample can be found at:
    ///   https://github.com/ivanpointer/NuLog/wiki/1.5-Synchronous-Logging
    /// </summary>
    public class SynchronousLoggingSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public SynchronousLoggingSample(string section, string sample) : base(section, sample) { }

        #endregion Sample Wiring

        // Logging Example
        public override void ExecuteSample()
        {
            LoggerSample();

            PauseSample();

            GlobalSample();
        }

        private void LoggerSample()
        {
            // Load the configuration for the example of performing synchronous logging in the logger
            var factory = new LoggerFactory("Samples/S1_5_SynchronousLogging/NuLogLogger.json");
            var logger = factory.Logger();

            logger.Log("Log later");
            logger.LogNow("Log now");
        }

        private void GlobalSample()
        {
            // Load the configuration for the example of performing synchronous logging globally
            var factory = new LoggerFactory("Samples/S1_5_SynchronousLogging/NuLogGlobal.json");
            var logger = factory.Logger();

            logger.Log("Log later?");
            logger.LogNow("Log now!");
        }
    }
}