/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

namespace NuLog.Layouts {

    /// <summary>
    /// Represents a single parameter within a standard layout. Designed specifically for the
    /// StandardLayout, but can be used by other layouts.
    /// </summary>
    public class LayoutParameter {

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
        /// Whether or not the parameter is included in the resulting string if no value is found for
        /// the parameter
        /// </summary>
        public bool Contingent { get; set; }

        /// <summary>
        /// The full path (name) of the parameter. A single string split by periods for each of the
        /// names. Used to drill down through the log event to find the value.
        /// </summary>
        public string Path { get; set; }
    }
}