/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.Configuration.Targets;
using System.IO.Compression;

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
            var factory = new LoggerFactory("Samples/S3_4_TextFileTarget/NuLog.json");
            var jsonLogger = factory.Logger();
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

                    .SetOldFileLimit(5)
                    .SetCompressOldFiles(true)

                    .SetCompressionLevel(CompressionLevel.Optimal)
                    .SetCompressionPassword("helloworld")

                    .Build());

            var factory = new LoggerFactory(config);
            var runtimeLogger = factory.Logger();
            runtimeLogger.LogNow("Hello from runtime config");
        }
    }
}