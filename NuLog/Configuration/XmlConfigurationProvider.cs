﻿/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace NuLog.Configuration {

    /// <summary>
    /// The standard implementation of a configuration provider, which parses an XmlElement to build
    /// out the configuration.
    /// </summary>
    public class XmlConfigurationProvider : IConfigurationProvider {

        /// <summary>
        /// The XML element that the config is to be parsed from.
        /// </summary>
        private readonly XmlElement xmlElement;

        public XmlConfigurationProvider(XmlElement xmlElement) {
            this.xmlElement = xmlElement;
        }

        public Config GetConfiguration() {
            // Stitch it all together
            var config = new Config {
                Rules = ParseRules(xmlElement),
                TagGroups = ParseTagGroups(xmlElement),
                Targets = ParseTargets(xmlElement),
                MetaData = ParseMetaData(xmlElement)
            };

            // Get the "includeStackFrame" flag
            config.IncludeStackFrame = GetBooleanAttribute(xmlElement, "includeStackFrame");

            // Get the fallback log path
            config.FallbackLogPath = GetStringAttribute(xmlElement, "fallbackLog");

            // Return the built config
            return config;
        }

        #region Parse Rules

        /// <summary>
        /// Parse out the rules from the given xmlElement.
        /// </summary>
        private static ICollection<RuleConfig> ParseRules(XmlElement xmlElement) {
            // A fresh, empty list of rules
            var rules = new List<RuleConfig>();

            // Find the rules collection in the element
            var rulesElement = xmlElement.SelectSingleNode("rules");

            // Check to see if we have a rules element, and return the empty list if no
            if (rulesElement == null) {
                return rules;
            }

            // Iterate over each rule element, and parse it out, adding it to the list
            foreach (var ruleElement in rulesElement.SelectNodes("rule")) {
                rules.Add(ParseRule((XmlElement)ruleElement));
            }

            // Return our parsed list of rules
            return rules;
        }

        /// <summary>
        /// Takes a single xmlElement that represents a single rule, and parses it out as a config rule.
        /// </summary>
        private static RuleConfig ParseRule(XmlElement xmlElement) {
            // Stitch the rule together
            return new RuleConfig {
                Includes = GetAttributeList(xmlElement, "include"),
                Excludes = GetAttributeList(xmlElement, "exclude"),
                Targets = GetAttributeList(xmlElement, "targets"),
                StrictInclude = GetBooleanAttribute(xmlElement, "strictInclude"),
                Final = GetBooleanAttribute(xmlElement, "final")
            };
        }

        #endregion Parse Rules

        #region Parse Targets

        /// <summary>
        /// Parse out the targets from the given xmlElement.
        /// </summary>
        private static ICollection<TargetConfig> ParseTargets(XmlElement xmlElement) {
            // The list of targets
            var targets = new List<TargetConfig>();

            // Find the targets collection in the element
            var targetsElement = xmlElement.SelectSingleNode("targets");

            // Check to see if we have a targets element, and return the empty list if no
            if (targetsElement == null) {
                return targets;
            }

            // Iterate over each target element, and parse it out, adding it to the list
            foreach (var targetElement in targetsElement.SelectNodes("target")) {
                targets.Add(ParseTarget((XmlElement)targetElement));
            }

            // Return the list of targets
            return targets;
        }

        /// <summary>
        /// Parses out a single target from the given xmlElement.
        /// </summary>
        private static TargetConfig ParseTarget(XmlElement xmlElement) {
            return new TargetConfig {
                Name = GetStringAttribute(xmlElement, "name"),
                Type = GetStringAttribute(xmlElement, "type"),
                Properties = GetAttributesAsDictionary(xmlElement)
            };
        }

        #endregion Parse Targets

        #region Parse Tag Groups

        /// <summary>
        /// Parse out the tag groups from the given xmlElement.
        /// </summary>
        private static ICollection<TagGroupConfig> ParseTagGroups(XmlElement xmlElement) {
            // The list of tag groups
            var tagGroups = new List<TagGroupConfig>();

            // Find the tag groups collection in the element
            var tagGroupsElement = xmlElement.SelectSingleNode("tagGroups");

            // Check to see if we have a tag groups element, and return the empty list if no
            if (tagGroupsElement == null) {
                return tagGroups;
            }

            // Iterate over each tag group element, and parse it out, adding it to the list
            foreach (var tagGroupElement in tagGroupsElement.SelectNodes("group")) {
                tagGroups.Add(ParseTagGroup((XmlElement)tagGroupElement));
            }

            // Return the list of tag groups
            return tagGroups;
        }

        /// <summary>
        /// Parses out a single tag group from the given XML element.
        /// </summary>
        private static TagGroupConfig ParseTagGroup(XmlElement xmlElement) {
            return new TagGroupConfig {
                BaseTag = GetStringAttribute(xmlElement, "baseTag"),
                Aliases = GetAttributeList(xmlElement, "aliases")
            };
        }

        #endregion Parse Tag Groups

        #region Parse MetaData

        /// <summary>
        /// Parse out the meta data defined in the given XML element.
        /// </summary>
        private static IDictionary<string, string> ParseMetaData(XmlElement xmlElement) {
            // The meta data
            var metaData = new Dictionary<string, string>();

            // Find the meta data element
            var metaDataElement = xmlElement.SelectSingleNode("metaData");

            // Check to see if we got the element, and return the empty dictionary if we didn't
            if (metaDataElement == null) {
                return metaData;
            }

            // Iterate over each meta data element, parsing it out and adding it to the dictionary
            foreach (var metaDataEntry in metaDataElement.SelectNodes("add")) {
                var key = GetStringAttribute((XmlElement)metaDataEntry, "key");
                var value = GetStringAttribute((XmlElement)metaDataEntry, "value");
                metaData[key] = value;
            }

            // Return the meta data
            return metaData;
        }

        #endregion Parse MetaData

        #region Helpers

        /// <summary>
        /// Gets the named attribute from the given XML element, and splits it by comma, and returns
        /// a collection of the resulting values.
        /// </summary>
        private static ICollection<string> GetAttributeList(XmlElement xmlElement, string attributeName) {
            // Le collection
            ICollection<string> items = null;

            // Get the attribute off the element
            var attribute = xmlElement.Attributes.GetNamedItem(attributeName);

            // Split the value into a list, if we got something
            if (attribute != null) {
                var attributeValue = attribute.Value;
                var attributeArray = attributeValue.Split(',');
                items = attributeArray.ToList();
            }

            // Return our items, or an empty list if we didn't find anything
            return items ?? new List<string>();
        }

        /// <summary>
        /// Gets, and parses into bool, a named attribute from the given XML element.
        /// </summary>
        private static bool GetBooleanAttribute(XmlElement xmlElement, string attributeName, bool defaultValue = false) {
            // Get the attribute off the element
            var attribute = xmlElement.Attributes.GetNamedItem(attributeName);

            // If no attribute by that name is found, return the default
            if (attribute == null) {
                return defaultValue;
            }

            // Parse out the value of the attribute
            var attributeValue = attribute.Value;
            bool parsed = false;
            if (bool.TryParse(attributeValue, out parsed)) {
                // The value was successfully parsed, return that
                return parsed;
            } else {
                // The value wasn't successfully parsed, return the default
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets and returns the named attribute from the given XML element, in string form.
        /// </summary>
        private static string GetStringAttribute(XmlElement xmlElement, string attributeName) {
            // Get the attribute in question
            var attribute = xmlElement.Attributes.GetNamedItem(attributeName);

            // Return the value off the attribute, or null if no attribute was found
            return attribute != null ? attribute.Value : null;
        }

        /// <summary>
        /// Returns the attributes on the XML element as a dictionary.
        /// </summary>
        private static IDictionary<string, object> GetAttributesAsDictionary(XmlElement xmlElement) {
            // The dictionary
            var dict = new Dictionary<string, object>();

            // Iterate over each attribute, adding it to the dictionary
            foreach (var attribute in xmlElement.Attributes) {
                var attr = (XmlAttribute)attribute;
                dict[attr.Name] = attr.Value;
            }

            // Return the dictionary
            return dict;
        }

        #endregion Helpers
    }
}