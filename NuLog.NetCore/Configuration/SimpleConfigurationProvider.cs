/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using Microsoft.Extensions.Configuration;
using NuLog.Configuration;
using INuLogConfigProvider = NuLog.Configuration.IConfigurationProvider;
using MsConfigSection = Microsoft.Extensions.Configuration.IConfigurationSection;

namespace NuLog.NetCore.Configuration
{
    public class SimpleConfigurationProvider : INuLogConfigProvider
    {
        private readonly MsConfigSection _configSection;

        public SimpleConfigurationProvider(MsConfigSection section)
        {
            _configSection = section;
        }

        public Config GetConfiguration()
        {
            return _configSection.Get<Config>();
        }
    }
}