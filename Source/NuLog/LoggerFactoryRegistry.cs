/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;

namespace NuLog
{
    /// <summary>
    /// A registry of all logger factories, used to shutdown all at once.
    /// </summary>
    public static class LoggerFactoryRegistry
    {
        private static readonly object _loggerFactoryRegistryLock = new object();
        private static readonly List<LoggerFactory> _loggerFactories = new List<LoggerFactory>();

        /// <summary>
        /// Registers a given logger factory in the registry.
        /// </summary>
        public static void Register(LoggerFactory loggerFactory)
        {
            lock (_loggerFactoryRegistryLock)
                _loggerFactories.Add(loggerFactory);
        }

        /// <summary>
        /// Removes the logger factory from this registry - presumably after it's been shutdown.
        /// </summary>
        public static void Deregister(LoggerFactory loggerFactory)
        {
            lock (_loggerFactoryRegistryLock)
                if (_loggerFactories.Contains(loggerFactory))
                    _loggerFactories.Remove(loggerFactory);
        }

        /// <summary>
        /// Shuts down all registered logger factories, and clears the list of registered logger factories.
        /// </summary>
        public static void ShutdownAll()
        {
            lock (_loggerFactoryRegistryLock)
            {
                // Take a copy of our logger factory list - we cannot edit it while iterating over it;
                //  Factories remove themselves from this registry when they shutdown
                var copy = new LoggerFactory[_loggerFactories.Count];
                _loggerFactories.CopyTo(copy);

                // Iterate over the known factories, telling them to shutdown
                foreach (var factory in copy)
                {
                    factory.Shutdown();
                }

                // This should be empty, but...
                _loggerFactories.Clear();
            }
        }
    }
}