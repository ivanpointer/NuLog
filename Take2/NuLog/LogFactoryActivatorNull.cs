/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;

namespace NuLog
{
    /// <summary>
    /// A "null object" pattern to allow for simplifying the activators down from requiring a config,
    /// to not needing one.
    /// </summary>
    public sealed class LogFactoryActivatorNull
    {
        /// <summary>
        /// The null type.
        /// </summary>
        public static readonly Type NullType = typeof(LogFactoryActivatorNull);

        /// <summary>
        /// Used to represent a configuration-less ILogFactoryActivator.
        /// </summary>
        public static readonly LogFactoryActivatorNull NullObject = new LogFactoryActivatorNull();

        // Private constructor - there should only be one instance
        private LogFactoryActivatorNull()
        {
        }
    }
}