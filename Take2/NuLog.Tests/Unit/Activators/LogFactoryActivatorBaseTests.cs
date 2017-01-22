/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Activators;
using System;
using System.Collections.Generic;
using Xunit;

namespace NuLog.Tests.Unit.Activators
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the log factory activator base class.
    /// </summary>
    [Trait("Category", "Unit")]
    public class LogFactoryActivatorBaseTests
    {
        /// <summary>
        /// The activator should correctly report the activator type.
        /// </summary>
        [Fact(DisplayName = "Should_ReportActivatorType")]
        public void Should_ReportActivatorType()
        {
            // Setup
            var activator = new DummyLogFactoryActivator<List<string>, DateTime>();

            // Execute
            var type = activator.GetActivatorType();

            // Verify
            Assert.Equal(typeof(List<string>), type);
        }

        /// <summary>
        /// The activator should correctly report the configuration type.
        /// </summary>
        [Fact(DisplayName = "Should_ReportConfigurationType")]
        public void Should_ReportConfigurationType()
        {
            // Setup
            var activator = new DummyLogFactoryActivator<List<string>, DateTime>();

            // Execute
            var type = activator.GetConfigurationType();

            // Verify
            Assert.Equal(typeof(DateTime), type);
        }

        /// <summary>
        /// The null configuration type should be reported when using the configuration-less
        /// implementation of the base.
        /// </summary>
        [Fact(DisplayName = "Should_ReportNullConfigurationType")]
        public void Should_ReportNullConfigurationType()
        {
            // Setup
            var activator = new DummyLogFactoryActivator<List<string>>();

            // Execute
            var type = activator.GetConfigurationType();

            // Verify
            Assert.Equal(LogFactoryActivatorNull.NullType, type);
        }

        /// <summary>
        /// The correct type should be reported when using the configuration-less implementation of
        /// the base.
        /// </summary>
        [Fact(DisplayName = "Should_ReportTypeWithConfigurationless")]
        public void Should_ReportTypeWithConfigurationless()
        {
            // Setup
            var activator = new DummyLogFactoryActivator<List<string>>();

            // Execute
            var type = activator.GetActivatorType();

            // Verify
            Assert.Equal(typeof(List<string>), type);
        }

        /// <summary>
        /// Should through a InvalidOperationException when calling "BuildNew" with a configuration
        /// item, on the configuration-less implementation of the log factory activator base.
        /// </summary>
        [Fact(DisplayName = "Should_ThrowInvalidOperationWhenCallingWithConfigOnConfigless")]
        public void Should_ThrowInvalidOperationWhenCallingWithConfigOnConfigless()
        {
            // Setup
            var activator = new DummyLogFactoryActivator<List<string>>();

            // Execute / Verify
            Assert.Throws(typeof(InvalidOperationException), () =>
            {
                activator.BuildNew(LogFactoryActivatorNull.NullObject);
            });
        }
    }

    /// <summary>
    /// A dummy implementation of the log factory activator base class, for testing the functionality
    /// of the base class.
    /// </summary>
    internal class DummyLogFactoryActivator<TActivator, TConfiguration> : LogFactoryActivatorBase<TActivator, TConfiguration>
    {
        public override TActivator BuildNew(TConfiguration config)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// A dummy implementation of the log factory activator base class, for testing the functionality
    /// of the base class.
    /// </summary>
    internal class DummyLogFactoryActivator<TActivator> : LogFactoryActivatorBase<TActivator>
    {
        public override TActivator BuildNew()
        {
            throw new NotImplementedException();
        }
    }
}