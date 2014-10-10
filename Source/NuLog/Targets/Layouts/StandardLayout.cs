/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/8/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using NuLog.Configuration.Layouts;
using NuLog.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace NuLog.Targets.Layouts
{
    /// <summary>
    /// The default/standard layout for the framework
    /// </summary>
    public class StandardLayout : ILayout
    {
        /* Documentation / Explanation:
         * 
         * Layouts are a mechanism for converting a log event into text using a "layout" format
         * Layouts are used by the standard text-based targets, and the SMTP target for the subject and the body
         * Layouts allow for the formatting of different parts of the log event, even recursively
         * Layouts allow for conditinoally showing formatted parts of the log event
         * 
         * "Hello Layout${?DateTime:': {0:MM/dd/yyyy hh:mm:ss.fff}'}!\r\n"
         * 
         * Static Text:
         *  - Anything not wrapped in a parameter enclosure ${} is treated as static text
         *  - Static text will always show in a log event formatted by a layout
         *  - Escaped characters (such as carriage return and line feed) are supported, and encouraged
         *  
         * Parametrers:
         *  ${?DateTime:': {0:MM/dd/yyyy hh:mm:ss.fff}'}
         *    
         *  - Parameters are wrapped with the property enclosure ${}
         *  - A single parameter in the layout format refers to a single property of the log event
         *  - Parameters have three parts:
         *    - Conditional Flag (Optional)
         *    - Property Name (Required)
         *    - Property Format (Optional)
         *    
         *  - Conditional Flag:
         *    - The conditional flag is a single '?' located at the front of the property, inside the enclosure ${}
         *    - If the conditional flag is present, the property will only be included in the resulting text if the property is not null or empty
         *    
         *  - Property Name:
         *    - The name of the proeprty within the log event is located at the beginning of the proeprty string, after the conditional flag
         *    - All log events have optional "Meta Data"
         *    - The Property Name value is reflective and recursive, child values can be accessed with periods, for example: DateTime.Day
         *    - The "Meta Data" is searched first for the property
         *    - The log event is searched for the property, if the property is not found in the "Meta Data"
         *    
         *  - Property Format:
         *    - The property format is used to format the value of the proeprty which was evaluated from the log event
         *    - The property format is separated from the property name by a colon ':'
         *    - The property format is wrapped in single quotes to allow for escaping within the format string
         *    - The framework uses System.String.Format with the property format and value
         */

        #region Constants

        private const string ParameterPattern = @"\${[^}:\s]+(:'([^']|(\\'))+')?}";
        private const string ParameterNamePattern = @"\$\{[^:|}]*";
        private const string ParameterFormatPattern = @":[^}]*(}.*')?";

        #endregion

        #region Members, Constructors and Initialization

        // The types are cached because using reflection to pull the PropertyInfo is expensive
        private IDictionary<Type, PropertyInfo[]> TypeCache { get; set; }

        // We cache the parsed layouts, because parsing them is expensive
        private IDictionary<string, LayoutParameter[]> LayoutCache { get; set; }

        // The layout configuration for this layout
        private LayoutConfig Config { get; set; }

        /// <summary>
        /// The default constructor creating an empty standard layout
        /// </summary>
        public StandardLayout()
        {
            TypeCache = new Dictionary<Type, PropertyInfo[]>();
            LayoutCache = new Dictionary<string, LayoutParameter[]>();
            Config = new LayoutConfig();
        }

        /// <summary>
        /// Builds the standard layout using the provided layout config
        /// </summary>
        /// <param name="layoutConfig">The layout config to use to build the standard layout</param>
        public StandardLayout(LayoutConfig layoutConfig)
        {
            TypeCache = new Dictionary<Type, PropertyInfo[]>();
            LayoutCache = new Dictionary<string, LayoutParameter[]>();
            Initialize(layoutConfig);
        }

        /// <summary>
        /// Builds the standard layout using the provided string format
        /// </summary>
        /// <param name="format">The string format to use to build this standard layout</param>
        public StandardLayout(string format)
        {
            TypeCache = new Dictionary<Type, PropertyInfo[]>();
            LayoutCache = new Dictionary<string, LayoutParameter[]>();
            Config = new LayoutConfig(format);
        }

        /// <summary>
        /// Initializes this standard layout using the passed logging config
        /// </summary>
        /// <param name="layoutConfig">The layout config to initialize this standard layout with</param>
        public void Initialize(LayoutConfig layoutConfig)
        {
            Config = layoutConfig;
            LayoutCache.Clear();
        }

        #endregion

        #region Formatting (Log Event Formatting)

        /// <summary>
        /// Formats the given log event into a string using the layout format
        /// </summary>
        /// <param name="logEvent">The log event to format into a string</param>
        /// <returns>The formatted log event in string format</returns>
        public string FormatLogEvent(LogEvent logEvent)
        {
            string message = Config.Format;
            if (logEvent != null)
            {
                // Parse the parameters from the format, and iterate over each one, building it into a string builder
                var parameters = ParseParameters(message);
                if (parameters.Length > 0)
                {
                    StringBuilder builtMessage = new StringBuilder();

                    object parameterValue;
                    string parameterString;
                    foreach (var parameter in parameters)
                    {
                        if (parameter.StaticText == false)
                        {
                            // If the parameter is not a static block of text,

                            // Check for special parameters that need special consideration for formatting, such as tag lists and exceptions
                            parameterValue = GetSpecialParameter(parameter, logEvent);
                            parameterValue = parameterValue ?? MetaDataParser.GetProperty(logEvent, parameter.NameList);

                            // If the paramater was not handled as a special parameter, treat it as a normal parameter
                            parameterString = !parameter.Contingent || !IsNullOrEmptyString(parameterValue)
                                ? GetFormattedValue(parameterValue, parameter.Format)
                                : String.Empty;

                            builtMessage.Append(parameterString);
                        }
                        else
                        {
                            builtMessage.Append(parameter.Text);
                        }
                    }

                    message = builtMessage.ToString();
                }
            }

            return message;
        }

        // Formats special parameters in the log event, such as tags and exceptions
        private static string GetSpecialParameter(LayoutParameter parameter, LogEvent logEvent)
        {
            switch (parameter.FullName)
            {
                case "Tags":
                    return logEvent.Tags != null && logEvent.Tags.Count > 0
                        ? String.Join(",", logEvent.Tags.ToArray())
                        : null;
                case "Exception":
                    return FormatException(logEvent.Exception);
            }

            return null;
        }

        // Formats an exception, recursing down the causing exceptions
        private static string FormatException(Exception exception)
        {
            StringBuilder sb = new StringBuilder();

            bool inner = false;
            while (exception != null)
            {
                sb.Append(String.Format("{0}{1}: {2}\r\n", inner ? "Caused by " : "", exception.GetType().FullName, exception.Message));
                sb.Append(String.Format("{0}\r\n", exception.StackTrace));
                exception = exception.InnerException;
                inner = true;
            }

            return sb.ToString();
        }

        // Uses the property format to format the given value
        private string GetFormattedValue(object value, string format)
        {
            if (value != null)
            {
                if (String.IsNullOrEmpty(format) == false)
                    return String.Format(format, value);
                return value.ToString();
            }
            return null;
        }

        #endregion

        #region Parameter Parsing

        /// <summary>
        /// Parses the passed string format and pulls out the parameter texts: E.G., ${Parameter}
        /// </summary>
        /// <param name="format">The format to pull the parameters from</param>
        /// <returns>A list of found parameters in the format</returns>
        public static ICollection<string> FindParameterTexts(string format)
        {
            var list = new List<string>();
            var matches = Regex.Matches(format, ParameterPattern);
            foreach (var match in matches)
                list.Add(match.ToString());
            return list;
        }

        // Parses the string format, returning a collection of layout parameters (actual properties and static text) to use for formatting log events
        private LayoutParameter[] ParseParameters(string format)
        {
            // Check to see if we have alread parsed out the parameters for the format
            if (!LayoutCache.ContainsKey(format))
            {
                try
                {
                    // Find and use the the parameter texts to build out each of the layout parameters
                    var parameters = new List<LayoutParameter>();
                    var parameterTexts = FindParameterTexts(format);

                    string runningFormat = format;
                    int parameterIndex;
                    string staticText;

                    // Use the parameter texts to "split" the static text out of the format
                    //  alternating between "static text" and the actual parameter.  If it so
                    //  happens that the alternating "static text" items are empty, they are ignored
                    foreach (var parameterText in parameterTexts)
                    {
                        parameterIndex = runningFormat.IndexOf(parameterText, StringComparison.Ordinal);
                        if (parameterIndex > 0)
                        {
                            staticText = runningFormat.Substring(0, parameterIndex);
                            parameters.Add(new LayoutParameter
                            {
                                StaticText = true,
                                Text = staticText
                            });
                        }
                        runningFormat = runningFormat.Substring(parameterIndex + parameterText.Length);

                        parameters.Add(ParseParameter(parameterText));
                    }

                    // Catch the "static text" at the end of the format
                    if (String.IsNullOrEmpty(runningFormat) == false)
                    {
                        parameters.Add(new LayoutParameter
                        {
                            StaticText = true,
                            Text = runningFormat
                        });
                    }

                    // Cache the results
                    LayoutCache[format] = parameters.ToArray();
                }
                catch (Exception e)
                {
                    throw new LoggingException(String.Format("Failed to parse layout format {0}", format), e);
                }
            }

            return LayoutCache[format];
        }

        // Parses a layout parameter from the given text
        private static LayoutParameter ParseParameter(string text)
        {
            // Initialize the layout parameter with a temporary full name and text
            var parm = new LayoutParameter
            {
                Text = text,
                FullName = Regex.Match(text, ParameterNamePattern).ToString()
            };

            // Figure the contingent flag and strip down the full name to remove the opening bracket and contingent flag
            parm.Contingent = parm.FullName[2] == '?';
            parm.FullName = parm.Contingent
                ? parm.FullName.Substring(3)
                : parm.FullName.Substring(2);

            // Spllit out the full name into its parts
            var nameParts = parm.FullName.Split('.');
            foreach (var namePart in nameParts)
                parm.NameList.Add(namePart);

            // Split out and clean up the parameter format
            parm.Format = Regex.Match(text, ParameterFormatPattern).ToString();
            parm.Format = !String.IsNullOrEmpty(parm.Format)
                ? parm.Format.Substring(1)
                : null;

            parm.Format = !String.IsNullOrEmpty(parm.Format) && parm.Format.StartsWith("'") && parm.Format.EndsWith("'")
                ? parm.Format.Substring(1, parm.Format.Length - 2)
                : parm.Format;

            parm.Format = !String.IsNullOrEmpty(parm.Format)
                ? parm.Format.Replace("\\'", "'").Replace("\\\\", "\\")
                : parm.Format;

            return parm;
        }

        #endregion

        #region Helpers

        // null-safe, string-converting null and empty check
        private static bool IsNullOrEmptyString(object value)
        {
            return value == null
                || (typeof(string).IsAssignableFrom(value.GetType()) && String.IsNullOrEmpty((string)value));
        }

        #endregion
    }
}
