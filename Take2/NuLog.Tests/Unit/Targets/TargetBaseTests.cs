/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.LogEvents;
using NuLog.Targets;
using System;
using System.Collections.Generic;
using Xunit;

namespace NuLog.Tests.Unit.Targets
{
    /// <summary>
    /// Defines (and verifies) the expected behavior of the target base class.
    /// </summary>
    [Trait("Category", "Unit")]
    public class TargetBaseTests
    {
        #region Get Property Helpers

        /// <summary>
        /// The target base should be able to get a string property from the config.
        /// </summary>
        [Fact(DisplayName = "Should_GetStringProperty")]
        public void Should_GetStringProperty()
        {
            // Setup
            var target = new DummyTarget();
            var config = BuildTargetConfigProperty("hello, world!");

            // Execute
            var prop = target.GetPropertyInternal<string>(config);

            // Verify
            Assert.Equal("hello, world!", prop);
        }

        /// <summary>
        /// The target base should be able to get a complex object property from the config.
        /// </summary>
        [Fact(DisplayName = "Should_GetComplexObjectProperty")]
        public void Should_GetComplexObjectProperty()
        {
            // Setup
            var target = new DummyTarget();
            var complexObject = new ComplexObject();
            var config = BuildTargetConfigProperty(complexObject);

            // Execute
            var prop = target.GetPropertyInternal<ComplexObject>(config);

            // Verify
            Assert.Equal(complexObject, prop);
        }

        /// <summary>
        /// Should be able to get a more base form of a property, than is stored in the properties.
        /// </summary>
        [Fact(DisplayName = "Should_GetComplexObjectPropertyBaseClass")]
        public void Should_GetComplexObjectPropertyBaseClass()
        {
            // Setup
            var target = new DummyTarget();
            var complexObject = new ComplexObjectExt();
            var config = BuildTargetConfigProperty(complexObject);

            // Execute
            var prop = target.GetPropertyInternal<ComplexObject>(config);

            // Verify
            Assert.Equal(complexObject, prop);
        }

        /// <summary>
        /// An InvalidCastException should be thrown when trying to get a property of an incompatible type.
        /// </summary>
        [Fact(DisplayName = "Should_ThrowInvalidCastExceptionOnIncompatibleCast")]
        public void Should_ThrowInvalidCastExceptionOnIncompatibleCast()
        {
            // Setup
            var target = new DummyTarget();
            var invalidObject = new List<string>();
            var config = BuildTargetConfigProperty(invalidObject);

            // Execute / Verify
            Assert.Throws(typeof(InvalidCastException), () =>
            {
                target.GetPropertyInternal<ComplexObject>(config);
            });
        }

        /// <summary>
        /// A try get property should be exposed.
        /// </summary>
        [Fact(DisplayName = "Should_TryGetPropertySuccess")]
        public void Should_TryGetPropertySuccess()
        {
            // Setup
            var target = new DummyTarget();
            var config = BuildTargetConfigProperty("hello, world!");

            // Execute
            string prop;
            var success = target.TryGetPropertyInternal<string>(config, out prop);

            // Verify
            Assert.True(success);
            Assert.Equal("hello, world!", prop);
        }

        /// <summary>
        /// The try-get property helper should return null (default) when the property doesn't exist.
        /// </summary>
        [Fact(DisplayName = "Should_TryGetPropertyMissing")]
        public void Should_TryGetPropertyMissing()
        {
            // Setup
            var target = new DummyTarget();
            var config = BuildTargetConfigProperty("hello, world!");

            // Execute
            string prop;
            var success = target.TryGetPropertyInternal<string>(config, out prop, "notMyProp");

            // Verify
            Assert.False(success);
            Assert.Null(prop);
        }

        /// <summary>
        /// The try-get property helper should return null (default) when the property's type doesn't match.
        /// </summary>
        [Fact(DisplayName = "Should_TryGetPropertyBadType")]
        public void Should_TryGetPropertyBadType()
        {
            // Setup
            var target = new DummyTarget();
            var invalidObject = new List<string>();
            var config = BuildTargetConfigProperty(invalidObject);

            // Execute
            ComplexObject prop;
            var success = target.TryGetPropertyInternal<ComplexObject>(config, out prop);

            // Verify
            Assert.False(success);
            Assert.Null(prop);
        }

        #endregion Get Property Helpers

        #region Helpers

        /// <summary>
        /// A helper method for building a target config with a single property, for testing the
        /// property helpers of the target base class.
        /// </summary>
        private static TargetConfig BuildTargetConfigProperty(object value, string name = "myProp")
        {
            var props = BuildTestProperty(value, name);
            return new TargetConfig
            {
                Properties = props
            };
        }

        /// <summary>
        /// A helper method to build a test property for testing the property helpers on the target base.
        /// </summary>
        private static IDictionary<string, object> BuildTestProperty(object value, string name = "myProp")
        {
            return new Dictionary<string, object>
            {
                { name, value }
            };
        }

        #endregion Helpers
    }

    /// <summary>
    /// A dummy target to exercise the functionality of the target base class.
    /// </summary>
    internal class DummyTarget : TargetBase
    {
        /// <summary>
        /// Exposes the get property helper.
        /// </summary>
        public TProperty GetPropertyInternal<TProperty>(TargetConfig config, string name = "myProp")
        {
            return GetProperty<TProperty>(config, name);
        }

        /// <summary>
        /// Exposes the try-get property helper.
        /// </summary>
        public bool TryGetPropertyInternal<TProperty>(TargetConfig config, out TProperty property, string name = "myProp")
        {
            return TryGetProperty(config, name, out property);
        }

        public override void Write(LogEvent logEvent)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// A "complex" object for testing getting properties from the target base.
    /// </summary>
    internal class ComplexObject
    {
        public string SomeProperty { get; set; }
    }

    /// <summary>
    /// An extension class for testing getting the base type from properties.
    /// </summary>
    internal class ComplexObjectExt : ComplexObject
    {
    }
}