/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/09/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Configuration;
using NuLog.Configuration.Layouts;
using NuLog.Configuration.Targets;
using NuLog.Extensions.Console;
using NuLog.Targets;
using System;
using System.Collections.Generic;

namespace NuLog.Samples.Samples.S3_3_ConsoleTarget
{
    /// <summary>
    /// An example showing the console target in action.  The narration of this sample
    ///   can be found at:
    ///   https://github.com/ivanpointer/NuLog/wiki/3.3-Console-Target
    /// </summary>
    public class ConsoleTargetSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public ConsoleTargetSample(string section, string sample) : base(section, sample) { }

        #endregion Sample Wiring

        // Logging Example
        public override void ExecuteSample()
        {
            ExecuteJSON();

            ExecuteRuntime();

            ExecuteMetaData();
        }

        // Example using JSON configuration
        private void ExecuteJSON()
        {
            LoggerFactory.Initialize("Samples/S3_3_ConsoleTarget/NuLog.json");
            var jsonLogger = LoggerFactory.GetLogger();
            jsonLogger.LogNow("Hello from JSON config, error", "error");
            jsonLogger.LogNow("Hello from JSON config, warn", "warn");
        }

        // Example using runtime configuration
        private void ExecuteRuntime()
        {
            var config = LoggingConfigBuilder.CreateLoggingConfig()
                .AddTarget(ConsoleTargetConfigBuilder.Create()
                    .AddConsoleColorRule("White", "DarkRed", "error")
                    .AddConsoleColorRule(new ConsoleColorRule
                    {
                        Tags = new[] { "warn" },
                        ForegroundColor = ConsoleColor.White,
                        BackgroundColor = ConsoleColor.DarkYellow
                    })
                    .SetLayoutConfig(new LayoutConfig("Runtime: ${Message}\r\n"))
                    .Build());

            LoggerFactory.Initialize(config);
            var runtimeLogger = LoggerFactory.GetLogger();
            runtimeLogger.LogNow("Hello from runtime config, error", "error");
            runtimeLogger.LogNow("Hello from runtime config, warn", "warn");
        }

        // Example using meta data
        private void ExecuteMetaData()
        {
            var logger = LoggerFactory.GetLogger();

            logger.LogNow("White and dark yellow", new Dictionary<string, object>
            {
                { ConsoleTarget.MetaBackground, ConsoleColor.White },
                { ConsoleTarget.MetaForeground, ConsoleColor.DarkYellow }
            });

            logger.LogNow("White and dark green", ConsoleColor.White, ConsoleColor.DarkGreen);
        }
    }
}