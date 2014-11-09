/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/08/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Configuration;
using NuLog.Configuration.Targets;

namespace NuLog.Samples.Samples.S3_1_TraceTarget
{
    /// <summary>
    /// An example showing the trace target in action.  The narration of this
    ///   sample can be found at:
    ///   https://github.com/ivanpointer/NuLog/wiki/3.1-Trace-Target/_edit
    /// </summary>
    public class TraceTargetSample : SampleBase
    {

        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public TraceTargetSample(string section, string sample) : base(section, sample) { }

        #endregion

        // Logging Example
        public override void ExecuteSample(Arguments args)
        {
            ExecuteJSON();

            ExecuteRuntime();
        }

        // Example using JSON configuration
        private void ExecuteJSON()
        {
            LoggerFactory.Initialize("Samples/S3_1_TraceTarget/NuLog.json");
            var jsonLogger = LoggerFactory.GetLogger();
            jsonLogger.LogNow("Hello from JSON config!");
        }

        // Example using runtime configuration
        private void ExecuteRuntime()
        {
            var traceTargetConfig = new TraceTargetConfig("trace", "Runtime: ${Message}\r\n");
            var config = LoggingConfigBuilder.CreateLoggingConfig()
                .AddTarget(traceTargetConfig);

            LoggerFactory.Initialize(config);
            var runtimeLogger = LoggerFactory.GetLogger();
            runtimeLogger.LogNow("Hello from runtime config!");
        }
    }
}
