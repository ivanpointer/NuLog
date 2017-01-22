/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.Factories;
using System;
using Xunit;

namespace NuLog.Tests.Unit.Factories
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the standard log factory.
    /// </summary>
    [Trait("Category", "Unit")]
    public class StandardLogFactoryTests
    {
        #region Activator Registration



        #endregion Activator Registration

        #region Building Targets

        ///// <summary>
        ///// The factory should leverage the activator for targets, to build targets
        ///// </summary>
        //[Fact(DisplayName = "Should_UseTargetActivatorToBuildTargets")]
        //public void Should_UseTargetActivatorToBuildTargets()
        //{
        //    throw new NotImplementedException();
        //}

        ///// <summary>
        ///// The standard log factory should create a new target instance.
        ///// </summary>
        //[Fact(DisplayName = "Should_CreateNewTargetInstance")]
        //public void Should_CreateNewTargetInstance()
        //{
        //    // Setup
        //    var targetConfig = new TargetConfig
        //    {
        //        Name = "dummy",
        //        Type = "NuLog.Tests.Unit.Factories.DummyTarget, NuLog.Tests"
        //    };
        //    var configs = new List<TargetConfig> { targetConfig };

        // var factory = GetLogFactory();

        // // Execute var targets = factory.GetTargets(configs);

        //    // Validate
        //    var target = targets.Single();
        //    Assert.True(typeof(DummyTarget).IsAssignableFrom(target.GetType()));
        //}

        ///// <summary>
        ///// When creating a new target, the factory should set the target's name.
        ///// </summary>
        //[Fact(DisplayName = "Should_SetTargetNameOnCreate")]
        //public void Should_SetTargetNameOnCreate()
        //{
        //    // Setup
        //    var targetConfig = new TargetConfig
        //    {
        //        Name = "dummy",
        //        Type = "NuLog.Tests.Unit.Factories.DummyTarget, NuLog.Tests"
        //    };
        //    var configs = new List<TargetConfig> { targetConfig };

        // var factory = GetLogFactory();

        // // Execute var targets = factory.GetTargets(configs);

        //    // Validate
        //    var target = targets.Single();
        //    Assert.Equal("dummy", target.Name);
        //}

        ///// <summary>
        ///// The factory should call "configure" on the target when it is created.
        ///// </summary>
        //[Fact(DisplayName = "Should_CallConfigure")]
        //public void Should_CallConfigure()
        //{
        //    // Setup
        //    var targetConfig = new TargetConfig
        //    {
        //        Name = "dummy",
        //        Type = "NuLog.Tests.Unit.Factories.DummyTarget, NuLog.Tests"
        //    };
        //    var configs = new List<TargetConfig> { targetConfig };

        // var factory = GetLogFactory();

        // // Execute var targets = factory.GetTargets(configs);

        //    // Validate
        //    var target = targets.Single();
        //    Assert.Equal(1, ((DummyTarget)target).ConfigureCallCount);
        //}

        ///// <summary>
        ///// The factory should pass the target's configuration to the call to config on the target.
        ///// </summary>
        //[Fact(DisplayName = "Should_PassConfigToConfigCall")]
        //public void Should_PassConfigToConfigCall()
        //{
        //    // Setup
        //    var targetConfig = new TargetConfig
        //    {
        //        Name = "dummy",
        //        Type = "NuLog.Tests.Unit.Factories.DummyTarget, NuLog.Tests"
        //    };
        //    var configs = new List<TargetConfig> { targetConfig };

        // var factory = GetLogFactory();

        // // Execute var targets = factory.GetTargets(configs);

        //    // Validate
        //    var target = (DummyTarget)targets.Single();
        //    var passedConfig = target.ConfigsPassed.Single();
        //    Assert.Equal(targetConfig, passedConfig);
        //}

        #endregion Building Targets

        #region Internals



        #endregion Internals
    }

    ///// <summary>
    ///// A dummy target to test the logger factory's ability to create new target instances.
    ///// </summary>
    //internal class DummyTarget : TargetBase
    //{
    //    public int ConfigureCallCount { get; private set; }

    // public ICollection<TargetConfig> ConfigsPassed { get; private set; }

    // public DummyTarget() { this.ConfigsPassed = new List<TargetConfig>(); }

    // public override void Configure(TargetConfig config) { ConfigureCallCount++;

    // this.ConfigsPassed.Add(config);

    // base.Configure(config); }

    //    public override void Write(LogEvent logEvent)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}