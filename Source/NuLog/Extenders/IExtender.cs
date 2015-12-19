/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/20/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Configuration;
using NuLog.Configuration.Extenders;
using NuLog.Dispatch;
using System.Diagnostics;

namespace NuLog.Extenders
{
    /// <summary>
    /// Defines the interface for an extender.  This is a generic object given the scope necessary
    ///   to extend the NuLog framework.
    /// </summary>
    public interface IExtender : ILoggingConfigExtender
    {
        /// <summary>
        /// Initializes the extender with the given configuration objects
        /// </summary>
        /// <param name="extenderConfig">The configuration for this extender</param>
        /// <param name="loggingConfig">The whole configuration</param>
        void Initialize(ExtenderConfig extenderConfig, LoggingConfig loggingConfig);

        /// <summary>
        /// Instructs the extender to startup, providing the initialized dispatcher
        /// </summary>
        /// <param name="dispatcher">The dispatcher to associate with this extender</param>
        void Startup(LogEventDispatcher dispatcher);

        /// <summary>
        /// Instructs this extender to shtudown within the given amount of time, using an optional intialized stopwatch
        /// </summary>
        /// <param name="timeout">The time, in milliseconds, this extender should take to shutdown</param>
        /// <param name="stopwatch">An optional stopwatch to use to determine how much time is passed</param>
        /// <returns>A bool indicating whether or not the extender succesfully shutdown within the given time</returns>
        bool Shutdown(int timeout, Stopwatch stopwatch = null);
    }
}