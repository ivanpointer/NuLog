/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/20/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using Newtonsoft.Json.Linq;

namespace NuLog.Configuration.Extenders
{
    /// <summary>
    /// Defines a base configuration item for the extenders
    /// </summary>
    public class ExtenderConfig
    {

        #region Constants/Members

        /// <summary>
        /// The name of the JSON token representing the "type" in the JSON configuration
        /// </summary>
        public const string TypeTokenName = "type";

        /// <summary>
        /// A string representing the full name and assembly of the type of the extender.  This is used to reflectively instantiate the extender
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The JSON token representing the configuration for this extender.
        /// This is only populated in the instance that the configuration is loaded from a JSON file;
        /// this will be null if runtime configuration is being used
        /// </summary>
        public JToken Config { get; set; }

        #endregion

        /// <summary>
        /// Default empty configuration, with optional type
        /// </summary>
        /// <param name="type">The type to set for this config</param>
        public ExtenderConfig(string type = null)
        {
            Type = type;
            Config = null;
        }

        /// <summary>
        /// Builds an extender config using the given JSON token
        /// </summary>
        /// <param name="config">The JSON token to use to build the extender config</param>
        public ExtenderConfig(JToken config)
        {
            // Setup
            Config = config;
            Type = null;

            // If the token is provided
            if (config != null)
            {
                // If it is a string, assume that it is the "type"
                if (config.Type == JTokenType.String)
                {
                    Type = config.Value<string>();
                }
                // If it is an object, look for the type token and interpret it
                else if (config.Type == JTokenType.Object)
                {
                    var typeToken = config[TypeTokenName];
                    if (typeToken != null && typeToken.Type == JTokenType.String)
                        Type = typeToken.Value<string>();
                }
            }
        }

    }
}
