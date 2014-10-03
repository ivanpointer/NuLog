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
    public class StandardLayout : ILayout
    {
        private IDictionary<Type, PropertyInfo[]> TypeCache { get; set; }
        private IDictionary<string, LayoutParameter[]> LayoutCache { get; set; }
        private LayoutConfig Config { get; set; }

        public StandardLayout()
        {
            TypeCache = new Dictionary<Type, PropertyInfo[]>();
            LayoutCache = new Dictionary<string, LayoutParameter[]>();
            Config = new LayoutConfig();
        }

        public StandardLayout(LayoutConfig layoutConfig)
        {
            TypeCache = new Dictionary<Type, PropertyInfo[]>();
            LayoutCache = new Dictionary<string, LayoutParameter[]>();
            Initialize(layoutConfig);
        }

        public StandardLayout(string format)
        {
            TypeCache = new Dictionary<Type, PropertyInfo[]>();
            LayoutCache = new Dictionary<string, LayoutParameter[]>();
            Config = new LayoutConfig(format);
        }

        public void Initialize(LayoutConfig layoutConfig)
        {
            Config = layoutConfig;
        }

        public string FormatLogEvent(LogEvent logEventInfo)
        {
            string message = Config.Format;
            if (logEventInfo != null)
            {
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
                            parameterValue = GetSpecialParameter(parameter, logEventInfo);
                            parameterValue = parameterValue ?? MetaDataParser.GetProperty(logEventInfo, parameter.NameList);

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

        private string GetFormattedValue(object value, string format)
        {
            // This was refactored using ReSharper, while I am not happy with the resulting
            //  code creadability (this isn't that bad really), we are aiming for performance
            //  here.
            if (value != null)
            {
                if (String.IsNullOrEmpty(format) == false)
                    return String.Format(format, value);
                return value.ToString();
            }
            return null;
        }

        private static bool IsNullOrEmptyString(object value)
        {
            return value == null
                || (typeof(string).IsAssignableFrom(value.GetType()) && String.IsNullOrEmpty((string)value));
        }

        private LayoutParameter[] ParseParameters(string format)
        {
            if (!LayoutCache.ContainsKey(format))
            {
                try
                {
                    var parameters = new List<LayoutParameter>();
                    var parameterTexts = FindParameterTexts(format);

                    string runningFormat = format;
                    int parameterIndex;
                    string staticText;

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

                    if (String.IsNullOrEmpty(runningFormat) == false)
                    {
                        parameters.Add(new LayoutParameter
                        {
                            StaticText = true,
                            Text = runningFormat
                        });
                    }

                    LayoutCache[format] = parameters.ToArray();
                }
                catch (Exception e)
                {
                    throw new LoggingException(String.Format("Failed to parse layout format {0}", format), e);
                }
            }

            return LayoutCache[format];
        }

        public static ICollection<string> FindParameterTexts(string format)
        {
            var list = new List<string>();
            var matches = Regex.Matches(format, @"\${[^}:\s]+(:'([^']|(\\'))+')?}");
            foreach (var match in matches)
                list.Add(match.ToString());
            return list;
        }

        private static LayoutParameter ParseParameter(string text)
        {
            var parm = new LayoutParameter
            {
                Text = text,
                FullName = Regex.Match(text, @"\$\{[^:|}]*").ToString()
            };

            parm.Contingent = parm.FullName[2] == '?';
            parm.FullName = parm.Contingent
                ? parm.FullName.Substring(3)
                : parm.FullName.Substring(2);

            var nameParts = parm.FullName.Split('.');
            foreach (var namePart in nameParts)
                parm.NameList.Add(namePart);

            parm.Format = Regex.Match(text, ":[^}]*(}.*')?").ToString();
            parm.Format = !String.IsNullOrEmpty(parm.Format)
                ? parm.Format.Substring(1)
                : null;

            // ReSharper disable once PossibleNullReferenceException - This is already checked immediately before with String.IsNullOrEmpty
            parm.Format = !String.IsNullOrEmpty(parm.Format) && parm.Format.StartsWith("'") && parm.Format.EndsWith("'")
                ? parm.Format.Substring(1, parm.Format.Length - 2)
                : parm.Format;

            parm.Format = !String.IsNullOrEmpty(parm.Format)
                // ReSharper disable once PossibleNullReferenceException - this is checked the line before with String.IsNullOrEmpty
                ? parm.Format.Replace("\\'", "'").Replace("\\\\", "\\")
                : parm.Format;

            return parm;
        }

    }
}
