/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

namespace NuLog.Targets
{
    /// <summary>
    /// Defines the expected behavior of a layout.
    /// </summary>
    public interface ILayout
    {
        /// <summary>
        /// Convert the given log event into a string representation.
        /// </summary>
        /// <param name="logEvent">The log event to convert to a string representation.</param>
        /// <returns>A string representation of the log event.</returns>
        string Format(ILogEvent logEvent);
    }
}