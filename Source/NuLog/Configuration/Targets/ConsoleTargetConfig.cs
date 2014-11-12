/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/5/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using Newtonsoft.Json.Linq;
using NuLog.Targets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NuLog.Configuration.Targets
{
    /// <summary>
    /// The configuration that represents the console target
    /// </summary>
    public class ConsoleTargetConfig : LayoutTargetConfig
    {
        #region Constants
        // Tokens
        private const string ColorRulesTokenName = "colorRules";
        private const string TagsTokenName = "tags";
        private const string ForeColorTokenName = "foreground";
        private const string BackColorTokenName = "background";

        // Functional Values
        private const string TraceConfigCategory = "config";
        private static readonly Type ConsoleColorType = typeof(ConsoleColor);

        #endregion

        /// <summary>
        /// The list of color rules associated to this configuration
        /// </summary>
        public ICollection<ConsoleColorRule> ColorRules { get; set; }

        /// <summary>
        /// Instantiates a base console target config
        /// </summary>
        public ConsoleTargetConfig()
            : base(synchronous: true)
        {
            Type = typeof(ConsoleTarget).FullName;
            ColorRules = new List<ConsoleColorRule>();
        }

        /// <summary>
        /// Instantiates a console target config using the passed JSON token
        /// </summary>
        /// <param name="jToken">The JSON token to parse for this console target config</param>
        public ConsoleTargetConfig(JToken jToken)
            : base(jToken, synchronous: true)
        {
            // Initialize the list of color rules
            ColorRules = new List<ConsoleColorRule>();

            // If a JSON configuration was provided
            if (jToken != null)
            {
                // Get a hold of the color rules in the JSON if they are provided
                var colorRulesToken = jToken[ColorRulesTokenName];
                if (colorRulesToken != null && colorRulesToken.Type == JTokenType.Array)
                {
                    // Prepare to parse the rules
                    var colorRulesTokens = colorRulesToken.Children();
                    ConsoleColorRule colorRule;
                    JToken tags;
                    JToken foreColorToken;
                    JToken backColorToken;

                    // Iterate over each of the child JSON tokens
                    //  in the rules
                    foreach (var colorRuleToken in colorRulesTokens)
                    {
                        try
                        {
                            // Make sure that the child element is an object
                            if (colorRuleToken.Type == JTokenType.Object)
                            {
                                // And if so, parse out the parts
                                colorRule = new ConsoleColorRule();

                                tags = colorRuleToken[TagsTokenName];
                                if (tags != null && tags.Type == JTokenType.Array)
                                    colorRule.Tags = tags.Values<string>().ToList();

                                foreColorToken = colorRuleToken[ForeColorTokenName];
                                if (foreColorToken != null && foreColorToken.Type == JTokenType.String)
                                    colorRule.ForegroundColor = (ConsoleColor)Enum.Parse(ConsoleColorType, foreColorToken.Value<string>());

                                backColorToken = colorRuleToken[BackColorTokenName];
                                if (backColorToken != null && backColorToken.Type == JTokenType.String)
                                    colorRule.BackgroundColor = (ConsoleColor)Enum.Parse(ConsoleColorType, backColorToken.Value<string>());

                                ColorRules.Add(colorRule);
                            }
                        }
                        catch (Exception e)
                        {
                            // Just ignore this color rule - colors are not mission critical
                            // Log the failure
                            Trace.WriteLine(e, TraceConfigCategory);
                        }
                    }
                }
            }
        }
    }
}
