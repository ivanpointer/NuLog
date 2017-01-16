/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Layouts;
using NuLog.Layouts.PropertyParsers;
using System;
using System.Collections.Generic;
using Xunit;

namespace NuLog.Tests.Unit.Layouts.PropertyParsers
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the property parser.
    /// </summary>
    [Trait("Category", "Unit")]
    public class PropertyParserTests
    {
        /// <summary>
        /// Passing a null path, should return a null value.
        /// </summary>
        [Fact(DisplayName = "Should_HandleNullPath")]
        public void Should_HandleNullPath()
        {
            // Setup
            var zobject = new MyTestClass
            {
                Hello = "World"
            };
            var parser = GetPropertyParser();

            // Execute
            var value = parser.GetProperty(zobject, null);

            // Verify
            Assert.Null(value);
        }

        /// <summary>
        /// Passing a null path, should return a null value.
        /// </summary>
        [Fact(DisplayName = "Should_HandleEmptyPath")]
        public void Should_HandleEmptyPath()
        {
            // Setup
            var zobject = new MyTestClass
            {
                Hello = "World"
            };
            var parser = GetPropertyParser();

            // Execute
            var value = parser.GetProperty(zobject, "");

            // Verify
            Assert.Null(value);
        }

        /// <summary>
        /// The property parser should be able to find a top-level simple property.
        /// </summary>
        [Fact(DisplayName = "Should_FindSimpleProperty")]
        public void Should_FindSimpleProperty()
        {
            // Setup
            var zobject = new MyTestClass
            {
                Hello = "World"
            };
            var parser = GetPropertyParser();

            // Execute
            var value = parser.GetProperty(zobject, "Hello");

            // Verify
            Assert.Equal(zobject.Hello, value);
        }

        /// <summary>
        /// The property parser should be able to find a top-level DateTime property.  This also checks the method's handling of structs.
        /// </summary>
        [Fact(DisplayName = "Should_FindDateTimeProperty")]
        public void Should_FindDateTimeProperty()
        {
            // Setup
            var zobject = new MyTestClass
            {
                MyDate = new DateTime(2017, 1, 16, 9, 42, 43)
            };
            var parser = GetPropertyParser();

            // Execute
            var value = parser.GetProperty(zobject, "MyDate");

            // Verify
            Assert.Equal(zobject.MyDate, value);
        }

        /// <summary>
        /// Should find a property that is nested down a layer.
        /// </summary>
        [Fact(DisplayName = "Should_FindSimpleNestedProperty")]
        public void Should_FindSimpleNestedProperty()
        {
            // Setup
            var zobject = new MyTestClass
            {
                MyDate = new DateTime(2017, 1, 16, 9, 42, 43)
            };
            var parser = GetPropertyParser();

            // Execute
            var value = parser.GetProperty(zobject, "MyDate.Day");

            // Verify
            Assert.Equal(16, value);
        }

        /// <summary>
        /// The parser should recognize a dictionary, and be able to traverse it - at the top level.
        /// </summary>
        [Fact(DisplayName = "Should_HandleDictionary")]
        public void Should_HandleDictionary()
        {
            // Setup
            var zobject = new MyTestClass
            {
                Hello = "Howdy"
            };
            var zdict = new Dictionary<string, object>
            {
                { "ZObject", zobject }
            };
            var parser = GetPropertyParser();

            // Execute
            var value = parser.GetProperty(zdict, "ZObject.Hello");

            // Verify
            Assert.Equal("Howdy", value);
        }

        /// <summary>
        /// The parser should recognize a dictionary, and be able to traverse it - at a nested level.
        /// </summary>
        [Fact(DisplayName = "Should_HandleNestedDictionary")]
        public void Should_HandleNextedDictionary()
        {
            // Setup
            var zobject = new MyTestClass
            {
                MyStuffs = new Dictionary<string, object>
                {
                    { "ZObject", new MyTestClass {
                        Hello = "Hey There!"
                    }}
                }
            };
            var parser = GetPropertyParser();

            // Execute
            var value = parser.GetProperty(zobject, "MyStuffs.ZObject.Hello");

            // Verify
            Assert.Equal("Hey There!", value);
        }

        /// <summary>
        /// Returns a new instance of the property parser under test.
        /// </summary>
        protected IPropertyParser GetPropertyParser()
        {
            return new StandardPropertyParser();
        }
    }

    /// <summary>
    /// A test class for exercising our property parser
    /// </summary>
    internal class MyTestClass
    {
        public string Hello { get; set; }

        public DateTime MyDate { get; set; }

        public Dictionary<string, object> MyStuffs { get; set; }
    }
}