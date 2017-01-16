/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Targets;

namespace NuLog.Layouts
{
    /// <summary>
    /// The standard implementation of a layout.
    /// </summary>
    public class StandardLayout : ILayout
    {
        /// <summary>
        /// The layout for this layout.
        /// </summary>
        private readonly string layout;

        public StandardLayout(string layout)
        {
            this.layout = layout;
        }

        public string Format(ILogEvent logEvent)
        {
            return this.layout;
        }
    }
}