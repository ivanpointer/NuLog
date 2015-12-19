/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/8/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Configuration.Layouts;

namespace NuLog.Layouts
{
    /// <summary>
    /// Defines the expected behavior of a lyout
    /// </summary>
    public interface ILayout
    {
        /* Layouts are a mechanism for converting a log event into text using a "layout" format
         * Layouts are used by the standard text-based targets, and the SMTP target for the subject and the body
         * Layouts allow for the formatting of different parts of the log event, even recursively
         * Layouts allow for conditinoally showing formatted parts of the log event */

        /// <summary>
        /// Initializes the layout with the given layout config
        /// </summary>
        /// <param name="layoutConfig">The layout config for the layout</param>
        void Initialize(LayoutConfig layoutConfig);

        /// <summary>
        /// Formats the logging event into a string
        /// </summary>
        /// <param name="logEvent">The log event to format</param>
        /// <returns>The log event represented in string format</returns>
        string FormatLogEvent(LogEvent logEvent);
    }
}