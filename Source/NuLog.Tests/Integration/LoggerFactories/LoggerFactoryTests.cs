/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.Configuration.Targets;
using NuLog.Targets;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace NuLog.Tests.Integration.LoggerFactories
{
    /// <summary>
    /// MIGRATED FROM EXISTING TEST PROJECT - Need to take a look at each of the NuLog components
    /// again, and rework to the DIP, to have better test isolation (I.E. true unit tests).
    ///
    /// I pulled these in as "integration" tests, even though some may qualify as unit tests, as this
    /// is a quick and dirty port.
    ///
    /// Documents (and validates) the expected behavior of the logger factory.
    /// </summary>
    [Trait("Category", "Integration")]
    [Trait("Domain", "LoggerFactories")]
    public class LoggerFactoryTests
    {
        private LoggerFactoryTestsListTarget ListTarget { get; set; }

        public LoggerFactoryTests()
        {
            ListTarget = new LoggerFactoryTestsListTarget();
        }

        [Fact]
        public void TestGetLogger()
        {
            LoggerFactory.GetLogger(defaultTags: new string[] { "abc", "123" });
        }

        [Fact]
        public void TestShutdown()
        {
            var config = LoggingConfigBuilder.CreateLoggingConfig()
                .AddTarget(new TargetConfig
                {
                    Name = "list",
                    Type = string.Format("{0}, NuLog.Tests", typeof(LoggerFactoryTestsListTarget).FullName)
                })
                .Build();

            using (var factory = new LoggerFactory(config))
            {
                var logger = factory.Logger();

                ListTarget.ClearList();
                for (var x = 0; x < 1000; x++)
                    logger.Log("Log message " + x);

                factory.Shutdown();

                var list = ListTarget.GetList();
                Assert.Equal(1000, list.Count);
            }
        }

        [Fact]
        public void TestDispose()
        {
            var config = LoggingConfigBuilder.CreateLoggingConfig()
                .AddTarget(new TargetConfig
                {
                    Name = "list",
                    Type = string.Format("{0}, NuLog.Tests", typeof(LoggerFactoryTestsListTarget).FullName)
                })
                .Build();

            using (var factory = new LoggerFactory(config))
            {
                var logger = factory.Logger();

                ListTarget.ClearList();
                for (var x = 0; x < 1000; x++)
                    logger.Log("Log message " + x);
            }

            var list = ListTarget.GetList();
            Assert.Equal(1000, list.Count);
        }
    }

    internal class LoggerFactoryTestsListTarget : TargetBase
    {
        private static readonly object _listLock = new object();
        private static readonly IList<LogEvent> _logEvents = new List<LogEvent>();

        public override void Log(LogEvent logEvent)
        {
            lock (_listLock)
            {
                _logEvents.Add(logEvent);
            }
        }

        public IList<LogEvent> GetList()
        {
            lock (_listLock)
            {
                return new ReadOnlyCollection<LogEvent>(_logEvents);
            }
        }

        public void ClearList()
        {
            lock (_listLock)
            {
                _logEvents.Clear();
            }
        }
    }
}