/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/11/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Targets;
using System;

namespace NuLog.Samples.CustomizeSamples.S2_1_AddingColor
{
    /// <summary>
    /// A sample class used to illustrate creating custom targets.  The narrative for this
    /// can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/2.1-Adding-Color
    /// </summary>
    public class ColorConsoleTarget : LayoutTargetBase
    {
        // A lock to make sure our colors don't get a discombobulated
        private static readonly object _colorLock = new object();

        // Our colors
        public ConsoleColor BackgroundColor { get; set; }
        public ConsoleColor ForegroundColor { get; set; }

        // Setup our "default" colors
        public ColorConsoleTarget() : base()
        {
            BackgroundColor = ConsoleColor.DarkBlue;
            ForegroundColor = ConsoleColor.White;
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
                    Console.BackgroundColor = BackgroundColor;
                    Console.ForegroundColor = ForegroundColor;

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

