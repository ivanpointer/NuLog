/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/08/2014
 * Updated 11/16/2014 - Added a couple extra constructors for building out the rule, including one to parse a string value into a ConsoleColor value.
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NuLog.Configuration.Targets
{
    /// <summary>
    /// Represents a color rule 
    /// </summary>
    public class ConsoleColorRule
    {

        #region Constants

        private const string CannotParseColorMessage = "Failed to parse console color \"{0}\" because {1}";

        private static readonly Type ConsoleColorType = typeof(ConsoleColor);

        #endregion

        /// <summary>
        /// Default constructor.  Creates an empty color rule.
        /// </summary>
        public ConsoleColorRule()
        {
            Tags = new List<string>();
            ForegroundColor = null;
            BackgroundColor = null;
        }

        /// <summary>
        /// Constructor to set given colors to the given tags
        /// </summary>
        /// <param name="backgroundColor">The background ConsoleColor to set</param>
        /// <param name="foregroundColor">The foreground ConsoleColor to set</param>
        /// <param name="tags">The tags that are used to match against for the given colors</param>
        public ConsoleColorRule(ConsoleColor backgroundColor, ConsoleColor foregroundColor, params string[] tags)
        {
            Tags = tags != null
                ? tags.ToList()
                : new List<string>();

            BackgroundColor = backgroundColor;
            ForegroundColor = foregroundColor;
        }

        /// <summary>
        /// Constructor to set given colors (in string form) to the given tags
        /// </summary>
        /// <param name="backgroundColor">The string representation of the background ConsoleColor to set</param>
        /// <param name="foregroundColor">The string representation of the foreground ConsoleColor to set</param>
        /// <param name="tags">The tags that are used to match against for the given colors</param>
        public ConsoleColorRule(string backgroundColor, string foregroundColor, params string[] tags)
        {
            Tags = tags != null
                ? tags.ToList()
                : new List<string>();

            BackgroundColor = GetConsoleColor(backgroundColor);
            ForegroundColor = GetConsoleColor(foregroundColor);
        }

        /// <summary>
        /// The tags that match this color rule
        /// </summary>
        public ICollection<string> Tags { get; set; }

        /// <summary>
        /// The foreground color applied by this rule
        /// </summary>
        public ConsoleColor? ForegroundColor { get; set; }

        /// <summary>
        /// The background color applied by this rule
        /// </summary>
        public ConsoleColor? BackgroundColor { get; set; }

        #region Helpers

        // Converts the given string into a ConsoleColor, returns null if it fails
        private static ConsoleColor? GetConsoleColor(string consoleColorString)
        {
            ConsoleColor? result = null;

            try
            {
                result = (ConsoleColor)Enum.Parse(ConsoleColorType, consoleColorString);
            }
            catch(Exception cause)
            {
                Trace.WriteLine(String.Format(CannotParseColorMessage, consoleColorString, cause));
            }

            return result;
        }

        #endregion
    }
}
