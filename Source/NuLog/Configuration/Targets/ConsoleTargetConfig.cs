using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Configuration.Targets
{
    public class ConsoleTargetConfig : LayoutTargetConfig
    {
        public ICollection<ConsoleColorRule> ColorRules { get; set; }

        public ConsoleTargetConfig()
            : base(synchronous: true)
        {
            ColorRules = new List<ConsoleColorRule>();
        }

        public ConsoleTargetConfig(JToken jToken)
            : base(jToken, synchronous: true)
        {
            ColorRules = new List<ConsoleColorRule>();

            if (jToken != null)
            {
                var colorRulesToken = jToken["colorRules"];
                if (colorRulesToken != null && colorRulesToken.Type == JTokenType.Array)
                {
                    var colorRulesTokens = colorRulesToken.Children();

                    ConsoleColorRule colorRule;
                    Type ConsoleColorType = typeof(ConsoleColor);
                    JToken tags;
                    JToken color;
                    JToken backgroundColor;

                    foreach (var colorRuleToken in colorRulesTokens)
                    {
                        try
                        {
                            if (colorRuleToken.Type == JTokenType.Object)
                            {
                                colorRule = new ConsoleColorRule();

                                tags = colorRuleToken["tags"];
                                if (tags != null && tags.Type == JTokenType.Array)
                                    colorRule.Tags = tags.Values<string>().ToList();

                                color = colorRuleToken["color"];
                                if (color != null && color.Type == JTokenType.String)
                                    colorRule.Color = (ConsoleColor)Enum.Parse(ConsoleColorType, color.Value<string>());

                                backgroundColor = colorRuleToken["background"];
                                if (backgroundColor != null && backgroundColor.Type == JTokenType.String)
                                    colorRule.BackgroundColor = (ConsoleColor)Enum.Parse(ConsoleColorType, backgroundColor.Value<string>());

                                ColorRules.Add(colorRule);
                            }
                        }
                        catch (Exception e)
                        {
                            // Just ignore this color rule - colors are not mission critical
                            // Log the failure
                            Trace.WriteLine(e, Constants.TraceLogger.ConfigCategory);
                        }
                    }
                }
            }
        }
    }
}
