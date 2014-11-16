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
    /// Represents the base configuration for layouts
    /// </summary>
    public class LayoutConfig
    {
        /// <summary>
        /// The type of layout to be implemented.  Represents the full assembly/class name path to be used to reflectively instantiate the layout
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The JSON representation of the layout config
        /// </summary>
        public JToken Config { get; set; }

        /// <summary>
        /// Builds a default layout config using a StandardLayout
        /// </summary>
        public LayoutConfig() { }

        /// <summary>
        /// Builds a layout config using the provided JSON configuration
        /// </summary>
        /// <param name="jsonConfig">The default JSON configuration</param>
        public LayoutConfig(JToken jsonConfig)
        {
            Config = jsonConfig;

            // If the JSON object is provided
            if (jsonConfig != null && jsonConfig.Type == JTokenType.Object)
            {
                // Read the type
                var typeToken = jsonConfig["type"];
                if (typeToken != null && typeToken.Type == JTokenType.String)
                    Type = typeToken.Value<string>();
            }
        }
    }
}
