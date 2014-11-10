/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/09/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Configuration;
using NuLog.Configuration.Targets;

namespace NuLog.Samples.Samples.S3_4_TextFileTarget
{
    /// <summary>
    /// An example showing the text file target in action.  The narration of this sample
    /// can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/3.4-Text-File-Target
    /// </summary>
    public class TextFileTargetSample : SampleBase
    {

        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public TextFileTargetSample(string section, string sample) : base(section, sample) { }

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
            LoggerFactory.Initialize("Samples/S3_4_TextFileTarget/NuLog.json");
            var jsonLogger = LoggerFactory.GetLogger();
            jsonLogger.LogNow("Hello from JSON config");
        }

        // Example using runtime configuration
        private void ExecuteRuntime()
        {
            int KB = 1024 ^ 2;

            var config = LoggingConfigBuilder.CreateLoggingConfig()
                .AddTarget(TextFileTargetConfigBuilder.Create()
                    .SetLayoutConfig("Runtime: ${Message}\r\n")

                    .SetFileName("sample.log")
                    .SetOldFileNamePattern("sample{0:.yyyy.MM.dd.hh.mm.ss}.log")

                    .SetRolloverPolicy(RolloverPolicy.Size)
                    .SetRolloverTrigger(1 * KB)

                    //.SetRolloverPolicy(RolloverPolicy.Day)
                    //.SetRolloverTrigger(2)

                    .SetOldFileLimit(5)
                    .SetCompressOldFiles(true)
                    .SetCompressionLevel(3)
                    .SetCompressionPassword("helloworld")
                    
                    .Build());

            LoggerFactory.Initialize(config);
            var runtimeLogger = LoggerFactory.GetLogger();
            runtimeLogger.LogNow("Hello from runtime config");
        }
    }
}
