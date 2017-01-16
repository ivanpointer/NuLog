/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Targets;
using System.Collections.Generic;
using System.Text;

namespace NuLog.Layouts.Standard
{
    /// <summary>
    /// The standard implementation of a layout.
    /// </summary>
    public class StandardLayout : ILayout
    {
        /// <summary>
        /// The layout parameters used to format a log event.
        /// </summary>
        private readonly IEnumerable<LayoutParameter> layoutParameters;

        /// <summary>
        /// The property parser used to parse the properties in the layout.
        /// </summary>
        private readonly IPropertyParser propertyParser;

        /// <summary>
        /// Constructs a new instance of this standard layout, with the given parameters and property parser.
        /// </summary>
        public StandardLayout(IEnumerable<LayoutParameter> layoutParameters, IPropertyParser propertyParser)
        {
            this.layoutParameters = layoutParameters;

            this.propertyParser = propertyParser;
        }

        public string Format(LogEvent logEvent)
        {
            var messageBuilder = new StringBuilder();

            object parameterValue = null;
            string parameterString = string.Empty;
            foreach (var parameter in this.layoutParameters)
            {
                if (parameter.StaticText == false)
                {
                    // The parameter is not static text, let's grab the property

                    // Check for special parameters
                    parameterValue = GetSpecialParameter(logEvent, parameter);

                    // Try the meta data
                    parameterValue = parameterValue ?? this.propertyParser.GetProperty(logEvent.MetaData, parameter.Path);

                    // Try the log event itself
                    parameterValue = parameterValue ?? this.propertyParser.GetProperty(logEvent, parameter.Path);

                    // If the parameter was not handled as a special parameter, treat it as a normal parameter
                    parameterString = !parameter.Contingent || !IsNullOrEmptyString(parameterValue)
                        ? GetFormattedValue(parameterValue, parameter.Format)
                        : string.Empty;

                    messageBuilder.Append(parameterString);
                }
                else
                {
                    messageBuilder.Append(parameter.Text);
                }
            }

            return messageBuilder.ToString();
        }

        /// <summary>
        /// Checks for and returns special parameters. For example, the "Tags" parameter is returned
        /// as a CSV list of tags, Exceptions receive special formatting, etc.
        /// </summary>
        private static object GetSpecialParameter(LogEvent logEvent, LayoutParameter parameter)
        {
            return null;
        }

        /// <summary>
        /// Null-safe, string-converting null and empty check.
        /// </summary>
        private static bool IsNullOrEmptyString(object value)
        {
            return value == null
                || (typeof(string).IsAssignableFrom(value.GetType()) && string.IsNullOrEmpty((string)value));
        }

        /// <summary>
        /// Uses the property format to format the given value.
        /// </summary>
        private static string GetFormattedValue(object value, string format)
        {
            if (value != null)
            {
                if (string.IsNullOrEmpty(format) == false)
                    return string.Format(format, value);
                return value.ToString();
            }
            return null;
        }
    }
}