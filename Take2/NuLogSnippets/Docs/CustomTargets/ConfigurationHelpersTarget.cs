/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.LogEvents;
using NuLog.Targets;
using System;

namespace NuLogSnippets.Docs.CustomTargets
{
    public class ConfigurationHelpersTarget : TargetBase
    {
        public string MyStringProperty { get; set; }

        public bool MyBoolProperty { get; set; }

        // start_snippet
        public override void Configure(TargetConfig config)
        {
            base.Configure(config);

            this.MyStringProperty = GetProperty<string>(config, "oneFish");

            bool isTwoFish;
            this.MyBoolProperty = TryGetProperty<bool>(config, "twoFish", out isTwoFish)
                ? isTwoFish
                : false; // default
        }

        // end_snippet

        public override void Write(LogEvent logEvent)
        {
            throw new NotImplementedException();
        }
    }
}