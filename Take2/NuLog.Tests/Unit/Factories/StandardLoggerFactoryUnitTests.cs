/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.Targets;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NuLog.Tests.Unit.Factories
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the standard logger factory.
    /// </summary>
    [Trait("Category", "Unit")]
    public class StandardLoggerFactoryUnitTests : StandardLoggerFactoryTestsBase
    {
        /// <summary>
        /// The standard logger factory should create a new target instance.
        /// </summary>
        [Fact(DisplayName = "Should_CreateNewTargetInstance")]
        public void Should_CreateNewTargetInstance()
        {
            // Setup
            var targetConfig = new TargetConfig
            {
                Name = "dummy",
                Type = "NuLog.Tests.DummyTarget, NuLog.Tests"
            };
            var configs = new List<TargetConfig> { targetConfig };
            var config = new Config
            {
                Targets = configs
            };

            var factory = GetLogFactory(config);

            // Execute
            var targets = factory.GetTargets();

            // Validate
            var target = targets.Single();
            Assert.True(typeof(DummyTarget).IsAssignableFrom(target.GetType()));
        }

        /// <summary>
        /// The factory should be lenient of a null set of target configs
        /// </summary>
        [Fact(DisplayName = "Should_SupportNullTargetConfig")]
        public void Should_SupportNullTargetConfig()
        {
            // Setup
            var config = new Config
            {
                Targets = null // Explicitly null
            };

            var factory = GetLogFactory(config);

            // Execute
            var targets = factory.GetTargets();

            // Validate
            Assert.Equal(0, targets.Count);
        }

        /// <summary>
        /// When creating a new target, the factory should set the target's name.
        /// </summary>
        [Fact(DisplayName = "Should_SetTargetNameOnCreate")]
        public void Should_SetTargetNameOnCreate()
        {
            // Setup
            var targetConfig = new TargetConfig
            {
                Name = "dummy",
                Type = "NuLog.Tests.DummyTarget, NuLog.Tests"
            };
            var configs = new List<TargetConfig> { targetConfig };
            var config = new Config
            {
                Targets = configs
            };
            var factory = GetLogFactory(config);

            // Execute
            var targets = factory.GetTargets();

            // Validate
            var target = targets.Single();
            Assert.Equal("dummy", target.Name);
        }

        /// <summary>
        /// The factory should call "configure" on the target when it is created.
        /// </summary>
        [Fact(DisplayName = "Should_CallConfigure")]
        public void Should_CallConfigure()
        {
            // Setup
            var targetConfig = new TargetConfig
            {
                Name = "dummy",
                Type = "NuLog.Tests.DummyTarget, NuLog.Tests"
            };
            var configs = new List<TargetConfig> { targetConfig };
            var config = new Config
            {
                Targets = configs
            };

            var factory = GetLogFactory(config);

            // Execute
            var targets = factory.GetTargets();

            // Validate
            var target = targets.Single();
            Assert.Equal(1, ((DummyTarget)target).ConfigureCallCount);
        }

        /// <summary>
        /// The factory should pass the target's configuration to the call to config on the target.
        /// </summary>
        [Fact(DisplayName = "Should_PassConfigToConfigCall")]
        public void Should_PassConfigToConfigCall()
        {
            // Setup
            var targetConfig = new TargetConfig
            {
                Name = "dummy",
                Type = "NuLog.Tests.DummyTarget, NuLog.Tests"
            };
            var configs = new List<TargetConfig> { targetConfig };
            var config = new Config
            {
                Targets = configs
            };

            var factory = GetLogFactory(config);

            // Execute
            var targets = factory.GetTargets();

            // Validate
            var target = (DummyTarget)targets.Single();
            var passedConfig = target.ConfigsPassed.Single();
            Assert.Equal(targetConfig, passedConfig);
        }

        /// <summary>
        /// The factory should set the layout on a new layout target.
        /// </summary>
        [Fact(DisplayName = "Should_SetLayout")]
        public void Should_SetLayout()
        {
            // Setup
            var targetConfig = new TargetConfig
            {
                Name = "dummy",
                Type = "NuLog.Tests.DummyLayoutTarget, NuLog.Tests",
                Properties = new Dictionary<string, object>
                {
                    { "layout", "${Message}" }
                }
            };
            var configs = new List<TargetConfig> { targetConfig };
            var config = new Config
            {
                Targets = configs
            };

            var factory = GetLogFactory(config);

            // Execute
            var targets = factory.GetTargets();

            // Validate
            var layoutTarget = (DummyLayoutTarget)targets.Single();
            Assert.NotNull(layoutTarget.GetLayout());
        }

        /// <summary>
        /// The standard logger factory should create multiple new target instances.
        /// </summary>
        [Fact(DisplayName = "Should_CreateMultipleTargets")]
        public void Should_CreateMultipleTargets()
        {
            // Setup
            var configs = new List<TargetConfig> { new TargetConfig
                {
                    Name = "dummy",
                    Type = "NuLog.Tests.DummyTarget, NuLog.Tests"
                },new TargetConfig
                {
                    Name = "dummy",
                    Type = "NuLog.Targets.DebugTarget, NuLog"
                }
            };
            var config = new Config
            {
                Targets = configs
            };
            var factory = GetLogFactory(config);

            // Execute
            var targets = factory.GetTargets();

            // Validate
            Assert.Equal(2, targets.Count);
            Assert.True(targets.Any(t => typeof(DummyTarget).IsAssignableFrom(t.GetType())));
            Assert.True(targets.Any(t => typeof(DebugTarget).IsAssignableFrom(t.GetType())));
        }

        /// <summary>
        /// The factory should create a new dispatcher.
        /// </summary>
        [Fact(DisplayName = "Should_BuildDispatcher")]
        public void Should_BuildDispatcher()
        {
            // Setup
            var factory = GetLogFactory(new Config());

            // Execute
            var dispatcher = factory.GetDispatcher();

            // Verify
            Assert.NotNull(dispatcher);
        }

        /// <summary>
        /// The factory should create a new tag normalizer.
        /// </summary>
        [Fact(DisplayName = "Should_BuildTagNormalizer")]
        public void Should_BuildTagNormalizer()
        {
            // Setup
            var factory = GetLogFactory(new Config());

            // Execute
            var normalizer = factory.GetTagNormalizer();

            // Verify
            Assert.NotNull(normalizer);
        }

        /// <summary>
        /// The standard logger factory should build a tag group processor.
        /// </summary>
        [Fact(DisplayName = "Should_BuildTagGroupProcessor")]
        public void Should_BuildTagGroupProcessor()
        {
            // Setup
            var factory = GetLogFactory(new Config());

            // Execute
            var tagGroupProcessor = factory.GetTagGroupProcessor();

            // Verify
            Assert.NotNull(tagGroupProcessor);
        }

        /// <summary>
        /// The standard logger factory should build a rule processor.
        /// </summary>
        [Fact(DisplayName = "Should_BuildRuleProcessor")]
        public void Should_BuildRuleProcessor()
        {
            // Setup
            var factory = GetLogFactory(new Config
            {
                Rules = new List<RuleConfig>
                {
                    new RuleConfig
                    {
                        Includes = new string[] {"one_tag"},
                        Targets = new string[] {"fake_target"}
                    }
                }
            });

            // Execute
            var ruleProcessor = factory.GetRuleProcessor();

            // Verify
            Assert.NotNull(ruleProcessor);
        }

        /// <summary>
        /// The standard logger factory should build a tag router.
        /// </summary>
        [Fact(DisplayName = "Should_BuildTagRouter")]
        public void Should_BuildTagRouter()
        {
            // Setup
            var factory = GetLogFactory(new Config());

            // Execute
            var tagRouter = factory.GetTagRouter();

            // Verify
            Assert.NotNull(tagRouter);
        }

        /// <summary>
        /// The standard logger factory should build a layout parser.
        /// </summary>
        [Fact(DisplayName = "Should_BuildLayoutParser")]
        public void Should_BuildLayoutParser()
        {
            // Setup
            var factory = GetLogFactory(null);

            // Execute
            var layoutParser = factory.GetLayoutParser();

            // Verify
            Assert.NotNull(layoutParser);
        }

        /// <summary>
        /// The standard logger factory should build a property parser.
        /// </summary>
        [Fact(DisplayName = "Should_BuildPropertyParser")]
        public void Should_BuildPropertyParser()
        {
            // Setup
            var factory = GetLogFactory(null);

            // Execute
            var propertyParser = factory.GetPropertyParser();

            // Verify
            Assert.NotNull(propertyParser);
        }

        /// <summary>
        /// The standard logger factory should build a layout.
        /// </summary>
        [Fact(DisplayName = "Should_BuildLayout")]
        public void Should_BuildLayout()
        {
            // Setup
            var factory = GetLogFactory(null);

            // Execute
            var layout = factory.GetLayout(new TargetConfig
            {
                Properties = new Dictionary<string, object>
                {
                    { "layout", "${Message}" }
                }
            });

            // Verify
            Assert.NotNull(layout);
        }
    }
}