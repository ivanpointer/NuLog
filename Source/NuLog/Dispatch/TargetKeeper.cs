/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/7/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Configuration;
using NuLog.Targets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace NuLog.Dispatch
{
    /// <summary>
    /// Responsible for matching tags to each other using then configured tag groups and wildcards
    /// </summary>
    public class TargetKeeper : IConfigObserver
    {
        #region Members and Constructors

        // Configuration
        private static readonly object _configLock = new object();

        private LoggingConfig LoggingConfig { get; set; }

        // Targets
        private static readonly object _targetLock = new object();

        private IDictionary<string, TargetBase> _targets;
        private IDictionary<string, ICollection<TargetBase>> _compiledTargets;

        public IDictionary<string, TargetBase> Targets
        {
            get
            {
                lock (_targetLock)
                {
                    var copy = new Dictionary<string, TargetBase>();

                    foreach (var original in _targets.Keys)
                        copy.Add(original, _targets[original]);

                    return copy;
                }
            }
        }

        // Ouside Stuff
        protected LogEventDispatcher Dispatcher { get; set; }

        public Action<Exception, string> ExceptionHandler { get; set; }

        /// <summary>
        /// Builds this target keeper attached to the dispatcher
        /// </summary>
        /// <param name="dispatcher">The dispatcher this target keeper is associated with</param>
        public TargetKeeper(LogEventDispatcher dispatcher)
        {
            _targets = new Dictionary<string, TargetBase>();
            _compiledTargets = new Dictionary<string, ICollection<TargetBase>>();
            Dispatcher = dispatcher;
        }

        #endregion Members and Constructors

        #region Target Management

        /// <summary>
        /// Gets a list of concrete target instances based on a list of target names
        /// </summary>
        /// <param name="targets">The target names to use to lookup the concrete target instances</param>
        /// <returns>A list of concrete target instances based on the passed list of target names</returns>
        public ICollection<TargetBase> GetTargets(ICollection<string> targets)
        {
            // Flatten the list of targets for caching/lookup reasons
            string key = FlattenTargets(targets);

            lock (_targetLock)
            {
                // Check the cache to see if we have built this list before, and return it if we have
                if (_compiledTargets.ContainsKey(key))
                {
                    return _compiledTargets[key];
                }
                // We haven't built and cached the list yet, we need to now
                else
                {
                    // Use LINQ to pull the list
                    var compiledTargets = (from targetEntry in _targets
                                           where targets.Contains(targetEntry.Key)
                                           select targetEntry.Value).ToList();

                    // Then archive the results
                    _compiledTargets[key] = compiledTargets;

                    // And return them
                    return compiledTargets;
                }
            }
        }

        // Registers a target to this target keeper in an observer pattern
        private void RegisterTarget(TargetBase target)
        {
            if (target != null)
            {
                lock (_targetLock)
                {
                    lock (_configLock)
                    {
                        _targets[target.Name] = target;
                    }
                }
            }
        }

        // Unregisters a target from this keeper in an observer pattern
        private void UnregisterTarget(TargetBase target)
        {
            if (target != null)
            {
                lock (_targetLock)
                {
                    try
                    {
                        // Try to shutdown the target
                        target.ShutdownInternal();
                    }
                    finally
                    {
                        // Remove it from the list if we have it registered
                        if (_targets.ContainsKey(target.Name))
                        {
                            _targets.Remove(target.Name);
                        }
                    }
                }
            }
        }

        // Unregisters a target from this target keeper by target name
        private void UnregisterTarget(string targetName)
        {
            lock (_targetLock)
            {
                if (_targets.ContainsKey(targetName))
                {
                    UnregisterTarget(_targets[targetName]);
                }
            }
        }

        #endregion Target Management

        #region Configration and Shutdown

        /// <summary>
        /// Notifies this target keeper of a new logging config
        /// </summary>
        /// <param name="loggingConfig">The new logging config</param>
        public void NotifyNewConfig(LoggingConfig loggingConfig)
        {
            lock (_configLock)
            {
                LoggingConfig = loggingConfig;

                lock (_targetLock)
                {
                    Type targetType;
                    ConstructorInfo constructorInfo;
                    TargetBase newTarget;

                    // Iterate over each of the targets to figure out how to handle each one
                    if (loggingConfig.Targets != null && loggingConfig.Targets.Count > 0)
                    {
                        foreach (var targetConfig in loggingConfig.Targets)
                        {
                            // Retrieve the target by name and unregister it
                            //  The unregister function handles a null (not found)
                            //  existing target
                            var oldTarget = (from target in _targets
                                             where target.Key == targetConfig.Name
                                             select target.Value).SingleOrDefault();
                            UnregisterTarget(oldTarget);

                            try
                            {
                                // Build, initialize and registyer a new instance of
                                //  the target based on its represented "Type"
                                targetType = Type.GetType(targetConfig.Type);
                                if (targetType != null)
                                {
                                    constructorInfo = targetType.GetConstructor(new Type[] { });
                                    newTarget = (TargetBase)constructorInfo.Invoke(null);
                                    newTarget.Initialize(targetConfig, Dispatcher, loggingConfig.Synchronous ? (bool?)true : null);

                                    RegisterTarget(newTarget);
                                }
                                else
                                {
                                    throw new LoggingException(String.Format("Type not found for {0}", targetConfig.Type));
                                }
                            }
                            catch (Exception exception)
                            {
                                // If we failed to register the new target,
                                //  re-register the old one and handle the
                                //  exception
                                RegisterTarget(oldTarget);
                                HandleException(exception);
                            }
                        }

                        // Determine which targets are no longer defined
                        //  in the configuration and unregister them
                        var droppedTargets = _targets.Where(_ => !loggingConfig.Targets.Any(c => c.Name == _.Value.Name)).ToList();
                        foreach (var droppedTarget in droppedTargets)
                        {
                            try
                            {
                                UnregisterTarget(droppedTarget.Value);
                            }
                            catch (Exception exception)
                            {
                                HandleException(exception);
                            }
                        }
                    }
                    else
                    {
                        // If no targets were defined, drop the existing registered targets
                        var droppedTargets = _targets.Where(_ => _.Value.Name == "target").ToList();
                        foreach (var droppedTarget in droppedTargets)
                        {
                            try
                            {
                                UnregisterTarget(droppedTarget.Value);
                            }
                            catch (Exception exception)
                            {
                                HandleException(exception);
                            }
                        }

                        // Register a single trace target
                        if (_targets.ContainsKey("target") == false)
                        {
                            newTarget = new TraceTarget("target");
                            RegisterTarget(newTarget);
                        }
                    }

                    // Clear the list of compiled targets
                    _compiledTargets.Clear();
                }
            }
        }

        /// <summary>
        /// Shuts down this target keeper and all targets associated with this target keeper
        /// </summary>
        public void Shutdown()
        {
            lock (_configLock)
            {
                lock (_targetLock)
                {
                    // Iterate over each target and shut it down
                    Trace.WriteLine("Shutting down all targets, to give them an opportunity to flush");
                    foreach (var target in _targets.Values)
                    {
                        try
                        {
                            target.ShutdownInternal();
                        }
                        catch (Exception e)
                        {
                            HandleException(e, "Failed to shutdown (dispose) target", false);
                        }
                    }
                }
            }
        }

        #endregion Configration and Shutdown

        #region Helpers

        // Flattens a list of target names into a single string
        private static string FlattenTargets(ICollection<string> targets)
        {
            return String.Join(",", targets.ToArray());
        }

        // Handles an exception passed to it
        private void HandleException(Exception exception, string message = null, bool bubble = true)
        {
            if (ExceptionHandler != null)
                ExceptionHandler.Invoke(exception, message);
            else if (bubble)
                throw new LoggingException(message == null ? "Error in target keeper" : message, exception);
            else
                Trace.WriteLine(message);
        }

        #endregion Helpers
    }
}