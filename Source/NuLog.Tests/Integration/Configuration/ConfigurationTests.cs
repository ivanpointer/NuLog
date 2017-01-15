/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using Newtonsoft.Json.Linq;
using NuLog.Configuration;
using NuLog.Configuration.Targets;
using NuLog.Targets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Xunit;

namespace NuLog.Tests.Integration.Configuration
{
    /// <summary>
    /// MIGRATED FROM EXISTING TEST PROJECT - Need to take a look at each of the NuLog components
    /// again, and rework to the DIP, to have better test isolation (I.E. true unit tests).
    ///
    /// I pulled these in as "integration" tests, even though some may qualify as unit tests, as this
    /// is a quick and dirty port. There was a bit of test harnessing done in the previous test fixture.
    ///
    /// This harnessing/scaffolding is actually better framed in xUnit - so I'll want to also change
    /// this scaffolding to leverage xUnit better. In all reality, it may be best to just rewrite
    /// tests to cover the config ground-up, and there's enough to cover this functionality,
    /// deprecate/remove these.
    ///
    /// Documents (and validates) the expected behavior of configuration components within NuLog.
    /// </summary>
    [Trait("Category", "Integration")]
    [Trait("Domain", "Configuration")]
    public class ConfigurationTests
    {
        private const string DefaultConfigFile = "NuLog.json";

        private static readonly object _configFailLock = new object();
        private static bool _configFailed;

        private ConfigurationTestsListTarget ListTarget { get; set; }

        private bool ConfigFailed
        {
            get
            {
                lock (_configFailLock)
                {
                    return _configFailed;
                }
            }
            set
            {
                lock (_configFailLock)
                {
                    _configFailed = value;
                }
            }
        }

        public ConfigurationTests()
        {
            ListTarget = new ConfigurationTestsListTarget();
        }

        [Fact]
        public void InitializeTest()
        {
            var config = new LoggingConfig();
            config.LoadConfig();
        }

        [Fact]
        public void WatchTest()
        {
            using (var copy = new ConfigFileCopy(DefaultConfigFile))
            {
                var config = new LoggingConfig();
                config.LoadConfig(copy.FileName);

                Assert.True(config.Watch);

                SetWatch(copy.FileName, false);
                for (int lp = 0; lp < 50; lp++)
                {
                    if (!config.Watch)
                        break;
                    Thread.Sleep(100);
                }

                Assert.False(config.Watch);
            }
        }

        [Fact]
        public void MalformedJSONTest()
        {
            // Initialize the failure test
            ConfigFailed = false;
            var config = new LoggingConfig()
            {
                ExceptionHandler = HandleException
            };
            ICollection<string> sourceLines;

            using (var copy = new ConfigFileCopy(DefaultConfigFile))
            {
                // Load the file
                config.LoadConfig(copy.FileName);

                // Break the file
                sourceLines = MalformnJSON(copy.FileName);

                // Check to see if the failure was detected
                int checkCount = 0;
                while (!ConfigFailed)
                {
                    Thread.Sleep(200);
                    if (checkCount++ > 5)
                        break;
                }
                Assert.True(ConfigFailed);

                // Next we need to see if we can recover from the error

                // Restore the file
                WriteFile(copy.FileName, sourceLines);

                // Make sure that watch is on
                Assert.True(config.Watch);

                // Turn off watch
                SetWatch(copy.FileName, false);
                for (int lp = 0; lp < 10; lp++)
                {
                    if (!config.Watch)
                        break;
                    Thread.Sleep(100);
                }

                // Make sure watch is off
                Assert.False(config.Watch);
            }
        }

        [Fact]
        public void TestRuntimeConfiguration()
        {
            var config = LoggingConfigBuilder.CreateLoggingConfig()
                .AddTarget(new TargetConfig
                {
                    Name = "list",
                    Type = string.Format("{0}, NuLog.Tests", typeof(ConfigurationTestsListTarget).FullName)
                })
                .Build();

            using (var factory = new LoggerFactory(config))
            {
                var logger = factory.Logger();

                logger.LogNow("Hello, World!");

                var messages = ListTarget.GetList();
                Assert.NotNull(messages);
                Assert.True(messages.Count == 1);

                var logEvent = messages[0];
                Assert.NotNull(logEvent);
                Assert.Equal("Hello, World!", logEvent.Message);
                Assert.True(logEvent.Tags.Contains(this.GetType().FullName));

                logger.Log("Delayed, Test!");
                Thread.Yield();

                int tries = 0;
                while (tries++ < 10 && ListTarget.GetList().Count < 2)
                    Thread.Sleep(100);
            }

            var messagesDelayed = ListTarget.GetList();
            Assert.Equal(2, messagesDelayed.Count);
            var delayedMessage = messagesDelayed[1];
            Assert.NotNull(delayedMessage);
            Assert.Equal("Delayed, Test!", delayedMessage.Message);
        }

        #region helpers

        private void HandleException(Exception exception, string message)
        {
            if (message == LoggingConfig.ConfigLoadFailedMessage)
            {
                ConfigFailed = true;
            }
            else
            {
                throw exception;
            }
        }

        private static void SetWatch(string configFile, bool watch)
        {
            var jsonString = File.ReadAllText(configFile);
            var jsonConfig = JObject.Parse(jsonString);

            jsonConfig.Property("watch").Value = watch;

            File.WriteAllText(configFile, jsonConfig.ToString());
        }

        private static ICollection<string> MalformnJSON(string configFile)
        {
            // Read all lines into an array
            var lines = new List<string>(File.ReadAllLines(configFile));

            // Take a copy of the lines
            var copy = new List<string>();
            foreach (var line in lines)
                copy.Add(line);

            // Remove the empty lines, and the closing bracket at the end of the file
            while (lines[lines.Count - 1].Contains("}") == false)
                lines.RemoveAt(lines.Count - 1);
            lines.RemoveAt(lines.Count - 1);

            // Write the lines back to the file, effectively breaking the file
            File.WriteAllText(configFile, string.Join(Environment.NewLine, lines));

            // Return the malformed copy
            return copy;
        }

        private static void WriteFile(string configFile, ICollection<string> lines)
        {
            var contents = string.Join(Environment.NewLine, lines);
            File.WriteAllText(configFile, contents);
        }

        #endregion helpers
    }

    internal class ConfigFileCopy : IDisposable
    {
        public string FileName { get; }

        public ConfigFileCopy(string fileName)
        {
            FileName = Guid.NewGuid().ToString("N");
            File.Copy(fileName, FileName);
        }

        public void Dispose()
        {
            File.Delete(FileName);
        }
    }

    internal class ConfigurationTestsListTarget : TargetBase
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