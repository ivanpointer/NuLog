/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/5/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using System;
using System.Collections.Generic;

namespace NuLog.Configuration.Targets
{
    /// <summary>
    /// Represents a color rule 
    /// </summary>
    public class ConsoleColorRule
    {
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
