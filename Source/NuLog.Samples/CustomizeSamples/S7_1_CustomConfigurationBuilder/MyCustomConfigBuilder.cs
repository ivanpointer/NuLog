/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.Configuration.Targets;
using System.Configuration;

namespace NuLog.Samples.CustomizeSamples.S7_1_CustomConfigurationBuilder
{
    /// <summary>
    /// A custom config builder that creates a simple config with a single console target that all log events go to.
    /// Colors for the console target are loaded from the app config.
    /// To see the narration for this sample code, see the article:
    /// https://github.com/ivanpointer/NuLog/wiki/7.1-Creating-a-Custom-Configuration-Builder
    /// </summary>
    public class MyCustomConfigBuilder : ILoggingConfigBuilder
    {
        #region Constants

        public const string BackgroundColorKey = "S7_1_ConsoleBackground";
        public const string ForegroundColorKey = "S7_1_ConsoleForeground";

        public const string AllTags = "*";

        #endregion Constants

        /// <summary>
        /// Builds our custom configuration, pulling the console colors from the app config
        /// </summary>
        /// <returns>Our custom configuration</returns>
        public LoggingConfig Build()
        {
            // Get the string representation of the colors from our app settings
            string backgroundColorString = ConfigurationManager.AppSettings[BackgroundColorKey];
            string foregroundColorString = ConfigurationManager.AppSettings[ForegroundColorKey];

            // Build our config with a single console target, colors from app config, all log events go to this
            //  Runtime config:
            return LoggingConfigBuilder.CreateLoggingConfig()
                .AddTarget(ConsoleTargetConfigBuilder.Create()
                    .AddConsoleColorRule(new ConsoleColorRule(backgroundColorString, foregroundColorString, AllTags))
                    .Build())
                .Build();
        }
    }
}