/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/5/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

namespace NuLog.Configuration
{
    /// <summary>
    /// Defines the expected interface for a logging config extender
    /// </summary>
    public interface ILoggingConfigExtender
    {
        /// <summary>
        /// Updates the passed config
        /// </summary>
        /// <param name="loggingConfig">The logging config to update</param>
        void UpdateConfig(LoggingConfig loggingConfig);
    }
}
