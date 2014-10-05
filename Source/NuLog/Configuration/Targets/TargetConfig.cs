using Newtonsoft.Json.Linq;
using NuLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Configuration.Targets
{
    public class TargetConfig
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool? Synchronous { get; set; }
        public JToken Config { get; set; }

        public TargetConfig(bool? synchronous = null)
        {
            Synchronous = synchronous;
        }

        public TargetConfig(JToken jToken, bool? synchronous = null)
        {
            Config = jToken;
            Type = typeof(BufferedConsoleTarget).FullName;
            Synchronous = synchronous;

            if (jToken != null)
            {
                if (jToken.Type == JTokenType.String)
                {
                    Type = jToken.Value<string>();
                }
                else if (jToken.Type == JTokenType.Object)
                {
                    var nameToken = jToken["name"];
                    if (nameToken != null && nameToken.Type == JTokenType.String)
                        Name = nameToken.Value<string>();

                    var typeToken = jToken["type"];
                    if (typeToken != null && typeToken.Type == JTokenType.String)
                        Type = typeToken.Value<string>();

                    var synchronousToken = jToken["synchronous"];
                    if (synchronousToken != null && synchronousToken.Type == JTokenType.Boolean)
                        synchronous = synchronousToken.Value<bool>();
                }
            }
        }
    }
}
