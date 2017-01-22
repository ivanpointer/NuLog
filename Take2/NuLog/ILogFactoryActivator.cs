/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;

namespace NuLog
{
    /// <summary>
    /// Defines the expected behavior of a log factory activator.
    ///
    /// Log factory activators are responsible for creating instances of the various pieces of NuLog
    /// - these are the "methods" of a sort of a factory method pattern.
    /// </summary>
    /// <typeparam name="TActivator">The type of the object being created.</typeparam>
    /// <typeparam name="TConfiguration">
    /// The type of configuration passed to the activator, when activated.
    /// </typeparam>
    public interface ILogFactoryActivator<TActivator, TConfiguration> : ILogFactoryActivator
    {
        /// <summary>
        /// Creates a new "thing" of type TNewObject, using the passed TConfiguration config item.
        /// </summary>
        TActivator BuildNew(TConfiguration config);
    }

    /// <summary>
    /// Defines the expected behavior of a activator-type only log factory activator.
    /// </summary>
    /// <typeparam name="TActivator">The type of the thing being created.</typeparam>
    public interface ILogFactoryActivator<TActivator> : ILogFactoryActivator<TActivator, LogFactoryActivatorNull>
    {
        TActivator BuildNew();
    }

    /// <summary>
    /// Defines the expected behavior of the base log factory activator. This isn't actually really
    /// used, except to enforce some kind of order to the activators; especially in registration with
    /// a log factory.
    /// </summary>
    public interface ILogFactoryActivator
    {
        Type GetActivatorType();

        Type GetConfigurationType();
    }
}