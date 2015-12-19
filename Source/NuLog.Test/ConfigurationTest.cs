using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using NuLog.Configuration;
using NuLog.Configuration.Targets;
using NuLog.Test.TestHarness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace NuLog.Test
{
    [TestClass]
    public class ConfigurationTest
    {
        private const string DefaultConfigFile = "NuLog.json";

        private static readonly object _configFailLock = new object();
        private static bool _configFailed;

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

        [TestInitialize]
        public void TestInitialize()
        {
            ListTarget.ClearList();
        }

        [TestMethod]
        public void InitializeTest()
        {
            var config = new LoggingConfig();
            config.LoadConfig();
        }

        [TestMethod]
        public void WatchTest()
        {
            using (var copy = new ConfigFileCopy(DefaultConfigFile))
            {
                var config = new LoggingConfig();
                config.LoadConfig(copy.FileName);

                Assert.IsTrue(config.Watch);

                SetWatch(copy.FileName, false);
                for (int lp = 0; lp < 50; lp++)
                {
                    if (!config.Watch)
                        break;
                    Thread.Sleep(100);
                }

                Assert.IsFalse(config.Watch);
            }
        }

        [TestMethod]
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
                Assert.IsTrue(ConfigFailed);

                // Next we need to see if we can recover from the error

                // Restore the file
                WriteFile(copy.FileName, sourceLines);

                // Make sure that watch is on
                Assert.IsTrue(config.Watch);

                // Turn off watch
                SetWatch(copy.FileName, false);
                for (int lp = 0; lp < 10; lp++)
                {
                    if (!config.Watch)
                        break;
                    Thread.Sleep(100);
                }

                // Make sure watch is off
                Assert.IsFalse(config.Watch);
            }
        }

        [TestMethod]
        public void TestRuntimeConfiguration()
        {
            var config = LoggingConfigBuilder.CreateLoggingConfig()
                .AddTarget(new TargetConfig
                {
                    Name = "list",
                    Type = String.Format("{0}, NuLog.Test", typeof(ListTarget).FullName)
                })
                .Build();

            LoggerFactory.Initialize(config);

            var logger = LoggerFactory.GetLogger();

            logger.LogNow("Hello, World!");

            var messages = ListTarget.GetList();
            Assert.IsNotNull(messages);
            Assert.IsTrue(messages.Count == 1);

            var logEvent = messages[0];
            Assert.IsNotNull(logEvent);
            Assert.AreEqual("Hello, World!", logEvent.Message);
            Assert.IsTrue(logEvent.Tags.Contains(this.GetType().FullName));

            logger.Log("Delayed, Test!");
            Thread.Yield();

            int tries = 0;
            while (tries++ < 10 && ListTarget.GetList().Count < 2)
                Thread.Sleep(100);

            var messagesDelayed = ListTarget.GetList();
            Assert.IsTrue(messagesDelayed.Count == 2);
            var delayedMessage = messagesDelayed[1];
            Assert.IsNotNull(delayedMessage);
            Assert.AreEqual("Delayed, Test!", delayedMessage.Message);
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
            string jsonString;

            using (var fileStream = File.Open(configFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var streamReader = new StreamReader(fileStream))
                jsonString = streamReader.ReadToEnd();

            JObject jsonConfig = JObject.Parse(jsonString);

            jsonConfig.Property("watch").Value = watch;

            using (var fileStream = File.Open(configFile, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
            using (var streamWriter = new StreamWriter(fileStream))
                streamWriter.Write(jsonConfig.ToString());
        }

        private static ICollection<string> MalformnJSON(string configFile)
        {
            var lines = new List<string>();

            // Read all lines into a list
            using (var fileStream = File.Open(configFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var streamReader = new StreamReader(fileStream))
                while (streamReader.EndOfStream == false)
                    lines.Add(streamReader.ReadLine());

            // Take a copy of the lines
            var copy = new List<string>();
            foreach (var line in lines)
                copy.Add(line);

            // Remove the last lines up to (empty lines) and including the "last" line (the line containing the closing bracket)
            while (lines[lines.Count - 1].Contains("}") == false)
                lines.RemoveAt(lines.Count - 1);
            lines.RemoveAt(lines.Count - 1);

            // Write the lines back to the file, effectively malforming the file
            using (var fileStream = File.Open(configFile, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
            using (var streamWriter = new StreamWriter(fileStream))
                foreach (var line in lines)
                    streamWriter.WriteLine(line);

            return copy;
        }

        private static void WriteFile(string configFile, ICollection<string> lines)
        {
            using (var fileStream = File.Open(configFile, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
            using (var streamWriter = new StreamWriter(fileStream))
                foreach (var line in lines)
                    streamWriter.WriteLine(line);
        }

        #endregion helpers
    }

    internal class ConfigFileCopy : IDisposable
    {
        private string _fileName;

        public string FileName
        {
            get
            {
                return _fileName;
            }
        }

        public ConfigFileCopy(string fileName)
        {
            _fileName = Guid.NewGuid().ToString("N");
            File.Copy(fileName, _fileName);
        }

        public void Dispose()
        {
            File.Delete(_fileName);
        }
    }
}