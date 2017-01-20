/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Factories.Configuration;

namespace NuLog.Configuration
{
    /// <summary>
    /// The standard implementation of a configuration provider.
    /// </summary>
    public class StandardConfigurationProvider : IConfigurationProvider
    {
        public StandardConfigurationProvider(string configSection)
        {
        }

        public Config GetConfiguration()
        {
            return new Config();
        }
    }
}