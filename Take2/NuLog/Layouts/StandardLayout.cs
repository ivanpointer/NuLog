﻿/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.LogEvents;
using NuLog.Targets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NuLog.Layouts
{
    /// <summary>
    /// The standard implementation of a layout.
    /// </summary>
    public class StandardLayout : ILayout
    {
        /// <summary>
        /// The IEnumerable type, for checking when formatting values.
        /// </summary>
        private static readonly Type iEnumerableType = typeof(IEnumerable);

        /// <summary>
        /// The string type, for formatting values.
        /// </summary>
        private static readonly Type stringType = typeof(string);

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

            foreach (var parameter in this.layoutParameters)
            {
                messageBuilder.Append(FormatParameter(logEvent, parameter));
            }

            return messageBuilder.ToString();
        }

        /// <summary>
        /// Return a formatted string for the given parameter, on the given log event.
        /// </summary>
        private string FormatParameter(LogEvent logEvent, LayoutParameter parameter)
        {
            if (parameter.StaticText)
            {
                return parameter.Text;
            }
            else
            {
                // The parameter is not static text, let's grab the property

                // Check for special parameters
                var parameterValue = GetSpecialParameter(logEvent, parameter);

                // Try the meta data
                parameterValue = parameterValue ?? this.propertyParser.GetProperty(logEvent.MetaData, parameter.Path);

                // Try the log event itself
                parameterValue = parameterValue ?? this.propertyParser.GetProperty(logEvent, parameter.Path);

                // If the parameter was not handled as a special parameter, treat it as a normal parameter
                var parameterString = !parameter.Contingent || !IsNullOrEmptyString(parameterValue)
                    ? GetFormattedValue(parameterValue, parameter.Format)
                    : string.Empty;

                return parameterString;
            }
        }

        /// <summary>
        /// Checks for and returns special parameters. For example, the "Tags" parameter is returned
        /// as a CSV list of tags, Exceptions receive special formatting, etc.
        /// </summary>
        protected virtual object GetSpecialParameter(LogEvent logEvent, LayoutParameter parameter)
        {
            switch (parameter.Path)
            {
                case "Tags":
                    return string.Join(",", logEvent.Tags);

                case "Exception":
                    return FormatException(logEvent.Exception);

                default:
                    return null;
            }
        }

        /// <summary>
        /// Formats an exception, recursing down the causing exceptions.
        /// </summary>
        private static string FormatException(Exception exception)
        {
            var sb = new StringBuilder();

            bool inner = false;
            while (exception != null)
            {
                sb.Append(string.Format("{0}{1}: {2}\r\n", inner ? "Caused by " : "", exception.GetType().FullName, exception.Message));
                sb.Append(string.Format("{0}\r\n", exception.StackTrace));
                exception = exception.InnerException;
                inner = true;
            }

            return sb.ToString();
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
            // If we have a string format, use that.
            if (string.IsNullOrWhiteSpace(format) == false)
            {
                return string.Format(format, value);
            }

            // If not, check to see if it's enumerable (but not a string).
            var valueType = value.GetType();

            if (stringType.IsAssignableFrom(valueType) || iEnumerableType.IsAssignableFrom(valueType) == false)
            {
                // It's a string, or it's not enumerable, just convert it to a string.
                return Convert.ToString(value);
            }
            else
            {
                // It's enumerable, enumerate it and wrap in brackets.
                return string.Format("[{0}]", string.Join(",", EnumerateValues((IEnumerable)value)));
            }
        }

        /// <summary>
        /// Enumerates the values in the given enumerable value.
        /// </summary>
        private static IEnumerable<string> EnumerateValues(IEnumerable items)
        {
            foreach (var item in items)
            {
                yield return Convert.ToString(item);
            }
        }
    }
}