/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;

namespace NuLog.Activators
{
    /// <summary>
    /// Implements some base helper methods to make implementing the log factory activator interface simpler.
    /// </summary>
    public abstract class LogFactoryActivatorBase<TActivator, TConfiguration> : ILogFactoryActivator<TActivator, TConfiguration>
    {
        private readonly Type activatorType;

        private readonly Type configurationType;

        public LogFactoryActivatorBase()
        {
            this.activatorType = typeof(TActivator);
            this.configurationType = typeof(TConfiguration);
        }

        public abstract TActivator BuildNew(TConfiguration config);

        public Type GetActivatorType()
        {
            return activatorType;
        }

        public Type GetConfigurationType()
        {
            return configurationType;
        }
    }

    public abstract class LogFactoryActivatorBase<TActivator> : ILogFactoryActivator<TActivator>
    {
        private readonly Type activatorType;

        public LogFactoryActivatorBase()
        {
            this.activatorType = typeof(TActivator);
        }

        public abstract TActivator BuildNew();

        public TActivator BuildNew(LogFactoryActivatorNull config)
        {
            throw new InvalidOperationException("Cannot call \"BuildNew\" on a configuration-less log factory activator.");
        }

        public Type GetActivatorType()
        {
            return activatorType;
        }

        public Type GetConfigurationType()
        {
            return LogFactoryActivatorNull.NullType;
        }
    }
}