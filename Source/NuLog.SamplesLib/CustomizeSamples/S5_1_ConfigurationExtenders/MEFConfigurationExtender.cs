/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/13/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Configuration;
using NuLog.Configuration.Targets;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace NuLog.Samples.CustomizeSamples.S5_1_ConfigurationExtenders
{
    /// <summary>
    /// Custom configuration extender that adds a console target and directs all log events to it
    /// </summary>
    [Export(typeof(ILoggingConfigExtender))]
    public class MEFConfigurationExtender : ILoggingConfigExtender
    {
        #region Constants

        // Particular settings for our target and rule
        private const string MyCustomTargetName = "mefCustomConsole";
        private const string MyCustomRuleInclude = "mefCustomConsole";

        private const ConsoleColor MyCustomBackgroundColor = ConsoleColor.DarkRed;
        private const ConsoleColor MyCustomForegroundColor = ConsoleColor.White;

        // ...

        #endregion

        public void UpdateConfig(LoggingConfig loggingConfig)
        {
            // Make sure targets exists
            if (loggingConfig.Targets == null)
                loggingConfig.Targets = new List<TargetConfig>();

            // Add our target if it doesn't yet exist
            if (!loggingConfig.Targets.Any(_ => _.Name == MyCustomTargetName))
            {
                loggingConfig.Targets.Add(ConsoleTargetConfigBuilder.Create()
                    .SetName(MyCustomTargetName)
                    .AddConsoleColorRule(new ConsoleColorRule
                    {
                        Tags = new string[] { MyCustomRuleInclude },
                        BackgroundColor = MyCustomBackgroundColor,
                        ForegroundColor = MyCustomForegroundColor
                    })
                    .Build());
            }

            // Make sure rules exist
            if (loggingConfig.Rules == null)
                loggingConfig.Rules = new List<RuleConfig>();

            // Add our rule if it doesn't yet exist
            if (!loggingConfig.Rules.Any(_ =>
                _.Include != null
                && _.Include.Count == 1
                && _.Include.Contains(MyCustomRuleInclude)

                && _.WriteTo != null
                && _.WriteTo.Count == 1
                && _.WriteTo.Contains(MyCustomTargetName)))
                loggingConfig.Rules.Add(RuleConfigBuilder.CreateRuleConfig()
                    .AddInclude(MyCustomRuleInclude)
                    .AddWriteTo(MyCustomTargetName)
                    .Build());
        }
    }
}
