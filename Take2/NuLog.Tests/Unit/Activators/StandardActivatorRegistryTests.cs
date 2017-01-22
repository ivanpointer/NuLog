/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.Factories;
using System;
using Xunit;

namespace NuLog.Tests.Unit.Activators
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of a standard activator registry.
    /// </summary>
    [Trait("Category", "Unit")]
    public class StandardActivatorRegistryTests
    {
        /// <summary>
        /// The factory should store and retrieve an activator, based on its activator, and
        /// configuration type.
        /// </summary>
        [Fact(DisplayName = "Should_RegisterAndGetActivator")]
        public void Should_RegisterAndGetActivator()
        {
            // Setup
            var factory = GetLogFactory();
            var activator = GetFakeActivator<ILogger, string>();

            // Execute
            factory.RegisterActivator(activator);
            var registered = factory.GetActivator<ILogger, string>();

            // Verify
            Assert.Equal(activator, registered);
        }

        /// <summary>
        /// The factory should store and retrieve multiple different activators.
        /// </summary>
        [Fact(DisplayName = "Should_RegisterAndGetMultipleActivators")]
        public void Should_RegisterAndGetMultipleActivators()
        {
            // Setup
            var factory = GetLogFactory();
            var activator1 = GetFakeActivator<ILogger, string>();
            var activator2 = GetFakeActivator<ITarget, string>();

            // Execute
            factory.RegisterActivator(activator1);
            factory.RegisterActivator(activator2);
            var registered1 = factory.GetActivator<ILogger, string>();
            var registered2 = factory.GetActivator<ITarget, string>();

            // Verify
            Assert.Equal(activator1, registered1);
            Assert.Equal(activator2, registered2);
        }

        /// <summary>
        /// The factory should be able to register multiple activators for the same type, but, with
        /// different configuration types.
        /// </summary>
        [Fact(DisplayName = "Should_RegisterAndGetMultipleActivators_SameTypeDifferentConfig")]
        public void Should_RegisterAndGetMultipleActivators_SameTypeDifferentConfig()
        {
            // Setup
            var factory = GetLogFactory();
            var activator1 = GetFakeActivator<ILogger, string>();
            var activator2 = GetFakeActivator<ILogger, DateTime>();

            // Execute
            factory.RegisterActivator(activator1);
            factory.RegisterActivator(activator2);
            var registered1 = factory.GetActivator<ILogger, string>();
            var registered2 = factory.GetActivator<ILogger, DateTime>();

            // Verify
            Assert.Equal(activator1, registered1);
            Assert.Equal(activator2, registered2);
        }

        /// <summary>
        /// The factory should store and retrieve an activator, based on its activator, and
        /// configuration type.
        /// </summary>
        [Fact(DisplayName = "Should_RegisterAndGetActivator_Configless")]
        public void Should_RegisterAndGetActivator_Configless()
        {
            // Setup
            var factory = GetLogFactory();
            var activator = GetFakeActivator<ILogger>();

            // Execute
            factory.RegisterActivator(activator);
            var registered = factory.GetActivator<ILogger>();

            // Verify
            Assert.Equal(activator, registered);
        }

        /// <summary>
        /// The factory should store and retrieve multiple different activators.
        /// </summary>
        [Fact(DisplayName = "Should_RegisterAndGetMultipleActivators_Configless")]
        public void Should_RegisterAndGetMultipleActivators_Configless()
        {
            // Setup
            var factory = GetLogFactory();
            var activator1 = GetFakeActivator<ILogger>();
            var activator2 = GetFakeActivator<ITarget>();

            // Execute
            factory.RegisterActivator(activator1);
            factory.RegisterActivator(activator2);
            var registered1 = factory.GetActivator<ILogger>();
            var registered2 = factory.GetActivator<ITarget>();

            // Verify
            Assert.Equal(activator1, registered1);
            Assert.Equal(activator2, registered2);
        }

        /// <summary>
        /// Get a new instance of the activator registry under test.
        /// </summary>
        protected IActivatorRegistry GetLogFactory()
        {
            return new StandardLogFactory();
        }

        private static ILogFactoryActivator<TActivator, TConfiguration> GetFakeActivator<TActivator, TConfiguration>()
        {
            var activator = A.Fake<ILogFactoryActivator<TActivator, TConfiguration>>();
            A.CallTo(() => activator.GetActivatorType()).Returns(typeof(TActivator));
            A.CallTo(() => activator.GetConfigurationType()).Returns(typeof(TConfiguration));
            return activator;
        }

        private static ILogFactoryActivator<TActivator> GetFakeActivator<TActivator>()
        {
            var activator = A.Fake<ILogFactoryActivator<TActivator>>();
            A.CallTo(() => activator.GetActivatorType()).Returns(typeof(TActivator));
            A.CallTo(() => activator.GetConfigurationType()).Returns(LogFactoryActivatorNull.NullType);
            return activator;
        }
    }
}