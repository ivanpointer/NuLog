/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.Configuration.Layouts;
using NuLog.Configuration.Targets;

namespace NuLog.Samples.Samples.S1_8_RuntimeConfigurationOverview
{
    /// <summary>
    /// An example showing how configuration can be performed at runtime, instead
    ///   of using a configuration file.  The narration of this sample can be found at:
    ///   https://github.com/ivanpointer/NuLog/wiki/1.8-Runtime-Configuration-Overview
    /// </summary>
    public class RuntimeConfigurationSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public RuntimeConfigurationSample(string section, string sample) : base(section, sample) { }

        #endregion Sample Wiring

        // Logging Example
        public override void ExecuteSample()
        {
            // Build out the config, a single console target with a dark blue background and white text
            //  all log events go to the console target
            var config = LoggingConfigBuilder.CreateLoggingConfig()
                .AddTarget(ConsoleTargetConfigBuilder.Create()
                    .SetLayoutConfig(new LayoutConfig("${Message}"))
                    .AddConsoleColorRule("DarkBlue", "White", "*")
                    .Build());

            // Initialize the framework to the configuration
            using (var factory = new LoggerFactory(config))
            {
                // Get an instance of the logger...
                var logger = factory.Logger();

                // and log to it
                logger.Log("Hello, World!");
            }
        }
    }
}