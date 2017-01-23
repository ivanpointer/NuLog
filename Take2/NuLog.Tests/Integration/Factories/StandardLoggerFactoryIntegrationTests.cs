/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.Configuration;
using NuLog.LogEvents;
using NuLog.TagRouters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Xunit;

namespace NuLog.Tests.Integration.Factories
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the standard logger factory, but will cross
    /// boundaries into other classes, making these integration tests, and not unit tests.
    /// </summary>
    [Trait("Category", "Integration")]
    public class StandardLoggerFactoryIntegrationTests : StandardLoggerFactoryTestsBase, IDisposable
    {
        private HashSetTraceListener traceListener;

        public StandardLoggerFactoryIntegrationTests()
        {
            this.traceListener = new HashSetTraceListener();
            Debug.Listeners.Add(this.traceListener);
        }

        public void Dispose()
        {
            Debug.Listeners.Remove(this.traceListener);
            this.traceListener = null;
        }

        /// <summary>
        /// The standard logger factory should leverage the given tag group configs when building the
        /// tag group processor.
        /// </summary>
        [Fact(DisplayName = "Should_UseTagGroupConfigs")]
        public void Should_UseTagGroupConfigs()
        {
            // Setup
            var tagGroupConfig = new TagGroupConfig
            {
                BaseTag = "base_tag",
                Aliases = new string[] { "one_tag", "two_tag", "red_tag", "blue_tag" }
            };
            var tagGroupConfigs = new List<TagGroupConfig> { tagGroupConfig };
            var config = new Config
            {
                TagGroups = tagGroupConfigs
            };
            var factory = GetLogFactory(config);

            // Execute
            var tagGroupProcessor = factory.GetTagGroupProcessor();

            // Validate
            var tags = tagGroupProcessor.GetAliases("red_tag");
            Assert.Contains("base_tag", tags);
        }

        /// <summary>
        /// The standard logger factory should create a rule processor, and hand it the given rule
        /// configs, and tag group processor.
        /// </summary>
        [Fact(DisplayName = "Should_UseRuleConfigsAndTagGroupProcessor")]
        public void Should_UseRuleConfigsAndTagGroupProcessor()
        {
            // Setup
            var tagGroupProcessor = A.Fake<ITagGroupProcessor>();
            A.CallTo(() => tagGroupProcessor.GetAliases(A<string>.Ignored))
                .Returns(new string[] { "base_tag", "red_tag" });
            var ruleConfig = new RuleConfig
            {
                Includes = new List<string> { "base_tag" },
                Targets = new List<string> { "fake_target" }
            };
            var tagGroupConfig = new TagGroupConfig
            {
                BaseTag = "base_tag",
                Aliases = new string[] { "red_tag" }
            };
            var config = new Config
            {
                Rules = new List<RuleConfig> { ruleConfig },
                TagGroups = new List<TagGroupConfig> { tagGroupConfig }
            };
            var factory = GetLogFactory(config);
            var ruleProcessor = factory.GetRuleProcessor();

            // Execute
            var targets = ruleProcessor.DetermineTargets(new string[] { "red_tag" });

            // Verify
            Assert.Equal(1, targets.Count());
            Assert.Contains("fake_target", targets);
        }

        /// <summary>
        /// The standard logger factory should create a tag router, and hand it the given rule processor.
        /// </summary>
        [Fact(DisplayName = "Should_UseRuleProcessor")]
        public void Should_UseRuleProcessor()
        {
            // Setup
            var ruleConfig = new RuleConfig
            {
                Includes = new string[] { "one_tag" },
                Targets = new string[] { "fake_target" }
            };
            var config = new Config { Rules = new List<RuleConfig> { ruleConfig } };
            var factory = GetLogFactory(config);
            var tagRouter = factory.GetTagRouter();

            // Execute
            var targets = tagRouter.Route(new string[] { "one_tag" });

            // Verify
            Assert.Equal(1, targets.Count());
            Assert.Contains("fake_target", targets);
        }

        /// <summary>
        /// The standard logger factory should use the given targets, and build and use the tag router.
        /// </summary>
        [Fact(DisplayName = "Should_UseTargetsAndBuildTagRouter")]
        public void Should_UseTargetsAndBuildTagRouter()
        {
            // Setup
            var targetConfig = new TargetConfig
            {
                Name = "debug",
                Type = "NuLog.Targets.DebugTarget",
                Properties = new Dictionary<string, object>
                {
                    { "layout", "${Message}" }
                }
            };
            var ruleConfig = new RuleConfig
            {
                Includes = new string[] { "one_tag" },
                Targets = new string[] { "debug" }
            };
            var config = new Config
            {
                Targets = new List<TargetConfig> { targetConfig },
                Rules = new List<RuleConfig> { ruleConfig }
            };
            var factory = GetLogFactory(config);
            var dispatcher = factory.GetDispatcher();

            // Execute
            dispatcher.DispatchNow(new LogEvent
            {
                Message = "test dispatch targets and router",
                Tags = new string[] { "one_tag" }
            });

            // Verify
            Assert.Contains("test dispatch targets and router", this.traceListener.Messages);
        }

        /// <summary>
        /// The layout built by the factory should leverage the layout properties and property parser.
        /// </summary>
        [Fact(DisplayName = "Should_UseTargetsAndBuildTagRouter")]
        public void Should_UseLayoutParmsAndPropertyParser()
        {
            // Setup
            var targetConfig = new TargetConfig
            {
                Properties = new Dictionary<string, object>
                {
                    { "layout", "${Message}" }
                }
            };
            var factory = GetLogFactory(null);
            var layout = factory.GetLayout(targetConfig);

            // Execute
            var formatted = layout.Format(new LogEvent
            {
                Message = "hello, world!"
            });

            // Validate
            Assert.Equal("hello, world!", formatted);
        }

        /// <summary>
        /// The factory should set a default layout format, when none is provided by the target config.
        /// </summary>
        [Fact(DisplayName = "Should_UseDefaultLayoutFormat")]
        public void Should_UseDefaultLayoutFormat()
        {
            // Setup
            var targetConfig = new TargetConfig
            {
                // notably vacant of default layouts...
            };
            var factory = GetLogFactory(null);
            var layout = factory.GetLayout(targetConfig);

            // Execute
            var formatted = layout.Format(new LogEvent
            {
                Message = "hello, default layout!",
                Tags = new string[] { "dummy_event" },
                Thread = Thread.CurrentThread,
                DateLogged = DateTime.Now
            });

            // Validate
            Assert.Contains("hello, default layout!", formatted);
        }

        /// <summary>
        /// The factory should assign a dispatcher to the built logger.
        /// </summary>
        [Fact(DisplayName = "Should_SetDispatcherToNewLogger")]
        public void Should_SetDispatcherToNewLogger()
        {
            // Setup
            var config = new Config
            {
                Targets = new List<TargetConfig>
                {
                    new TargetConfig
                    {
                        Name = "debug",
                        Type = "NuLog.Targets.DebugTarget",
                        Properties = new Dictionary<string, object>
                        {
                            { "layout", "${Message}" }
                        }
                    }
                },
                Rules = new List<RuleConfig>
                {
                    new RuleConfig
                    {
                        Includes = new string[] { "one_tag" },
                        Targets = new string[] { "debug" }
                    }
                }
            };
            var factory = GetLogFactory(config);
            var logger = factory.GetLogger(null, null);

            // Execute
            logger.LogNow("hello, logger factory dispatcher set!", "one_tag");

            // Verify
            Assert.Contains("hello, logger factory dispatcher set!", this.traceListener.Messages);
        }

        /// <summary>
        /// The factory should assign the given meta data provider, to the new logger.
        /// </summary>
        [Fact(DisplayName = "Should_SetMetaDataProviderToNewLogger")]
        public void Should_SetMetaDataProviderToNewLogger()
        {
            // Setup
            var config = new Config
            {
                Targets = new List<TargetConfig>
                {
                    new TargetConfig
                    {
                        Name = "debug",
                        Type = "NuLog.Targets.DebugTarget",
                        Properties = new Dictionary<string, object>
                        {
                            { "layout", "${MyMetaData}" }
                        }
                    }
                },
                Rules = new List<RuleConfig>
                {
                    new RuleConfig
                    {
                        Includes = new string[] { "one_tag" },
                        Targets = new string[] { "debug" }
                    }
                }
            };
            var factory = GetLogFactory(config);
            var provider = A.Fake<IMetaDataProvider>();
            A.CallTo(() => provider.ProvideMetaData())
                .Returns(new Dictionary<string, object> { { "MyMetaData", "Meta data to my logger!" } });

            var logger = factory.GetLogger(provider, null);

            // Execute
            logger.LogNow("nope", "one_tag");

            // Verify
            Assert.Contains("Meta data to my logger!", this.traceListener.Messages);
        }

        /// <summary>
        /// The factory should set the given default tags to the new logger.
        /// </summary>
        [Fact(DisplayName = "Should_SetDefaultTagsToNewLogger")]
        public void Should_SetDefaultTagsToNewLogger()
        {
            // Setup
            var config = new Config
            {
                Targets = new List<TargetConfig>
                {
                    new TargetConfig
                    {
                        Name = "debug",
                        Type = "NuLog.Targets.DebugTarget",
                        Properties = new Dictionary<string, object>
                        {
                            { "layout", "${Tags}" }
                        }
                    }
                },
                Rules = new List<RuleConfig>
                {
                    new RuleConfig
                    {
                        Includes = new string[] { "one_tag" },
                        Targets = new string[] { "debug" }
                    }
                }
            };
            var factory = GetLogFactory(config);
            var logger = factory.GetLogger(null, new string[] { "default_tag" });

            // Execute
            logger.LogNow("nope", "one_tag");

            // Verify
            Assert.Contains("default_tag,one_tag", this.traceListener.Messages);
        }

        /// <summary>
        /// The factory should set default meta data to the new logger.
        /// </summary>
        [Fact(DisplayName = "Should_SetDefaultMetaDataToNewLogger")]
        public void Should_SetDefaultMetaDataToNewLogger()
        {
            // Setup
            var config = new Config
            {
                Targets = new List<TargetConfig>
                {
                    new TargetConfig
                    {
                        Name = "debug",
                        Type = "NuLog.Targets.DebugTarget",
                        Properties = new Dictionary<string, object>
                        {
                            { "layout", "${MyMetaData}" }
                        }
                    }
                },
                Rules = new List<RuleConfig>
                {
                    new RuleConfig
                    {
                        Includes = new string[] { "one_tag" },
                        Targets = new string[] { "debug" }
                    }
                },
                MetaData = new Dictionary<string, string>
                {
                    { "MyMetaData", "Default meta data to my logger!" }
                }
            };
            var factory = GetLogFactory(config);
            var logger = factory.GetLogger(null, null);

            // Execute
            logger.LogNow("nope", "one_tag");

            // Verify
            Assert.Contains("Default meta data to my logger!", this.traceListener.Messages);
        }
    }
}