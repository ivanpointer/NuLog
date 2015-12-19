/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/20/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using Newtonsoft.Json.Linq;
using NuLog.Extenders;
using System.Linq;

namespace NuLog.Configuration.Extenders
{
    /// <summary>
    /// Defines the configuration for the TraceListenerExtender
    /// </summary>
    public class TraceListenerConfig : ExtenderConfig
    {
        #region Constants/Members

        // The name of the tags configuration token in the JSON config
        public const string TagsTokenName = "tags";

        // Defaults for this configuration
        public static readonly string DefaultType = typeof(TraceListenerExtender).FullName.ToString();

        public static readonly string[] DefaultTags = new string[] { "tracelistener" };

        /// <summary>
        /// The list of tags to include in log events sent from trace
        /// </summary>
        public string[] TraceTags { get; set; }

        #endregion Constants/Members

        /// <summary>
        /// The default constructor setting this up with a "tracelistener" tag
        /// </summary>
        public TraceListenerConfig()
            : base()
        {
            Type = DefaultType;
            TraceTags = DefaultTags;
        }

        /// <summary>
        /// Builds this TraceListenerConfig from the provided JSON token
        /// </summary>
        /// <param name="config">The JSON token from which to build this configuration</param>
        public TraceListenerConfig(JToken config)
            : base(config)
        {
            // Set our defaults
            Type = DefaultType;
            TraceTags = DefaultTags;

            // Check to see if we have been given something parsable to
            //  configure with.  If so, look for the tags in the config
            //  and parse those out.  This supports a configuration of
            //  a single tag as a string, or a string array of tags.
            if (config != null && config.Type == JTokenType.Object)
            {
                var tagsRaw = config[TagsTokenName];
                if (tagsRaw != null)
                {
                    if (tagsRaw.Type == JTokenType.Array)
                    {
                        TraceTags = tagsRaw.Values<string>().ToArray<string>();
                    }
                    else if (tagsRaw.Type == JTokenType.String)
                    {
                        TraceTags = new string[] { tagsRaw.Value<string>() };
                    }
                }
            }
        }
    }
}