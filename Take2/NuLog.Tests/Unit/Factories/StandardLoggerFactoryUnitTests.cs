/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.Dispatchers;
using NuLog.Factories;
using NuLog.FallbackLoggers;
using NuLog.LogEvents;
using NuLog.Targets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace NuLog.Tests.Unit.Factories {

    /// <summary>
    /// Documents (and verifies) the expected behavior of the standard logger factory.
    /// </summary>
    [Trait("Category", "Unit")]
    public class StandardLoggerFactoryUnitTests : StandardLoggerFactoryTestsBase {
        private HashSetTraceListener traceListener;

        /// <summary>
        /// Constructor hooks in the xUnit test output helper, which in a base class, is used in
        /// conjunction with a trace listener, to route trace messages to output.
        /// </summary>
        public StandardLoggerFactoryUnitTests(ITestOutputHelper output) : base(output) {
            this.traceListener = new HashSetTraceListener();
            Trace.Listeners.Add(this.traceListener);
        }

        public override void Dispose() {
            Trace.Listeners.Remove(this.traceListener);
            this.traceListener = null;

            base.Dispose();
        }

        /// <summary>
        /// The standard logger factory should create a new target instance.
        /// </summary>
        [Fact(DisplayName = "Should_CreateNewTargetInstance")]
        public void Should_CreateNewTargetInstance() {
            // Setup
            var targetConfig = new TargetConfig {
                Name = "dummy",
                Type = "NuLog.Tests.DummyTarget, NuLog.Tests"
            };
            var configs = new List<TargetConfig> { targetConfig };
            var config = new Config {
                Targets = configs
            };

            var factory = GetLogFactory(config);

            // Execute
            var targets = factory.MakeTargets();

            // Validate
            var target = targets.Single();
            Assert.True(typeof(DummyTarget).IsAssignableFrom(target.GetType()));
        }

        /// <summary>
        /// The factory should be lenient of a null set of target configs
        /// </summary>
        [Fact(DisplayName = "Should_SupportNullTargetConfig")]
        public void Should_SupportNullTargetConfig() {
            // Setup
            var config = new Config {
                Targets = null // Explicitly null
            };

            var factory = GetLogFactory(config);

            // Execute
            var targets = factory.MakeTargets();

            // Validate
            Assert.Equal(0, targets.Count);
        }

        /// <summary>
        /// When creating a new target, the factory should set the target's name.
        /// </summary>
        [Fact(DisplayName = "Should_SetTargetNameOnCreate")]
        public void Should_SetTargetNameOnCreate() {
            // Setup
            var targetConfig = new TargetConfig {
                Name = "dummy",
                Type = "NuLog.Tests.DummyTarget, NuLog.Tests"
            };
            var configs = new List<TargetConfig> { targetConfig };
            var config = new Config {
                Targets = configs
            };
            var factory = GetLogFactory(config);

            // Execute
            var targets = factory.MakeTargets();

            // Validate
            var target = targets.Single();
            Assert.Equal("dummy", target.Name);
        }

        /// <summary>
        /// The factory should call "configure" on the target when it is created.
        /// </summary>
        [Fact(DisplayName = "Should_CallConfigure")]
        public void Should_CallConfigure() {
            // Setup
            var targetConfig = new TargetConfig {
                Name = "dummy",
                Type = "NuLog.Tests.DummyTarget, NuLog.Tests"
            };
            var configs = new List<TargetConfig> { targetConfig };
            var config = new Config {
                Targets = configs
            };

            var factory = GetLogFactory(config);

            // Execute
            var targets = factory.MakeTargets();

            // Validate
            var target = targets.Single();
            Assert.Equal(1, ((DummyTarget)target).ConfigureCallCount);
        }

        /// <summary>
        /// The factory should pass the target's configuration to the call to config on the target.
        /// </summary>
        [Fact(DisplayName = "Should_PassConfigToConfigCall")]
        public void Should_PassConfigToConfigCall() {
            // Setup
            var targetConfig = new TargetConfig {
                Name = "dummy",
                Type = "NuLog.Tests.DummyTarget, NuLog.Tests"
            };
            var configs = new List<TargetConfig> { targetConfig };
            var config = new Config {
                Targets = configs
            };

            var factory = GetLogFactory(config);

            // Execute
            var targets = factory.MakeTargets();

            // Validate
            var target = (DummyTarget)targets.Single();
            var passedConfig = target.ConfigsPassed.Single();
            Assert.Equal(targetConfig, passedConfig);
        }

        /// <summary>
        /// The factory should set the layout on a new layout target.
        /// </summary>
        [Fact(DisplayName = "Should_SetLayout")]
        public void Should_SetLayout() {
            // Setup
            var targetConfig = new TargetConfig {
                Name = "dummy",
                Type = "NuLog.Tests.DummyLayoutTarget, NuLog.Tests",
                Properties = new Dictionary<string, object>
                {
                    { "layout", "${Message}" }
                }
            };
            var configs = new List<TargetConfig> { targetConfig };
            var config = new Config {
                Targets = configs
            };

            var factory = GetLogFactory(config);

            // Execute
            var targets = factory.MakeTargets();

            // Validate
            var layoutTarget = (DummyLayoutTarget)targets.Single();
            Assert.NotNull(layoutTarget.GetLayout());
        }

        /// <summary>
        /// The standard logger factory should create multiple new target instances.
        /// </summary>
        [Fact(DisplayName = "Should_CreateMultipleTargets")]
        public void Should_CreateMultipleTargets() {
            // Setup
            var configs = new List<TargetConfig> { new TargetConfig
                {
                    Name = "dummy",
                    Type = "NuLog.Tests.DummyTarget, NuLog.Tests"
                },new TargetConfig
                {
                    Name = "dummy",
                    Type = "NuLog.Targets.TraceTarget, NuLog"
                }
            };
            var config = new Config {
                Targets = configs
            };
            var factory = GetLogFactory(config);

            // Execute
            var targets = factory.MakeTargets();

            // Validate
            Assert.Equal(2, targets.Count);
            Assert.Contains(targets, t => typeof(DummyTarget).IsAssignableFrom(t.GetType()));
            Assert.Contains(targets, t => typeof(TraceTarget).IsAssignableFrom(t.GetType()));
        }

        /// <summary>
        /// The factory should create a new dispatcher.
        /// </summary>
        [Fact(DisplayName = "Should_BuildDispatcher")]
        public void Should_BuildDispatcher() {
            // Setup
            var factory = GetLogFactory(new Config());

            // Execute
            var dispatcher = factory.MakeDispatcher();

            // Verify
            Assert.NotNull(dispatcher);
        }

        /// <summary>
        /// The factory should create a new tag normalizer.
        /// </summary>
        [Fact(DisplayName = "Should_BuildTagNormalizer")]
        public void Should_BuildTagNormalizer() {
            // Setup
            var factory = GetLogFactory(new Config());

            // Execute
            var normalizer = factory.MakeTagNormalizer();

            // Verify
            Assert.NotNull(normalizer);
        }

        /// <summary>
        /// The standard logger factory should build a tag group processor.
        /// </summary>
        [Fact(DisplayName = "Should_BuildTagGroupProcessor")]
        public void Should_BuildTagGroupProcessor() {
            // Setup
            var factory = GetLogFactory(new Config());

            // Execute
            var tagGroupProcessor = factory.MakeTagGroupProcessor();

            // Verify
            Assert.NotNull(tagGroupProcessor);
        }

        /// <summary>
        /// The standard logger factory should build a rule processor.
        /// </summary>
        [Fact(DisplayName = "Should_BuildRuleProcessor")]
        public void Should_BuildRuleProcessor() {
            // Setup
            var factory = GetLogFactory(new Config {
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
            var ruleProcessor = factory.MakeRuleProcessor();

            // Verify
            Assert.NotNull(ruleProcessor);
        }

        /// <summary>
        /// The standard logger factory should build a tag router.
        /// </summary>
        [Fact(DisplayName = "Should_BuildTagRouter")]
        public void Should_BuildTagRouter() {
            // Setup
            var factory = GetLogFactory(new Config());

            // Execute
            var tagRouter = factory.MakeTagRouter();

            // Verify
            Assert.NotNull(tagRouter);
        }

        /// <summary>
        /// The standard logger factory should build a layout parser.
        /// </summary>
        [Fact(DisplayName = "Should_BuildLayoutParser")]
        public void Should_BuildLayoutParser() {
            // Setup
            var factory = GetLogFactory(null);

            // Execute
            var layoutParser = factory.MakeLayoutParser();

            // Verify
            Assert.NotNull(layoutParser);
        }

        /// <summary>
        /// The standard logger factory should build a property parser.
        /// </summary>
        [Fact(DisplayName = "Should_BuildPropertyParser")]
        public void Should_BuildPropertyParser() {
            // Setup
            var factory = GetLogFactory(null);

            // Execute
            var propertyParser = factory.MakePropertyParser();

            // Verify
            Assert.NotNull(propertyParser);
        }

        /// <summary>
        /// The standard logger factory should build a layout.
        /// </summary>
        [Fact(DisplayName = "Should_BuildLayout")]
        public void Should_BuildLayout() {
            // Setup
            var factory = GetLogFactory(null);

            // Execute
            var layout = factory.MakeLayout("${Message}");

            // Verify
            Assert.NotNull(layout);
        }

        /// <summary>
        /// The logger factory should build a logger instance.
        /// </summary>
        [Fact(DisplayName = "Should_BuildLogger")]
        public void Should_BuildLogger() {
            // Setup
            var factory = GetLogFactory(new Config());

            // Execute
            var logger = factory.GetLogger(null, null);

            // Verify
            Assert.NotNull(logger);
        }

        /// <summary>
        /// The factory should set the "IncludeStackFrame" flag on the logger when the config is also
        /// set to.
        /// </summary>
        [Fact(DisplayName = "Should_SetStackFrameFlagOnLogger")]
        public void Should_SetStackFrameFlagOnLogger() {
            // Setup
            var factory = GetLogFactory(new Config {
                IncludeStackFrame = true
            });

            // Execute
            var logger = factory.GetLogger(null, null);

            // Verify
            Assert.True(logger.IncludeStackFrame);
        }

        /// <summary>
        /// The factory should build a trace fallback logger when a fallback log (file path) isn't configured.
        /// </summary>
        [Fact(DisplayName = "Should_BuildTraceFallbackLoggerDefault")]
        public void Should_BuildTraceFallbackLoggerDefault() {
            // Setup
            var factory = GetLogFactory(new Config());

            // Execute
            var fallbackLogger = factory.MakeFallbackLogger();

            // Verify
            Assert.NotNull(fallbackLogger);
            Assert.True(typeof(StandardTraceFallbackLogger).IsAssignableFrom(fallbackLogger.GetType()));
        }

        /// <summary>
        /// When a fallback path is configured, the factory should build the file fallback logger.
        /// </summary>
        [Fact(DisplayName = "Should_BuildFileFallbackLoggerWhenConfigured")]
        public void Should_BuildFileFallbackLoggerWhenConfigured() {
            // Setup
            var factory = GetLogFactory(new Config {
                FallbackLogPath = "fallbacklog.txt"
            });

            // Execute
            var fallbackLogger = factory.MakeFallbackLogger();

            // Verify
            Assert.NotNull(fallbackLogger);
            Assert.True(typeof(StandardFileFallbackLogger).IsAssignableFrom(fallbackLogger.GetType()));
        }

        /// <summary>
        /// Failures in creating targets shouldn't bubble up through the dispatcher.
        /// </summary>
        [Fact(DisplayName = "Should_NotThrowExceptionForBadTargetType")]
        public void Should_NotThrowExceptionForBadTargetType() {
            // Setup
            var targetConfig = new TargetConfig {
                Name = "broken",
                Type = "NuLog.Tests.Unit.Factories.DoesNotExist, NuLog.Tests"
            };
            var configs = new List<TargetConfig> { targetConfig };
            var config = new Config {
                Targets = configs
            };

            var factory = GetLogFactory(config);

            // Execute
            var targets = factory.MakeTargets();

            // Validate
            Assert.Equal(0, targets.Count);
        }

        /// <summary>
        /// The factory shouldn't throw an exception for a target that exists, but that throws an
        /// exception on construction.
        /// </summary>
        [Fact(DisplayName = "Should_NotThrowExceptionForBrokenTarget")]
        public void Should_NotThrowExceptionForBrokenTarget() {
            // Setup
            var targetConfig = new TargetConfig {
                Name = "broken",
                Type = "NuLog.Tests.Unit.Factories.BrokenTarget, NuLog.Tests"
            };
            var configs = new List<TargetConfig> { targetConfig };
            var config = new Config {
                Targets = configs
            };

            var factory = GetLogFactory(config);

            // Execute
            var targets = factory.MakeTargets();

            // Validate
            Assert.Equal(0, targets.Count);
        }

        /// <summary>
        /// The factory shouldn't throw an exception when there's a failure in getting the fallback
        /// logger. It should instead, fallback to a trace fallback logger, and report the error there.
        /// </summary>
        [Fact(DisplayName = "Should_NotThrowExceptionForBadFallbackLogger")]
        public void Should_NotThrowExceptionForBadFallbackLogger() {
            // Setup / Execute
            var factory = new BrokenFactoryFallbackLogger(new Config());
            var fallback = factory.GetFallbackLogger();

            // Verify
            Assert.True(this.traceListener.Messages.Any(m => m.Contains("Failed to get fallback logger for cause: ")), "No message was traced warning of the failed construction of the fallback logger.");
        }

        [Fact(DisplayName = "Should_NotAllowGetLoggerAfterDispose")]
        public void Should_NotAllowGetLoggerAfterDispose() {
            // Setup
            var targetConfig = new TargetConfig {
                Name = "broken",
                Type = "NuLog.Tests.Unit.Factories.BrokenTarget, NuLog.Tests"
            };
            var configs = new List<TargetConfig> { targetConfig };
            var config = new Config {
                Targets = configs
            };

            var factory = GetLogFactory(config);
            factory.Dispose();

            // Execute/Validate
            Assert.Throws<InvalidOperationException>(() => factory.GetLogger(null, null));
        }
    }

    /// <summary>
    /// A broken target, which throws a "NotImplementedException" in the constructor.
    /// </summary>
    internal class BrokenTarget : ITarget {
        public string Name { get; set; }

        public BrokenTarget() {
            throw new NotImplementedException();
        }

        public void Configure(TargetConfig config) {
            throw new NotImplementedException();
        }

        public void Dispose() {
            throw new NotImplementedException();
        }

        public void Write(LogEvent logEvent) {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// A broken standard logger factory, which throws an exception when getting the fallback logger.
    /// </summary>
    internal class BrokenFactoryFallbackLogger : StandardLoggerFactory {

        public BrokenFactoryFallbackLogger(Config config) : base(config) {
        }

        public override IFallbackLogger MakeFallbackLogger() {
            // Deliberate - to test the "fallback" of the fallback logger, in the logger factory constructor.
            throw new NotImplementedException();
        }
    }
}