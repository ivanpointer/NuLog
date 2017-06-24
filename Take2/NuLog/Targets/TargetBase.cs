/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.LogEvents;

namespace NuLog.Targets
{
    /// <summary>
    /// A base target class with some helpers defined to make building out targets simpler.
    /// </summary>
    public abstract class TargetBase : ITarget
    {
        public virtual string Name { get; set; }

        public virtual void Configure(TargetConfig config)
        {
            // Does nothing
        }

        public abstract void Write(LogEvent logEvent);

        /// <summary>
        /// Gets the named property, casted to the indicated type. Returns default(TProperty) if the
        /// property isn't found in the config.
        /// </summary>
        protected TProperty GetProperty<TProperty>(TargetConfig config, string propertyName)
        {
            // If the config doesn't have any properties, return the default for the type
            if (config.Properties == null)
            {
                return default(TProperty);
            }

            // Check for the property, return the default if it isn't in the config
            if (config.Properties.ContainsKey(propertyName) == false)
            {
                return default(TProperty);
            }

            // The property exists, get it and see if we're compatible
            var property = config.Properties[propertyName];
            return (TProperty)property;
        }

        /// <summary>
        /// Tries to get the named property. If successful, returns True.
        /// </summary>
        protected bool TryGetProperty<TProperty>(TargetConfig config, string propertyName, out TProperty property)
        {
            // Check for the property, return the default if it isn't in the config
            if (config.Properties.ContainsKey(propertyName) == false)
            {
                property = default(TProperty);
                return false;
            }

            // The property exists, get it and see if we're compatible
            var obj = config.Properties[propertyName];
            var targetType = typeof(TProperty);
            if (!targetType.IsAssignableFrom(obj.GetType()))
            {
                property = default(TProperty);
                return false;
            }

            // The property is compatible, cast and return
            property = (TProperty)obj;
            return true;
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            // Eh, nothing to do..
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion IDisposable Support
    }
}