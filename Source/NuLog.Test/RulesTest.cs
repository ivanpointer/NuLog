using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuLog.Configuration;
using NuLog.Dispatch;
using System.Collections.Generic;

namespace NuLog.Test
{
    [TestClass]
    public class RulesTest
    {
        [TestMethod]
        public void TestSimpleIncludeRule()
        {
            var config = new LoggingConfig();
            config.LoadConfig(@"Configs\SimpleIncludeRuleTest.json");
            var dispatcher = new LogEventDispatcher(config);
            var ruleKeeper = dispatcher.RuleKeeper;

            var infoTargets = ruleKeeper.GetTargetsForTags(new List<string> { "info" });
            Assert.IsNotNull(infoTargets);
            Assert.IsTrue(infoTargets.Count == 1);
            Assert.IsTrue(infoTargets.Contains("console"));

            var traceTargets = ruleKeeper.GetTargetsForTags(new List<string> { "trace" });
            Assert.IsNotNull(traceTargets);
            Assert.IsTrue(traceTargets.Count == 0);
        }

        [TestMethod]
        public void TestStrictIncludeRule()
        {
            var config = new LoggingConfig();
            config.LoadConfig(@"Configs\StrictIncludeRuleTest.json");
            var dispatcher = new LogEventDispatcher(config);
            var ruleKeeper = dispatcher.RuleKeeper;

            var strictTargets = ruleKeeper.GetTargetsForTags(new List<string> { this.GetType().FullName, "info" });
            Assert.IsNotNull(strictTargets);
            Assert.IsTrue(strictTargets.Count == 1);
            Assert.IsTrue(strictTargets.Contains("console"));

            var incompleteTargets = ruleKeeper.GetTargetsForTags(new List<string> { "info" });
            Assert.IsNotNull(incompleteTargets);
            Assert.IsTrue(incompleteTargets.Count == 0);
        }

        [TestMethod]
        public void TestSimpleExcludeRule()
        {
            var config = new LoggingConfig();
            config.LoadConfig(@"Configs\SimpleExcludeRuleTest.json");
            var dispatcher = new LogEventDispatcher(config);
            var ruleKeeper = dispatcher.RuleKeeper;

            var includeTargets = ruleKeeper.GetTargetsForTags(new List<string> { "includeme" });
            Assert.IsNotNull(includeTargets);
            Assert.IsTrue(includeTargets.Count == 1);
            Assert.IsTrue(includeTargets.Contains("console"));

            var simpleExcludeTargets = ruleKeeper.GetTargetsForTags(new List<string> { "excludeme" });
            Assert.IsNotNull(simpleExcludeTargets);
            Assert.IsTrue(simpleExcludeTargets.Count == 0);

            var excludeTargets = ruleKeeper.GetTargetsForTags(new List<string> { "dontincludeme", "excludemetoo" });
            Assert.IsNotNull(excludeTargets);
            Assert.IsTrue(excludeTargets.Count == 0);
        }

        [TestMethod]
        public void TestMultipleRule()
        {
            var config = new LoggingConfig();
            config.LoadConfig(@"Configs\MultipleRuleTest.json");
            var dispatcher = new LogEventDispatcher(config);
            var ruleKeeper = dispatcher.RuleKeeper;

            var cond1Targets = ruleKeeper.GetTargetsForTags(new List<string> { "cond1" });
            Assert.IsNotNull(cond1Targets);
            Assert.IsTrue(cond1Targets.Count == 2);
            Assert.IsTrue(cond1Targets.Contains("target1"));
            Assert.IsTrue(cond1Targets.Contains("target2"));

            var cond2Targets = ruleKeeper.GetTargetsForTags(new List<string> { "cond2" });
            Assert.IsNotNull(cond2Targets);
            Assert.IsTrue(cond2Targets.Count == 1);
            Assert.IsTrue(cond2Targets.Contains("target2"));

            var cond3Targets = ruleKeeper.GetTargetsForTags(new List<string> { "cond3" });
            Assert.IsNotNull(cond3Targets);
            Assert.IsTrue(cond3Targets.Count == 1);
            Assert.IsTrue(cond3Targets.Contains("target1"));
        }

        [TestMethod]
        public void TestFinalRule()
        {
            var config = new LoggingConfig();
            config.LoadConfig(@"Configs\FinalRuleTest.json");
            var dispatcher = new LogEventDispatcher(config);
            var ruleKeeper = dispatcher.RuleKeeper;

            var cond1Targets = ruleKeeper.GetTargetsForTags(new List<string> { "cond1" });
            Assert.IsNotNull(cond1Targets);
            Assert.IsTrue(cond1Targets.Count == 2);
            Assert.IsTrue(cond1Targets.Contains("target1"));
            Assert.IsTrue(cond1Targets.Contains("target3"));

            var cond2Targets = ruleKeeper.GetTargetsForTags(new List<string> { "cond2" });
            Assert.IsNotNull(cond2Targets);
            Assert.IsTrue(cond2Targets.Count == 1);
            Assert.IsTrue(cond2Targets.Contains("target2"));

            var cond3Targets = ruleKeeper.GetTargetsForTags(new List<string> { "cond3" });
            Assert.IsNotNull(cond3Targets);
            Assert.IsTrue(cond3Targets.Count == 1);
            Assert.IsTrue(cond3Targets.Contains("target3"));
        }

        [TestMethod]
        public void TestComplexRule()
        {
            var config = new LoggingConfig();
            config.LoadConfig(@"Configs\ComplexRuleTest.json");
            var dispatcher = new LogEventDispatcher(config);
            var ruleKeeper = dispatcher.RuleKeeper;

            var infoTargets = ruleKeeper.GetTargetsForTags(new List<string> { "info", this.GetType().FullName });
            Assert.IsNotNull(infoTargets);
            Assert.IsTrue(infoTargets.Count == 1);
            Assert.IsTrue(infoTargets.Contains("console"));

            var traceTargets = ruleKeeper.GetTargetsForTags(new List<string> { "trace", this.GetType().FullName });
            Assert.IsNotNull(traceTargets);
            Assert.IsTrue(traceTargets.Count == 0);

            var infoTargetTargets = ruleKeeper.GetTargetsForTags(new List<string> { "info", "NuLog.Targets", this.GetType().FullName });
            Assert.IsNotNull(infoTargetTargets);
            Assert.IsTrue(infoTargetTargets.Count == 0);
        }
    }
}