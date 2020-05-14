/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Configuration;
using System.Xml;

namespace NuLog.Configuration {

    /// <summary>
    /// A configuration provider which loads the configuration from the configuration manager - I.E.
    /// from Web.config or App.config.
    /// </summary>
    public class ConfigurationManagerProvider : IConfigurationProvider {
        private readonly string sectionName;

        public ConfigurationManagerProvider(string sectionName = "nulog") {
            this.sectionName = sectionName;
        }

        public Config GetConfiguration() {
            // Get our XML element
            var xmlElement = (XmlElement)ConfigurationManager.GetSection(this.sectionName);

            // Build a provider for the XmlElement
            var xmlProvider = new XmlConfigurationProvider(xmlElement);

            // Get the config from the provider
            var config = xmlProvider.GetConfiguration();

            // Return the config
            return config;
        }
    }
}