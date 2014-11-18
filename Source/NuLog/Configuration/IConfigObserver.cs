/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/5/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

namespace NuLog.Configuration
{
    /// <summary>
    /// Defines a config observer (observer pattern)
    /// </summary>
    public interface IConfigObserver
    {
        /// <summary>
        /// Notifies the observer of a new configuration
        /// </summary>
        /// <param name="loggingConfig">The new configuration</param>
        void NotifyNewConfig(LoggingConfig loggingConfig);
    }
}
