using NuLog.Configuration;
using NuLog.Targets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Dispatch
{
    public class TargetKeeper : IConfigObserver
    {
        private static readonly object _configLock = new object();
        private LoggingConfig LoggingConfig { get; set; }

        private static readonly object _targetLock = new object();
        private IDictionary<string, TargetBase> _targets;
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

        private IDictionary<string, ICollection<TargetBase>> _compiledTargets;

        public Action<Exception, string> ExceptionHandler { get; set; }

        protected LogEventDispatcher Dispatcher { get; set; }

        public TargetBase RootTarget { get; private set; }

        public TargetKeeper()
        {
            _targets = new Dictionary<string, TargetBase>();
            _compiledTargets = new Dictionary<string, ICollection<TargetBase>>();
        }

        public ICollection<TargetBase> GetTargets(ICollection<string> targets)
        {
            string key = FlattenTargets(targets);

            lock (_targetLock)
            {
                if (_compiledTargets.ContainsKey(key))
                {
                    return _compiledTargets[key];
                }
                else
                {
                    var compiledTargets = (from targetEntry in _targets
                                           where targets.Contains(targetEntry.Key)
                                           select targetEntry.Value).ToList();
                    _compiledTargets[key] = compiledTargets;

                    return compiledTargets;
                }
            }
        }

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

        private void UnregisterTarget(TargetBase target)
        {
            if (target != null)
            {
                lock (_targetLock)
                {
                    try
                    {
                        target.Shutdown();
                    }
                    finally
                    {
                        if (_targets.ContainsKey(target.Name))
                        {
                            _targets.Remove(target.Name);
                        }
                    }
                }
            }
        }

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

                    if (loggingConfig.Targets != null && loggingConfig.Targets.Count > 0)
                    {
                        foreach (var targetConfig in loggingConfig.Targets)
                        {
                            var oldTarget = (from target in _targets
                                             where target.Key == targetConfig.Name
                                             select target.Value).SingleOrDefault();
                            UnregisterTarget(oldTarget);

                            try
                            {
                                targetType = Type.GetType(targetConfig.Type);
                                if (targetType != null)
                                {
                                    constructorInfo = targetType.GetConstructor(new Type[] { });
                                    newTarget = (TargetBase)constructorInfo.Invoke(null);
                                    newTarget.Initialize(targetConfig);

                                    RegisterTarget(newTarget);
                                }
                                else
                                {
                                    throw new LoggingException(String.Format("Type not found for {0}", targetConfig.Type));
                                }
                            }
                            catch (Exception exception)
                            {
                                RegisterTarget(oldTarget);
                                HandleException(exception);
                            }
                        }

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

                        if (_targets.ContainsKey("target") == false)
                        {
                            newTarget = new TraceTarget("target");
                            RegisterTarget(newTarget);
                        }
                    }

                    _compiledTargets.Clear();
                }
            }
        }

        public void Shutdown()
        {
            lock (_configLock)
            {
                lock (_targetLock)
                {
                    foreach (var target in _targets.Values)
                    {
                        try
                        {
                            target.Shutdown();
                        }
                        catch (Exception e)
                        {
                            HandleException(e, "Failed to shutdown (dispose) target", false);
                        }
                    }
                }
            }
        }

        #region Helpers

        private static string FlattenTargets(ICollection<string> targets)
        {
            return String.Join(",", targets.ToArray());
        }

        private void HandleException(Exception exception, string message = null, bool bubble = true)
        {
            if (ExceptionHandler != null)
                ExceptionHandler.Invoke(exception, message);
            else if (bubble)
                throw new LoggingException(message == null ? "Error in target keeper" : message, exception);
            else
                Trace.WriteLine(message);
        }

        #endregion
    }
}
