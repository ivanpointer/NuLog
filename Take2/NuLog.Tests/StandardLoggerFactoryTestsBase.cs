/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.Factories;
using Xunit.Abstractions;

namespace NuLog.Tests
{
    /// <summary>
    /// Defines common functionality for the standard logger factory tests. Tests exist in both
    /// "Unit" and "Integration" categories, but test the same factory, so this has been brought to a
    /// common scope above each.
    /// </summary>
    public abstract class StandardLoggerFactoryTestsBase : TraceListenerTestsBase
    {
        public StandardLoggerFactoryTestsBase(ITestOutputHelper output) : base(output)
        {
        }

        /// <summary>
        /// Get the logger factory under test.
        /// </summary>
        protected StandardLoggerFactory GetLogFactory(Config config)
        {
            return new StandardLoggerFactory(config);
        }
    }
}