/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/15/2014
 * Updated: 7/4/2015
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using Newtonsoft.Json.Linq;
using NuLog.Layouts;

namespace NuLog.Configuration.Layouts
{
    /// <summary>
    /// Represents the standard layout config for layouts
    /// </summary>
    public class StandardLayoutConfig : LayoutConfig
    {
        /// <summary>
        /// The default layout format
        /// </summary>
        public const string DefaultFormat = "${DateTime:'{0:MM/dd/yyyy hh:mm:ss.fff}'} | ${Thread.ManagedThreadId:'{0,4}'} | ${Tags} | ${Message}${?Exception:'\r\n{0}'}\r\n";

        /// <summary>
        /// The string format of the layout.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Builds a StandardLayout config with the default format
        /// </summary>
        public StandardLayoutConfig()
            : base()
        {
            Type = typeof(StandardLayout).FullName;
            Format = DefaultFormat;
        }

        /// <summary>
        /// Builds a StandardLayout config with the provided format string
        /// </summary>
        /// <param name="format">The format to use for the layout</param>
        public StandardLayoutConfig(string format)
            : base()
        {
            Type = typeof(StandardLayout).FullName;
            Format = format;
        }

        public StandardLayoutConfig(JToken jsonConfig)
            : base(jsonConfig)
        {
            Type = typeof(StandardLayout).FullName;
            Format = DefaultFormat;

            // If the JSON object is provided
            if (jsonConfig != null)
            {
                if (jsonConfig.Type == JTokenType.String)
                {
                    Format = jsonConfig.Value<string>();
                }
                else if (jsonConfig.Type == JTokenType.Object)
                {
                    // Read the forat string
                    var formatToken = jsonConfig["format"];
                    if (formatToken != null && formatToken.Type == JTokenType.String)
                        Format = formatToken.Value<string>();
                }
            }
        }
    }
}