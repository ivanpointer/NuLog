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
    /// Provides a base abstract class for extenders;
    /// an implementation for UpdateConfig and Shutdown is not
    /// required in all instances, this allows for a more basic
    /// implementation of an extender.
    /// </summary>
    public abstract class ExtenderBase : IExtender
    {

        #region Constants/Members

        /// <summary>
        /// Default time in milliseconds allowed for the shutdown of this extender
        /// </summary>
        public const int DefaultShutdownTimeout = 5000;

        /// <summary>
        /// The extender config specific to this extender
        /// </summary>
        protected ExtenderConfig ExtenderConfig { get; set; }

        /// <summary>
        /// The whole configuration
        /// </summary>
        protected LoggingConfig LoggingConfig { get; set; }

        #endregion

        /// <summary>
        /// Gives this extender the opportunity to extend the given logging configuration before it is used to initialize the objects in the framework
        /// </summary>
        /// <param name="loggingConfig">The logging config to update</param>
        public virtual void UpdateConfig(LoggingConfig loggingConfig)
        {
            // Do nothing
        }

        /// <summary>
        /// Initializes the extender with the given configuration objects
        /// </summary>
        /// <param name="extenderConfig">The configuration for this extender</param>
        /// <param name="loggingConfig">The whole configuration</param>
        public virtual void Initialize(ExtenderConfig extenderConfig, LoggingConfig loggingConfig)
        {
            ExtenderConfig = extenderConfig;
            LoggingConfig = loggingConfig;
        }

        /// <summary>
        /// Instructs the extender to startup, providing the initialized dispatcher
        /// </summary>
        /// <param name="dispatcher">The dispatcher to associate with this extender</param>
        public abstract void Startup(LogEventDispatcher dispatcher);

        /// <summary>
        /// Instructs this extender to shtudown within the given amount of time, using an optional intialized stopwatch
        /// </summary>
        /// <param name="timeout">The time, in milliseconds, this extender should take to shutdown</param>
        /// <param name="stopwatch">An optional stopwatch to use to determine how much time is passed</param>
        /// <returns>A bool indicating whether or not the extender succesfully shutdown within the given time</returns>
        public virtual bool Shutdown(int timeout = DefaultShutdownTimeout, Stopwatch stopwatch = null)
        {
            // Do nothing
            return true;
        }
    }
}
