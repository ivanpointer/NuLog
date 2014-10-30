/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/28/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

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

        #endregion

        // Logging Example
        public override void ExecuteSample(Arguments args)
        {
            LoggerSample();

            PauseSample();

            GlobalSample();
        }

        private void LoggerSample()
        {
            // Load the configuration for the example of performing synchronous logging in the logger
            LoggerFactory.Initialize("Samples/S1_5_SynchronousLogging/NuLogLogger.json");
            LoggerBase logger = LoggerFactory.GetLogger();

            logger.Log("Log later");
            logger.LogNow("Log now");
        }

        private void GlobalSample()
        {
            // Load the configuration for the example of performing synchronous logging globally
            LoggerFactory.Initialize("Samples/S1_5_SynchronousLogging/NuLogGlobal.json");
            LoggerBase logger = LoggerFactory.GetLogger();

            logger.Log("Log later?");
            logger.LogNow("Log now!");
        }
    }
}
