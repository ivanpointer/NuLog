/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;

namespace NuLog.Layouts {

    /// <summary>
    /// Defines the expected behavior of a layout parser.
    /// </summary>
    public interface ILayoutParser {

        /// <summary>
        /// Parses the given layout, returning a collection of layout parameters, representing static
        /// text and dynamic properties.
        /// </summary>
        /// <param name="format">The layout format to parse.</param>
        ICollection<LayoutParameter> Parse(string format);
    }
}