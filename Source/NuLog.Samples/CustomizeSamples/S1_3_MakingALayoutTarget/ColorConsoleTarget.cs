/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/11/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Targets;
using System;

namespace NuLog.Samples.CustomizeSamples.S1_3_MakingALayoutTarget
{
    /// <summary>
    /// A sample class used to illustrate creating custom targets.  The narrative for this
    /// can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/1.3-Making-a-Layout-Target
    /// </summary>
    public class ColorConsoleTarget : LayoutTargetBase
    {
        // Synchronous logging
        public override void Log(LogEvent logEvent)
        {
            Console.Out.Write(Layout.FormatLogEvent(logEvent));
        }
    }
}