/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

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

        #endregion Sample Wiring

        // Logging Example
        public override void ExecuteSample()
        {
            ExecuteJSON();

            ExecuteRuntime();
        }

        // Example using JSON configuration
        private void ExecuteJSON()
        {
            var factory = new LoggerFactory("Samples/S3_2_SimpleConsoleTarget/NuLog.json");
            var jsonLogger = factory.Logger();
            jsonLogger.LogNow("Hello from JSON config!");
        }

        // Example using runtime configuration
        private void ExecuteRuntime()
        {
            var consoleTargetConfig = new SimpleConsoleTargetConfig("console", "Runtime: ${Message}\r\n");
            var config = LoggingConfigBuilder.CreateLoggingConfig()
                .AddTarget(consoleTargetConfig);

            var factory = new LoggerFactory(config);
            var runtimeLogger = factory.Logger();
            runtimeLogger.LogNow("Hello from runtime config!");
        }
    }
}