/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

namespace NuLog.Targets
{
    /// <summary>
    /// Defines the expected behavior of a layout factory.
    /// </summary>
    public interface ILayoutFactory
    {
        /// <summary>
        /// Builds a new layout for the given format.
        /// </summary>
        ILayout GetLayout(string format);
    }
}