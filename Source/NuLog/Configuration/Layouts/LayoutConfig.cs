/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/5/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using Newtonsoft.Json.Linq;
using NuLog.Layouts;
using System;

namespace NuLog.Configuration.Layouts
{
    /// <summary>
    /// Represents the base configuration for layout providers
    /// </summary>
    public class LayoutConfig
    {
        /// <summary>
        /// The default layout format
        /// </summary>
        public const String DefaultFormat = "${DateTime:'{0:MM/dd/yyyy hh:mm:ss.fff}'} | ${Thread.ManagedThreadId:'{0,4}'} | ${Tags} | ${Message}${?Exception:'\r\n{0}'}\r\n";

        /// <summary>
        /// The type of layout to be implemented.  Represents the full assembly/class name path to be used to reflectively instantiate the layout
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The string format of the layout.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// The JSON representation of the layout config
        /// </summary>
        public JToken Config { get; set; }

        /// <summary>
        /// Builds a default layout config using a StandardLayout
        /// </summary>
        public LayoutConfig()
        {
            Type = typeof(StandardLayout).FullName;
            Format = DefaultFormat;
        }

        /// <summary>
        /// Builds a layout config using the provided JSON configuration
        /// </summary>
        /// <param name="jToken">The default JSON configuration</param>
        public LayoutConfig(JToken jToken)
        {
            Config = jToken;
            Type = typeof(StandardLayout).FullName;
            Format = DefaultFormat;

            JSONConfig(jToken);
        }
        
        /// <summary>
        /// Builds a StandardLayout config with the provided format string
        /// </summary>
        /// <param name="format">The format to use for the layout</param>
        public LayoutConfig(string format)
        {
            Config = null;
            Type = typeof(StandardLayout).FullName;
            Format = format;
        }

        /// <summary>
        /// Configures this layout config using the provided JSON configuration
        /// </summary>
        /// <param name="jToken">The JSON configuration to use to configure this layout config</param>
        private void JSONConfig(JToken jToken)
        {
            // If the JSON object is provided
            if (jToken != null)
            {
                // If it is simply a string, used a StandardLayout and treat the config as a format
                if (jToken.Type == JTokenType.String)
                {
                    Format = jToken.Value<string>();
                }
                // Otherwise, treat it as a full config object
                else if (jToken.Type == JTokenType.Object)
                {
                    // Read the type
                    var typeToken = jToken["type"];
                    if (typeToken != null && typeToken.Type == JTokenType.String)
                        Type = typeToken.Value<string>();

                    // Read the forat string
                    var formatToken = jToken["format"];
                    if (formatToken != null && formatToken.Type == JTokenType.String)
                        Format = formatToken.Value<string>();
                }
            }
        }
    }
}
