/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/08/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Configuration;
using NuLog.Configuration.Targets;

namespace NuLog.Samples.Samples.S3_2_SimpleConsoleTarget
{
    /// <summary>
    /// An example showing the simple console target in action.  The narration of this
    ///   sample can be found at:
    ///   https://github.com/ivanpointer/NuLog/wiki/3.2-Simple-Console-Target
    /// </summary>
    public class SimpleConsoleTargetSample : SampleBase
    {

        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public SimpleConsoleTargetSample(string section, string sample) : base(section, sample) { }

        #endregion

        // Logging Example
        public override void ExecuteSample()
        {
            ExecuteJSON();

            ExecuteRuntime();
        }

        // Example using JSON configuration
        private void ExecuteJSON()
        {
            LoggerFactory.Initialize("Samples/S3_2_SimpleConsoleTarget/NuLog.json");
            var jsonLogger = LoggerFactory.GetLogger();
            jsonLogger.LogNow("Hello from JSON config!");
        }
        
        // Example using runtime configuration
        private void ExecuteRuntime()
        {
            var consoleTargetConfig = new SimpleConsoleTargetConfig("console", "Runtime: ${Message}\r\n");
            var config = LoggingConfigBuilder.CreateLoggingConfig()
                .AddTarget(consoleTargetConfig);

            LoggerFactory.Initialize(config);
            var runtimeLogger = LoggerFactory.GetLogger();
            runtimeLogger.LogNow("Hello from runtime config!");
        }
    }
}
