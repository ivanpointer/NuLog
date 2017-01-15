/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.Configuration.Targets;
using NuLog.Targets;
using NuLog.Tests.Scaffolding;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Xunit;

namespace NuLog.Tests.Integration.MetaDataProviders
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
    [Trait("Domain", "MetaData")]
    public class MetaDataProvidersTests
    {
        private MetaDataProvidersTestsListsTarget ListTarget { get; set; }

        public MetaDataProvidersTests()
        {
            ListTarget = new MetaDataProvidersTestsListsTarget();
        }

        [Fact]
        public void TestMetaDataProvider()
        {
            var config = LoggingConfigBuilder.CreateLoggingConfig()
                .AddTarget(new TargetConfig
                {
                    Name = "list",
                    Type = string.Format("{0}, NuLog.Tests", typeof(MetaDataProvidersTestsListsTarget).FullName)
                })
                .Build();

            using (var factory = new LoggerFactory(config))
            {
                var metaDataProvider = new TestMetaDataProvider();

                var logger = factory.Logger(metaDataProvider);

                logger.LogNow("This is a test");

                for (int checks = 0; checks < 20; checks++)
                {
                    Thread.Sleep(100);
                    if (ListTarget.GetList().Count > 0)
                        break;
                }

                var list = ListTarget.GetList();
                Assert.NotNull(list);
                Assert.Equal(1, list.Count);

                var message = list[0];
                Assert.NotNull(message);
                Assert.True(message.MetaData.ContainsKey("Hello"));
                Assert.True(message.MetaData["Hello"].ToString() == "World");
                Assert.True(message.MetaData.ContainsKey("Addtl"));
                Assert.True(message.MetaData["Addtl"].ToString() == "Data");
            }
        }
    }

    internal class MetaDataProvidersTestsListsTarget : TargetBase
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