/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using System.Xml;

namespace NuLog.Tests.Unit.Configuration.XmlConfigurationProviderTests {

    /// <summary>
    /// The base test methods for testing the XML configuration provider.
    /// </summary>
    public class XmlConfigurationProviderTestsBase {

        /// <summary>
        /// Returns the configuration provider under test.
        /// </summary>
        protected IConfigurationProvider GetConfigurationProvider(string xmlString) {
            var xmlElement = ToXmlElement(xmlString);
            return new XmlConfigurationProvider(xmlElement);
        }

        /// <summary>
        /// Converts the given XML string, into an XmlElement.
        /// </summary>
        private static XmlElement ToXmlElement(string xmlString) {
            var doc = new XmlDocument();
            doc.LoadXml(xmlString);
            return doc.DocumentElement;
        }
    }
}