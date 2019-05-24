/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Configuration;
using System.Xml;

namespace NuLog.Configuration {

    public class ConfigurationSectionHandler : IConfigurationSectionHandler {

        /// <summary>
        /// Parses the configuration section.
        /// </summary>
        /// <param name="parent">       
        /// The configuration settings in a corresponding parent configuration section.
        /// </param>
        /// <param name="configContext">
        /// The configuration context when called from the ASP.NET configuration system. Otherwise,
        /// this parameter is reserved and is a null reference.
        /// </param>
        /// <param name="section">      The <see cref="XmlNode" /> for the NuLog section.</param>
        /// <returns>The <see cref="XmlNode" /> for the NuLog section.</returns>
        /// <remarks>
        /// <para>Returns the <see cref="XmlNode" /> containing the configuration data,</para>
        /// </remarks>
        public object Create(object parent, object configContext, XmlNode section) {
            return section;
        }
    }
}