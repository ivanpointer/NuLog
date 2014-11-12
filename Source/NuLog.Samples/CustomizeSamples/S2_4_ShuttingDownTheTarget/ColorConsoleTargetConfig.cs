/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/11/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using Newtonsoft.Json.Linq;
using NuLog.Configuration.Targets;
using System;
using System.Diagnostics;

namespace NuLog.Samples.CustomizeSamples.S2_4_ShuttingDownTheTarget
{
    /// <summary>
    /// A sample target configuration for configuring a background and foreground
    /// color on a console target.  See the following article for the narration
    /// of this example:
    /// https://github.com/ivanpointer/NuLog/wiki/2.2-Adding-Configuration
    /// </summary>
    public class ColorConsoleTargetConfig : LayoutTargetConfig
    {

        #region Constants

        // Error messages
        private const string FailedParseConsoleColorMessage = "Failed to parse the ConsoleColor \"{0}\"";

        // The name of the configuration items in the JSON configuration
        //  and the ConsoleColor type for parsing
        private const string BackgroundColorToken = "backgroundColor";
        private const string ForegroundColorToken = "foregroundColor";
        private static readonly Type ConsoleColorType = typeof(ConsoleColor);

        // The defaults for a runtime configuration
        private const string DefaultName = "colorConsole";
        private static readonly string DefaultType = typeof(ColorConsoleTarget).FullName;

        // Default colors, in case none are supplied
        private const ConsoleColor DefaultBackgroundColor = ConsoleColor.Black;
        private const ConsoleColor DefaultForegroundColor = ConsoleColor.Gray;

        #endregion

        /// <summary>
        /// The background color to display log events in
        /// </summary>
        public ConsoleColor BackgroundColor { get; set; }
        /// <summary>
        /// The foreground color to display log events in
        /// </summary>
        public ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// The default constructor.  This is generally used for runtime configurations.
        /// </summary>
        public ColorConsoleTargetConfig() : base()
        {
            Name = DefaultName;
            Type = DefaultType;
            BackgroundColor = DefaultBackgroundColor;
            ForegroundColor = DefaultForegroundColor;
        }

        /// <summary>
        /// Builds this configuration based on a provided jToken, which is
        /// the JSON token representing this target configuration
        /// </summary>
        /// <param name="jToken"></param>
        public ColorConsoleTargetConfig(JToken jToken) : base(jToken)
        {
            if (jToken != null)
            {
                BackgroundColor = GetConsoleColor(jToken, BackgroundColorToken, DefaultBackgroundColor);
                ForegroundColor = GetConsoleColor(jToken, ForegroundColorToken, DefaultForegroundColor);
            }
        }

        #region Helpers

        // A helper that reads the child value from the given token, by the given child name
        //  and tries to parse a color out of it, if it fails, it returns the default value
        private static ConsoleColor GetConsoleColor(JToken jToken, string name, ConsoleColor defVal)
        {
            var child = jToken[name];
            try
            {
                if (child != null)
                    return (ConsoleColor)Enum.Parse(ConsoleColorType, child.Value<string>());
            }
            catch
            {
                Trace.WriteLine(String.Format(FailedParseConsoleColorMessage, child));
            }
            
            return defVal;
        }

        #endregion

    }
}
