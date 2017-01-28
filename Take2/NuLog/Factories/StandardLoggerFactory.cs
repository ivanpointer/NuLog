/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.Dispatchers;
using NuLog.Dispatchers.TagRouters;
using NuLog.Layouts;
using NuLog.Loggers;
using NuLog.TagRouters;
using NuLog.TagRouters.RuleProcessors;
using NuLog.TagRouters.TagGroupProcessors;
using NuLog.Targets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NuLog.Factories
{
    /// <summary>
    /// The standard implementation of the logger factory.
    /// </summary>
    public class StandardLoggerFactory : ILoggerFactory
    {
        /// <summary>
        /// The type of the layout target base - used to identify which targets we need to assign
        /// layouts to.
        /// </summary>
        private static readonly Type LayoutTargetBaseType = typeof(LayoutTargetBase);

        /// <summary>
        /// The default layout format for the standard layout.
        /// </summary>
        public const string DefaultLayoutFormat = "${DateTime:'{0:MM/dd/yyyy hh:mm:ss.fff}'} | ${Thread.ManagedThreadId:'{0,4}'} | ${Tags} | ${Message}${?Exception:'\r\n{0}'}\r\n";

        /// <summary>
        /// The config for this standard logger factory.
        /// </summary>
        protected readonly Config Config;

        /// <summary>
        /// A lock used for controlling the creation of the dispatcher, normalizer, etc. A highly
        /// efficient version of the singleton pattern.
        /// </summary>
        private static readonly object FactoryLock = new object();

        /// <summary>
        /// The private instance of the dispatcher.
        /// </summary>
        private IDispatcher _dispatcher;

        /// <summary>
        /// The dispatcher for this factory - a thread safe implementation which calls GetDispatcher.
        /// </summary>
        protected IDispatcher Dispatcher
        {
            get
            {
                if (_dispatcher == null)
                {
                    lock (FactoryLock)
                    {
                        if (isDisposing)
                        {
                            throw new InvalidOperationException("Cannot instantiate dispatcher after factory is disposed.");
                        }
                        else if (_dispatcher == null)
                        {
                            _dispatcher = GetDispatcher();
                        }
                    }
                }
                return _dispatcher;
            }
        }

        /// <summary>
        /// The private instance of the tag normalizer.
        /// </summary>
        private ITagNormalizer _tagNormalizer;

        /// <summary>
        /// The tag normalizer for this factory - a thread safe implementation which calls GetTagNormalizer.
        /// </summary>
        protected ITagNormalizer TagNormalizer
        {
            get
            {
                if (_tagNormalizer == null)
                {
                    lock (FactoryLock)
                    {
                        if (_tagNormalizer == null)
                        {
                            _tagNormalizer = GetTagNormalizer();
                        }
                    }
                }
                return _tagNormalizer;
            }
        }

        /// <summary>
        /// The default meta data for this factory.
        /// </summary>
        private IDictionary<string, object> _defaultMetaData;

        /// <summary>
        /// The default meta data for this factory - a thread safe implementation which calls ToMetaData.
        /// </summary>
        protected IDictionary<string, object> DefaultMetaData
        {
            get
            {
                if (_defaultMetaData == null)
                {
                    lock (FactoryLock)
                    {
                        if (_defaultMetaData == null)
                        {
                            _defaultMetaData = new ReadOnlyDictionary<string, object>(ToMetaData(Config.MetaData));
                        }
                    }
                }
                return _defaultMetaData;
            }
        }

        /// <summary>
        /// Used to prevent new log events from being enqueued after disposal has been initiated.
        /// </summary>
        private bool isDisposing;

        public StandardLoggerFactory(Config config)
        {
            Config = config;
        }

        public ILogger GetLogger(IMetaDataProvider metaDataProvider, IEnumerable<string> defaultTags)
        {
            return new StandardLogger(Dispatcher, TagNormalizer, metaDataProvider, defaultTags, DefaultMetaData, Config.IncludeStackFrame);
        }

        public virtual IDispatcher GetDispatcher()
        {
            var targets = GetTargets();
            var tagRouter = GetTagRouter();
            return new StandardDispatcher(targets, tagRouter, null);
        }

        public virtual ICollection<ITarget> GetTargets()
        {
            var targets = new List<ITarget>();

            // Should be lenient of null targets
            if (Config.Targets == null)
            {
                return targets;
            }

            foreach (var targetConfig in Config.Targets)
            {
                targets.Add(BuildTarget(targetConfig));
            }

            return targets;
        }

        public virtual ITagGroupProcessor GetTagGroupProcessor()
        {
            return new StandardTagGroupProcessor(ToTagGroups(Config.TagGroups));
        }

        public virtual IRuleProcessor GetRuleProcessor()
        {
            var tagGroupProcessor = GetTagGroupProcessor();
            return new StandardRuleProcessor(ToRules(Config.Rules), tagGroupProcessor);
        }

        public virtual ITagRouter GetTagRouter()
        {
            var ruleProcessor = GetRuleProcessor();
            return new StandardTagRouter(ruleProcessor);
        }

        public virtual ITagNormalizer GetTagNormalizer()
        {
            return new StandardTagNormalizer();
        }

        public virtual ILayoutParser GetLayoutParser()
        {
            return new StandardLayoutParser();
        }

        public virtual IPropertyParser GetPropertyParser()
        {
            return new StandardPropertyParser();
        }

        public virtual ILayout GetLayout(TargetConfig config)
        {
            // Get the layout parameters, or use the default format if we don't find it
            var layoutParser = GetLayoutParser();
            var format = config.Properties != null && config.Properties.ContainsKey("layout")
                ? (string)config.Properties["layout"]
                : DefaultLayoutFormat;
            var layoutParms = layoutParser.Parse(format);

            // Get the property parser
            var propertyParser = GetPropertyParser();

            // Stitch it together into a new standard layout
            return new StandardLayout(layoutParms, propertyParser);
        }

        #region Config Conversions

        /// <summary>
        /// Translate the given tag group configs, into tag groups.
        /// </summary>
        protected virtual IEnumerable<TagGroup> ToTagGroups(IEnumerable<TagGroupConfig> tagGroupConfigs)
        {
            var tagGroups = new List<TagGroup>();

            // Should be tolerant of a null tag group
            if (tagGroupConfigs == null)
            {
                return tagGroups;
            }

            foreach (var config in tagGroupConfigs)
            {
                tagGroups.Add(ToTagGroup(config));
            }

            return tagGroups;
        }

        /// <summary>
        /// Translate the given tag group config, into a tag group.
        /// </summary>
        protected virtual TagGroup ToTagGroup(TagGroupConfig config)
        {
            return new TagGroup
            {
                BaseTag = config.BaseTag,
                Aliases = config.Aliases
            };
        }

        /// <summary>
        /// Translate the given list of rule configs, into rules.
        /// </summary>
        protected virtual IEnumerable<Rule> ToRules(IEnumerable<RuleConfig> ruleConfigs)
        {
            var rules = new List<Rule>();

            // We should be tolerant of a null list of rules
            if (ruleConfigs == null)
            {
                return rules;
            }

            foreach (var config in ruleConfigs)
            {
                rules.Add(ToRule(config));
            }

            return rules;
        }

        /// <summary>
        /// Translates the given rule config, into a rule.
        /// </summary>
        protected virtual Rule ToRule(RuleConfig config)
        {
            return new Rule
            {
                Include = config.Includes,
                StrictInclude = config.StrictInclude,
                Exclude = config.Excludes,
                Targets = config.Targets,
                Final = config.Final
            };
        }

        /// <summary>
        /// Translates meta data from config, into the kind of meta data the logger expects.
        /// </summary>
        protected virtual IDictionary<string, object> ToMetaData(IDictionary<string, string> configMetaData)
        {
            var metaData = new Dictionary<string, object>();

            // Should be tolerant of null config meta data.
            if (configMetaData == null)
            {
                return metaData;
            }

            foreach (var entry in configMetaData)
            {
                metaData[entry.Key] = entry.Value;
            }

            return metaData;
        }

        #endregion Config Conversions

        #region Disposal

        public void Dispose()
        {
            // Signal a true disposal
            Dispose(true);

            // Tell the GC that we've got it
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The bulk of the clean-up code is implemented in Dispose(bool)
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Signal that we're coming down
                isDisposing = true;
            }

            if (_dispatcher != null)
            {
                _dispatcher.Dispose();
                _dispatcher = null;
            }
        }

        ~StandardLoggerFactory()
        {
            Dispose(false);
        }

        #endregion Disposal

        #region Internals

        protected virtual ITarget BuildTarget(TargetConfig targetConfig)
        {
            // Use the activator to build out a new target instance
            var type = Type.GetType(targetConfig.Type);
            var target = (ITarget)Activator.CreateInstance(type);

            // Configure and set the target's name
            target.Configure(targetConfig);
            target.Name = targetConfig.Name;

            // Check to see if the target is a layout target, and set its layout if so
            if (LayoutTargetBaseType.IsAssignableFrom(target.GetType()))
            {
                var layout = GetLayout(targetConfig);
                var layoutTarget = (LayoutTargetBase)target;
                layoutTarget.SetLayout(layout);
            }

            // Return the built target
            return target;
        }

        #endregion Internals
    }
}