/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using System;
using System.Collections.Generic;

namespace NuLog.Factories
{
    /// <summary>
    /// The standard implementation of the log factory.
    /// </summary>
    public class StandardLogFactory : ILogFactory, IActivatorRegistry
    {
        ///// <summary>
        ///// The activators registered to this log factory.
        ///// </summary>
        private readonly IDictionary<Type, IDictionary<Type, ILogFactoryActivator>> activators;

        public StandardLogFactory()
        {
            this.activators = new Dictionary<Type, IDictionary<Type, ILogFactoryActivator>>();
        }

        public void RegisterActivator<TActivator>(ILogFactoryActivator<TActivator> activator)
        {
            RegisterActivatorInternal(activator);
        }

        public void RegisterActivator<TActivator, TConfiguration>(ILogFactoryActivator<TActivator, TConfiguration> activator)
        {
            RegisterActivatorInternal(activator);
        }

        private void RegisterActivatorInternal(ILogFactoryActivator activator)
        {
            // Get the inner dictionary for the activator type
            var activatorType = activator.GetActivatorType();
            if (activators.ContainsKey(activatorType) == false)
            {
                activators[activatorType] = new Dictionary<Type, ILogFactoryActivator>();
            }
            var activatorDict = activators[activatorType];

            // Get the activator for the configuration type expected
            var configurationType = activator.GetConfigurationType();
            activatorDict[configurationType] = activator;
        }

        public ILogFactoryActivator<TActivator> GetActivator<TActivator>()
        {
            var activatorType = typeof(TActivator);
            return (ILogFactoryActivator<TActivator>)GetActivatorInternal(activatorType, LogFactoryActivatorNull.NullType);
        }

        public ILogFactoryActivator<TActivator, TConfiguration> GetActivator<TActivator, TConfiguration>()
        {
            var activatorType = typeof(TActivator);
            var configurationType = typeof(TConfiguration);
            return (ILogFactoryActivator<TActivator, TConfiguration>)GetActivatorInternal(activatorType, configurationType);
        }

        private ILogFactoryActivator GetActivatorInternal(Type activatorType, Type configType)
        {
            var dict = this.activators[activatorType];
            var activator = dict[configType];

            return activator;
        }

        public ICollection<ITarget> GetTargets(IEnumerable<TargetConfig> targetConfigs)
        {
            throw new NotImplementedException();
        }

        //private ITarget BuildTarget(TargetConfig config)
        //{
        //    // Use the activator to create a new instance.
        //    var type = GetTargetType(config);
        //    var instance = (ITarget)Activator.CreateInstance(type);

        // // Set the target's name instance.Name = config.Name;

        // // Tell the target to configure itself instance.Configure(config);

        //    // Return the instance
        //    return instance;
        //}

        ///// <summary>
        ///// Returns the type for the given target config. Uses a dictionary to cache types to reduce
        ///// the cost of looking up the type.
        ///// </summary>
        //private Type GetTargetType(TargetConfig config)
        //{
        //    if (typeCache.ContainsKey(config.Type) == false)
        //    {
        //        typeCache[config.Type] = Type.GetType(config.Type);
        //    }

        //    return typeCache[config.Type];
        //}
    }
}