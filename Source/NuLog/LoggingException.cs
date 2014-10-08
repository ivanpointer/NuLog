/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/5/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using System;

namespace NuLog
{
    /// <summary>
    /// An exception used to represent exceptions occuring within the logging framework
    /// </summary>
    public class LoggingException : Exception
    {
        public LoggingException(string message) : base(message) { }

        public LoggingException(string message, Exception innerException) : base(message, innerException) { }
    }
}
