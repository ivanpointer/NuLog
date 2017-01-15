/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.Dispatch;
using System.Collections.Generic;
using Xunit;

namespace NuLog.Tests.Integration.Rules
{
    /// <summary>
    /// MIGRATED FROM EXISTING TEST PROJECT - Need to take a look at each of the NuLog components
    /// again, and rework to the DIP, to have better test isolation (I.E. true unit tests).
    ///
    /// I pulled these in as "integration" tests, even though some may qualify as unit tests, as this
    /// is a quick and dirty port.
    ///
    /// Documents (and validates) the expected behavior of the rules engine.
    /// </summary>
    [Trait("Category", "Integration")]
    [Trait("Domain", "Rules")]
    public class RulesTests
    {
        [Fact]
        public void TestSimpleIncludeRule()
        {
            var config = new LoggingConfig();
            config.LoadConfig(@"Configs\SimpleIncludeRuleTest.json");
            var dispatcher = new LogEventDispatcher(config);
            var ruleKeeper = dispatcher.RuleKeeper;

            var infoTargets = ruleKeeper.GetTargetsForTags(new List<string> { "info" });
            Assert.NotNull(infoTargets);
            Assert.True(infoTargets.Count == 1);
            Assert.True(infoTargets.Contains("console"));

            var traceTargets = ruleKeeper.GetTargetsForTags(new List<string> { "trace" });
            Assert.NotNull(traceTargets);
            Assert.True(traceTargets.Count == 0);
        }

        [Fact]
        public void TestStrictIncludeRule()
        {
            var config = new LoggingConfig();
            config.LoadConfig(@"Configs\StrictIncludeRuleTest.json");
            var dispatcher = new LogEventDispatcher(config);
            var ruleKeeper = dispatcher.RuleKeeper;

            var strictTargets = ruleKeeper.GetTargetsForTags(new List<string> { this.GetType().FullName, "info" });
            Assert.NotNull(strictTargets);
            Assert.True(strictTargets.Count == 1);
            Assert.True(strictTargets.Contains("console"));

            var incompleteTargets = ruleKeeper.GetTargetsForTags(new List<string> { "info" });
            Assert.NotNull(incompleteTargets);
            Assert.True(incompleteTargets.Count == 0);
        }

        [Fact]
        public void TestSimpleExcludeRule()
        {
            var config = new LoggingConfig();
            config.LoadConfig(@"Configs\SimpleExcludeRuleTest.json");
            var dispatcher = new LogEventDispatcher(config);
            var ruleKeeper = dispatcher.RuleKeeper;

            var includeTargets = ruleKeeper.GetTargetsForTags(new List<string> { "includeme" });
            Assert.NotNull(includeTargets);
            Assert.True(includeTargets.Count == 1);
            Assert.True(includeTargets.Contains("console"));

            var simpleExcludeTargets = ruleKeeper.GetTargetsForTags(new List<string> { "excludeme" });
            Assert.NotNull(simpleExcludeTargets);
            Assert.True(simpleExcludeTargets.Count == 0);

            var excludeTargets = ruleKeeper.GetTargetsForTags(new List<string> { "dontincludeme", "excludemetoo" });
            Assert.NotNull(excludeTargets);
            Assert.True(excludeTargets.Count == 0);
        }

        [Fact]
        public void TestMultipleRule()
        {
            var config = new LoggingConfig();
            config.LoadConfig(@"Configs\MultipleRuleTest.json");
            var dispatcher = new LogEventDispatcher(config);
            var ruleKeeper = dispatcher.RuleKeeper;

            var cond1Targets = ruleKeeper.GetTargetsForTags(new List<string> { "cond1" });
            Assert.NotNull(cond1Targets);
            Assert.True(cond1Targets.Count == 2);
            Assert.True(cond1Targets.Contains("target1"));
            Assert.True(cond1Targets.Contains("target2"));

            var cond2Targets = ruleKeeper.GetTargetsForTags(new List<string> { "cond2" });
            Assert.NotNull(cond2Targets);
            Assert.True(cond2Targets.Count == 1);
            Assert.True(cond2Targets.Contains("target2"));

            var cond3Targets = ruleKeeper.GetTargetsForTags(new List<string> { "cond3" });
            Assert.NotNull(cond3Targets);
            Assert.True(cond3Targets.Count == 1);
            Assert.True(cond3Targets.Contains("target1"));
        }

        [Fact]
        public void TestFinalRule()
        {
            var config = new LoggingConfig();
            config.LoadConfig(@"Configs\FinalRuleTest.json");
            var dispatcher = new LogEventDispatcher(config);
            var ruleKeeper = dispatcher.RuleKeeper;

            var cond1Targets = ruleKeeper.GetTargetsForTags(new List<string> { "cond1" });
            Assert.NotNull(cond1Targets);
            Assert.True(cond1Targets.Count == 2);
            Assert.True(cond1Targets.Contains("target1"));
            Assert.True(cond1Targets.Contains("target3"));

            var cond2Targets = ruleKeeper.GetTargetsForTags(new List<string> { "cond2" });
            Assert.NotNull(cond2Targets);
            Assert.True(cond2Targets.Count == 1);
            Assert.True(cond2Targets.Contains("target2"));

            var cond3Targets = ruleKeeper.GetTargetsForTags(new List<string> { "cond3" });
            Assert.NotNull(cond3Targets);
            Assert.True(cond3Targets.Count == 1);
            Assert.True(cond3Targets.Contains("target3"));
        }

        [Fact]
        public void TestComplexRule()
        {
            var config = new LoggingConfig();
            config.LoadConfig(@"Configs\ComplexRuleTest.json");
            var dispatcher = new LogEventDispatcher(config);
            var ruleKeeper = dispatcher.RuleKeeper;

            var infoTargets = ruleKeeper.GetTargetsForTags(new List<string> { "info", this.GetType().FullName });
            Assert.NotNull(infoTargets);
            Assert.True(infoTargets.Count == 1);
            Assert.True(infoTargets.Contains("console"));

            var traceTargets = ruleKeeper.GetTargetsForTags(new List<string> { "trace", this.GetType().FullName });
            Assert.NotNull(traceTargets);
            Assert.True(traceTargets.Count == 0);

            var infoTargetTargets = ruleKeeper.GetTargetsForTags(new List<string> { "info", "NuLog.Targets", this.GetType().FullName });
            Assert.NotNull(infoTargetTargets);
            Assert.True(infoTargetTargets.Count == 0);
        }
    }
}