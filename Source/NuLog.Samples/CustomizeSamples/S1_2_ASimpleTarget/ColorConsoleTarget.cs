/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/11/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Targets;
using System;

namespace NuLog.Samples.CustomizeSamples.S1_2_ASimpleTarget
{
    /// <summary>
    /// A sample class used to illustrate creating custom targets.  The narrative for this
    /// can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/1.2-A-Simple-Target
    /// </summary>
    public class ColorConsoleTarget : TargetBase
    {
        // Synchronous logging
        public override void Log(LogEvent logEvent)
        {
            Console.Out.WriteLine(String.Format("My custom target says: {0}", logEvent.Message));
        }
    }
}

