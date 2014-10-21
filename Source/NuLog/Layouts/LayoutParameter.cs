/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/8/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using System.Collections.Generic;

namespace NuLog.Layouts
{
    /// <summary>
    /// Represents a single paramater within a standard layout.  Designed specifically for the StandardLayout, but can be used by other layouts.
    /// </summary>
    public class LayoutParameter
    {
        /// <summary>
        /// Whether or not this parameter represents static text within a layout format
        /// </summary>
        public bool StaticText { get; set; }

        /// <summary>
        /// The static text of the parameter if it is a static text parameter
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The format to be applied to the value of the parameter
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Whether or not the parameter is included in the resulting string if no value is found for the parameter
        /// </summary>
        public bool Contingent { get; set; }

        /// <summary>
        /// The full name of the parameter.  A single string split by periods for each of the names.  Used to drill down through the log event to find the value.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// The broken-down name of the parameter.  Used to drill down through the log event to find the value.
        /// </summary>
        public ICollection<string> NameList { get; set; }
        
        /// <summary>
        /// Standard constructor.  Defaults to "false" as a "StaticText", and initializes the name lest to an empty list
        /// </summary>
        public LayoutParameter()
        {
            StaticText = false;
            NameList = new List<string>();
        }
    }
}
