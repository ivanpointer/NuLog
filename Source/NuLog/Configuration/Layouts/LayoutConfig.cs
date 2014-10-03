using Newtonsoft.Json.Linq;
using NuLog.Targets.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Configuration.Layouts
{
    public class LayoutConfig
    {
        public const String DefaultFormat = "${DateTime:'{0:MM/dd/yyyy hh:mm:ss.fff}'} | ${Thread.ManagedThreadId:'{0,4}'} | ${Tags} | ${Message}${?Exception:'\r\n{0}'}\r\n";
        public string Type { get; set; }
        public string Format { get; set; }
        public JToken Config { get; set; }

        public LayoutConfig()
        {
            Type = typeof(StandardLayout).FullName;
            Format = DefaultFormat;
        }

        public LayoutConfig(JToken jToken)
        {
            Config = jToken;
            Type = typeof(StandardLayout).FullName;
            Format = DefaultFormat;

            JSONConfig(jToken);
        }

        public LayoutConfig(string format)
        {
            Config = null;
            Type = typeof(StandardLayout).FullName;
            Format = format;
        }

        private void JSONConfig(JToken jToken)
        {
            if (jToken != null)
            {
                if (jToken.Type == JTokenType.String)
                {
                    Format = jToken.Value<string>();
                }
                else if (jToken.Type == JTokenType.Object)
                {
                    var typeToken = jToken["type"];
                    if (typeToken != null && typeToken.Type == JTokenType.String)
                        Type = typeToken.Value<string>();

                    var formatToken = jToken["format"];
                    if (formatToken != null && formatToken.Type == JTokenType.String)
                        Format = formatToken.Value<string>();
                }
            }
        }
    }
}
