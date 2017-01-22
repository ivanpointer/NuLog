/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

namespace NuLog
{
    /// <summary>
    /// Defines the expected behavior of an activator registry.
    /// </summary>
    public interface IActivatorRegistry
    {
        /// <summary>
        /// Register a activator for the factory. This is here to specify (or override) the
        /// implementation of the activator (factory method).
        /// </summary>
        void RegisterActivator<TActivator, TConfiguration>(ILogFactoryActivator<TActivator, TConfiguration> activator);

        /// <summary>
        /// Register a activator for the factory. This is here to specify (or override) the
        /// implementation of the activator (factory method).
        /// </summary>
        void RegisterActivator<TActivator>(ILogFactoryActivator<TActivator> activator);

        /// <summary>
        /// Get the activator for the given activator, and configuration types.
        /// </summary>
        ILogFactoryActivator<TActivator, TConfiguration> GetActivator<TActivator, TConfiguration>();

        /// <summary>
        /// Get the activator for the given activator type.
        /// </summary>
        ILogFactoryActivator<TActivator> GetActivator<TActivator>();
    }
}