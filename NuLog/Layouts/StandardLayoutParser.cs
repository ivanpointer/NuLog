/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NuLog.Layouts {

    /// <summary>
    /// The standard implementation of a layout parser.
    /// </summary>
    public class StandardLayoutParser : ILayoutParser {
        private static readonly Regex parameterPattern = new Regex(@"\${[^}:\s]+(:'([^']|(\\'))+')?}", RegexOptions.Compiled);
        private static readonly Regex parameterNamePattern = new Regex(@"\$\{[^:|}]*", RegexOptions.Compiled);
        private static readonly Regex parameterFormatPattern = new Regex(@":[^}]*(}.*')?", RegexOptions.Compiled);

        public ICollection<LayoutParameter> Parse(string format) {
            // Find and use the parameter texts to build out each of the layout parameters
            var parameters = new List<LayoutParameter>();
            var parameterTexts = FindParameterTexts(format);

            string runningFormat = format;
            int parameterIndex;
            string staticText;

            // Use the parameter texts to "split" the static text out of the format alternating
            // between "static text" and the actual parameter. If it so happens that the alternating
            // "static text" items are empty, they are ignored
            foreach (var parameterText in parameterTexts) {
                parameterIndex = runningFormat.IndexOf(parameterText, StringComparison.Ordinal);
                if (parameterIndex > 0) {
                    staticText = runningFormat.Substring(0, parameterIndex);
                    parameters.Add(new LayoutParameter {
                        StaticText = true,
                        Text = staticText
                    });
                }
                runningFormat = runningFormat.Substring(parameterIndex + parameterText.Length);

                parameters.Add(ParseParameter(parameterText));
            }

            // Catch the "static text" at the end of the format
            if (!string.IsNullOrEmpty(runningFormat)) {
                parameters.Add(new LayoutParameter {
                    StaticText = true,
                    Text = runningFormat
                });
            }

            // Return what we've got
            return parameters;
        }

        /// <summary>
        /// Parses a layout parameter from the given text
        /// </summary>
        private static LayoutParameter ParseParameter(string text) {
            // Initialize the layout parameter with a temporary full name and text
            var parm = new LayoutParameter {
                Text = text,
                Path = parameterNamePattern.Match(text).ToString()
            };

            // Figure the contingent flag and strip down the full name to remove the opening bracket
            // and contingent flag
            parm.Contingent = parm.Path[2] == '?';
            parm.Path = parm.Contingent
                ? parm.Path.Substring(3)
                : parm.Path.Substring(2);

            // Split out and clean up the parameter format
            parm.Format = parameterFormatPattern.Match(text).ToString();
            parm.Format = !string.IsNullOrEmpty(parm.Format)
                ? parm.Format.Substring(1)
                : null;

            parm.Format = !string.IsNullOrEmpty(parm.Format) && parm.Format.StartsWith("'") && parm.Format.EndsWith("'")
                ? parm.Format.Substring(1, parm.Format.Length - 2)
                : parm.Format;

            parm.Format = !string.IsNullOrEmpty(parm.Format)
                ? parm.Format.Replace("\\'", "'").Replace("\\\\", "\\")
                : parm.Format;

            return parm;
        }

        /// <summary>
        /// Parses the passed string format and pulls out the parameter texts: E.G., ${Parameter}
        /// </summary>
        /// <param name="format">The format to pull the parameters from</param>
        /// <returns>A list of found parameters in the format</returns>
        private static ICollection<string> FindParameterTexts(string format) {
            var list = new List<string>();
            var matches = parameterPattern.Matches(format);
            foreach (var match in matches)
                list.Add(match.ToString());
            return list;
        }
    }
}