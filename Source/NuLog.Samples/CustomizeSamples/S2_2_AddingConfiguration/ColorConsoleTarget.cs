/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration.Targets;
using NuLog.Dispatch;
using NuLog.Targets;
using System;

namespace NuLog.Samples.CustomizeSamples.S2_2_AddingConfiguration
{
    /// <summary>
    /// A sample class used to illustrate creating custom targets.  The narrative for this
    /// can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/2.2-Adding-Configuration
    /// </summary>
    public class ColorConsoleTarget : LayoutTargetBase
    {
        // A lock to make sure our colors don't get a discombobulated
        private static readonly object _colorLock = new object();

        /* We no longer need the colors here, these will be moved into
         * a target configuration class
        public ConsoleColor BackgroundColor { get; set; }
        public ConsoleColor ForegroundColor { get; set; }*/

        /* We no longer need the default constructor - this configuration
         * is coming from the target's configuration
        public ColorConsoleTarget() : base()
        {
            BackgroundColor = ConsoleColor.DarkBlue;
            ForegroundColor = ConsoleColor.White;
        }*/

        public ColorConsoleTargetConfig ColorConfig { get; set; }

        public override void Initialize(TargetConfig targetConfig, LogEventDispatcher dispatcher = null, bool? synchronous = null)
        {
            // We need to inherit the configuration of the parent targets
            base.Initialize(targetConfig, dispatcher, synchronous);

            // Cast or parse our configuration into a ColorConsoleTargetConfig
            if (targetConfig != null)
                ColorConfig = targetConfig is ColorConsoleTargetConfig
                    ? (ColorConsoleTargetConfig)targetConfig
                    : new ColorConsoleTargetConfig(targetConfig.Config);
        }

        // Synchronous logging
        public override void Log(LogEvent logEvent)
        {
            // Lock, to keep our colors from getting messed up
            lock (_colorLock)
            {
                // Get a hold of the current colors
                var oldForeground = Console.ForegroundColor;
                var oldBackground = Console.BackgroundColor;

                try
                {
                    // Set the colors to our custom colors
                    Console.BackgroundColor = ColorConfig.BackgroundColor;
                    Console.ForegroundColor = ColorConfig.ForegroundColor;

                    // Write out our message
                    Console.Out.Write(Layout.FormatLogEvent(logEvent));
                }
                finally
                {
                    // Make sure we reset our colors
                    Console.ForegroundColor = oldForeground;
                    Console.BackgroundColor = oldBackground;
                }
            }
        }
    }
}