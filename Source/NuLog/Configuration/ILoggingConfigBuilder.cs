/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/5/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

namespace NuLog.Configuration
{
    /// <summary>
    /// Defines an expected interface for building a logging config
    /// </summary>
    public interface ILoggingConfigBuilder
    {
        /// <summary>
        /// Builds out the logging config
        /// </summary>
        /// <returns>The built config</returns>
        LoggingConfig Build();
    }
}
