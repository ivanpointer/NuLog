/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/7/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Dispatch;
using System.Collections.Generic;

namespace NuLog.Loggers
{
    /// <summary>
    /// The most basic logger implementation, the default implementation for the framework
    /// </summary>
    public class DefaultLogger : LoggerBase
    {
        /// <summary>
        /// Constructs the basic logger, with the given dispatcher attached
        /// </summary>
        /// <param name="dispatcher">The dispatcher to use for this logger</param>
        public DefaultLogger(LogEventDispatcher dispatcher) : base(dispatcher) { }

        /// <summary>
        /// Constructs the basic logger, with the given dispatcher and default tags
        /// </summary>
        /// <param name="dispatcher">The dispatcher to uswe for this logger</param>
        /// <param name="defaultTags">The default tags to assign to the logger</param>
        public DefaultLogger(LogEventDispatcher dispatcher, ICollection<string> defaultTags) : base(dispatcher, defaultTags) { }
    }
}