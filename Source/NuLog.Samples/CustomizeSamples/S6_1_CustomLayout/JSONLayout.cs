/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using Newtonsoft.Json;
using NuLog.Configuration.Layouts;
using NuLog.Layouts;

namespace NuLog.Samples.CustomizeSamples.S6_1_CustomLayout
{
    /// <summary>
    /// A very simple Layout - it simple converts the log event to a JSON string
    /// The narrative for this sample can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/6.1-Creating-a-Custom-Layout
    /// </summary>
    public class JSONLayout : ILayout
    {
        public void Initialize(LayoutConfig layoutConfig)
        {
            // There is nothing we need to know here
        }

        public string FormatLogEvent(LogEvent logEvent)
        {
            // Very simple, just convert the logEvent to a JSON string
            return JsonConvert.SerializeObject(logEvent);
        }
    }
}