/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

namespace NuLog.Configuration {

    /// <summary>
    /// Defines the expected behavior of a configuration provider.
    /// </summary>
    public interface IConfigurationProvider {

        /// <summary>
        /// Returns the configuration provided by this provider.
        /// </summary>
        Config GetConfiguration();
    }
}