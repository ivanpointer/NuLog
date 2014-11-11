/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/11/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Configuration.Targets;
using NuLog.Dispatch;
using NuLog.Targets;
using System;
using System.Collections.Concurrent;

namespace NuLog.Samples.CustomizeSamples.S1_1_BasicAnatomyOfATarget
{
    /// <summary>
    /// A sample class used to illustrate creating custom targets.  The narrative for this
    /// can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/1.1-Basic-Anatomy-of-a-Target
    /// </summary>
    public class ColorConsoleTarget : TargetBase
    {
        // Default settings
        private const string DefaultName = "colorConsole";
        private const bool DefaultSynchronous = true;

        // Initialize the target
        public override void Initialize(TargetConfig targetConfig, LogEventDispatcher dispatcher = null, bool? synchronous = null)
        {
            base.Initialize(targetConfig, dispatcher, synchronous);
        }

        // Notify the target of a new config
        public override void NotifyNewConfig(TargetConfig targetConfig)
        {
            base.NotifyNewConfig(targetConfig);
        }

        // Asynchronous logging
        protected override void ProcessLogQueue(ConcurrentQueue<LogEvent> logQueue, LogEventDispatcher dispatcher)
        {
            base.ProcessLogQueue(logQueue, dispatcher);
        }

        // Synchronous logging
        public override void Log(LogEvent logEvent)
        {
            throw new NotImplementedException();
        }

        // Shuts down the target
        public override bool Shutdown()
        {
            return base.Shutdown();
        }
    }
}
