/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/08/2014
 * Updated 11/16/2014 - Added a couple extra constructors for building out the rule, including one to parse a string value into a ConsoleColor value.
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace NuLog.Configuration.Targets
{
    /// <summary>
    /// Represents a color rule
    /// </summary>
    public class ConsoleColorRule
    {
        #region Constants

        private static readonly Type ConsoleColorType = typeof(ConsoleColor);

        #endregion Constants

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

            BackgroundColor = (ConsoleColor)Enum.Parse(ConsoleColorType, backgroundColor);
            ForegroundColor = (ConsoleColor)Enum.Parse(ConsoleColorType, foregroundColor);
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
    }
}