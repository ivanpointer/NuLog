using NuLog.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Dispatch
{
    public class RuleKeeper : IConfigObserver
    {
        private static readonly object _ruleLock = new object();
        private ICollection<RuleConfig> Rules { get; set; }

        private const int _routeCacheLimit = 100;
        private IDictionary<string, ICollection<string>> _routeCache;

        private TagKeeper TagKeeper { get; set; }

        public RuleKeeper(TagKeeper tagKeeper)
        {
            TagKeeper = tagKeeper;
            _routeCache = new Dictionary<string, ICollection<string>>();
        }

        public ICollection<string> GetTargetsForTags(ICollection<string> tags, bool routeCache = true)
        {
            ICollection<string> targets = new List<string>();

            lock (_ruleLock)
            {
                var route = routeCache ? FlattenTags(tags) : null;
                if (routeCache && _routeCache.ContainsKey(route))
                {
                    targets = _routeCache[route];
                }
                else
                {
                    bool doInclude;
                    foreach (var ruleConfig in Rules)
                    {
                        doInclude = ruleConfig.Include != null && ruleConfig.Include.Count > 0
                            ? TagKeeper.CheckMatch(tags, ruleConfig.Include, ruleConfig.StrictInclude)
                            : true;

                        doInclude = doInclude
                            && (ruleConfig.Exclude == null
                                || ruleConfig.Exclude.Count == 0
                                || !TagKeeper.CheckMatch(tags, ruleConfig.Exclude));

                        if (doInclude)
                        {
                            foreach (var target in ruleConfig.WriteTo)
                                if (targets.Contains(target) == false)
                                    targets.Add(target);

                            if (ruleConfig.Final)
                                break;
                        }
                    }

                    if (routeCache)
                    {
                        if (_routeCache.Count > _routeCacheLimit)
                            _routeCache.Remove(_routeCache.Keys.First());

                        _routeCache[route] = targets;
                    }

                }
            }

            return targets;
        }

        public void NotifyNewConfig(LoggingConfig loggingConfig)
        {
            lock (_ruleLock)
            {
                if (loggingConfig.Rules != null && loggingConfig.Rules.Count > 0)
                {
                    Rules = loggingConfig.Rules;
                }
                else
                {
                    var newRule = RuleConfigBuilder.CreateRuleConfig()
                        .AddWriteTo("*")
                        .Build();
                    Rules = new List<RuleConfig> { newRule };
                }

                _routeCache.Clear();
            }
        }

        #region Helpers

        private static string FlattenTags(ICollection<string> tags)
        {
            return String.Join(",", tags.OrderBy(_ => _).ToArray());
        }

        #endregion

    }
}
