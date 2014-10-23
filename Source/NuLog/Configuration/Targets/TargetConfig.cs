/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/5/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using Newtonsoft.Json.Linq;
using NuLog.Targets;

namespace NuLog.Configuration.Targets
{
    /// <summary>
    /// Configuration representing a target
    /// </summary>
    public class TargetConfig
    {
        #region Constants
        public const string NameTokenName = "name";
        public const string TypeTokenName = "type";
        public const string SynchronousTokenName = "synchronous";
        #endregion

        /// <summary>
        /// The name of the target
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The type of the target.  This is used to reflectively build the type.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// A flag that if true, signals to the framework to log events to this target synchronously
        /// </summary>
        public bool? Synchronous { get; set; }
        /// <summary>
        /// The JSON token for this config
        /// </summary>
        public JToken Config { get; set; }

        /// <summary>
        /// Builds a standard target config
        /// </summary>
        /// <param name="synchronous">A flag that if true, signals to the framework to log events to this target synchronously</param>
        public TargetConfig(bool? synchronous = null)
        {
            Synchronous = synchronous;
        }

        /// <summary>
        /// Builds a target config using the given JSON token
        /// </summary>
        /// <param name="jToken">The JSON token to use to build the target</param>
        /// <param name="synchronous">A flag that if true, signals to the framework to log events to this target synchronously</param>
        public TargetConfig(JToken jToken, bool? synchronous = null)
        {
            // Setup
            Config = jToken;
            Type = typeof(SimpleConsoleTarget).FullName;
            Synchronous = synchronous;

            // If the token is provided
            if (jToken != null)
            {
                // If it is just a string
                if (jToken.Type == JTokenType.String)
                {
                    // Treat it as the "type"
                    Type = jToken.Value<string>();
                }
                // If it is an object, build it out as expected
                else if (jToken.Type == JTokenType.Object)
                {
                    var nameToken = jToken[NameTokenName];
                    if (nameToken != null && nameToken.Type == JTokenType.String)
                        Name = nameToken.Value<string>();

                    var typeToken = jToken[TypeTokenName];
                    if (typeToken != null && typeToken.Type == JTokenType.String)
                        Type = typeToken.Value<string>();

                    var synchronousToken = jToken[SynchronousTokenName];
                    if (synchronousToken != null && synchronousToken.Type == JTokenType.Boolean)
                        synchronous = synchronousToken.Value<bool>();
                }
            }
        }
    }
}
