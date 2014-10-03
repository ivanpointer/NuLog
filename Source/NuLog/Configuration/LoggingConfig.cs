using Newtonsoft.Json.Linq;
using NuLog.Configuration.Targets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NuLog.Configuration
{
    public class LoggingConfig
    {
        public const string ConfigLoadFailedMessage = "Exception while loading config";

        private const string DefaultConfigurationFile = "NuLog.json";
        private const string ConfigurationAppSetting = "NuLog.File";

        private static readonly object _configLock = new object();

        // The file watcher for some reason fires two system events right on top of eachother
        //   we prevent this by having a minimum amount of time before reloads
        private DateTime LastChange { get; set; }
        private const long ChangeLimitInMilliseconds = 1000;
        private string WatchFile { get; set; }
        private FileSystemWatcher FileSystemWatcher { get; set; }

        private string _configFile;
        public string ConfigFile
        {
            get
            {
                return _configFile;
            }
        }

        public bool Debug { get; set; }
        public bool Watch { get; set; }

        public IList<TargetConfig> Targets { get; set; }

        public IList<RuleConfig> Rules { get; set; }

        public IList<TagGroupConfig> TagGroups { get; set; }

        private static readonly object _observerLock = new object();
        private ICollection<IConfigObserver> ConfigObservers { get; set; }

        public Action<Exception, string> ExceptionHandler { get; set; }

        public LoggingConfig(string configFile = null, bool loadConfig = true)
        {
            Targets = new List<TargetConfig>();
            Rules = new List<RuleConfig>();
            TagGroups = new List<TagGroupConfig>();

            LastChange = DateTime.MinValue;
            ConfigObservers = new List<IConfigObserver>();

            if (loadConfig)
                LoadConfig(configFile);
        }

        public void RegisterObserver(IConfigObserver observer)
        {
            lock (_observerLock)
                if (ConfigObservers.Contains(observer) == false)
                    ConfigObservers.Add(observer);
        }

        public void UnregisterObserver(IConfigObserver observer)
        {
            lock (_observerLock)
                if (ConfigObservers.Contains(observer))
                    ConfigObservers.Remove(observer);
        }

        public void Shutdown()
        {
            lock (_observerLock)
            {
                lock (_configLock)
                {
                    ConfigObservers.Clear();

                    ShutdownFileWatcher();
                }
            }
        }

        public void NotifyObservers()
        {
            LoggingConfig readOnlyCopy = ReadOnlyCopy();
            lock (_observerLock)
                foreach (IConfigObserver observer in ConfigObservers)
                    observer.NotifyNewConfig(readOnlyCopy);
        }

        public void LoadConfig(string configFile = null)
        {
            try
            {
                configFile = GetConfigFile(configFile);
                _configFile = configFile;

                // Make sure that the path is rooted
                //  In the case of ASP MVC, the default behavior
                //    would be to look in the IIS folder for
                //    this configuration file, which is not correct.
                //    We will instead look in the directory of the NuLogging
                //    DLL
                if (File.Exists(configFile) == false && Path.IsPathRooted(configFile) == false)
                {
                    var configFileName = Path.GetFileName(configFile);
                    var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                    var uriBuilder = new UriBuilder(codeBase);
                    var path = Uri.UnescapeDataString(uriBuilder.Path);
                    configFile = Path.Combine(Path.GetDirectoryName(path), configFileName);
                }

                if (File.Exists(configFile))
                {
                    string jsonString = ReadFileText(configFile);
                    JObject jsonConfig = JObject.Parse(jsonString);

                    lock (_configLock)
                    {
                        var targetsJson = jsonConfig["targets"].Children().ToList();
                        Targets = LoadTargetConfigs(targetsJson);

                        var rulesJson = jsonConfig["rules"].Children().ToList();
                        Rules = LoadRuleConfigs(rulesJson);

                        var tagGroupsJson = jsonConfig["tagGroups"];
                        TagGroups = tagGroupsJson != null
                            ? LoadTagGroupConfigs(tagGroupsJson)
                            : new List<TagGroupConfig>();

                        Watch = false;

                        var watchJson = jsonConfig["watch"];
                        if (watchJson != null)
                            Watch = watchJson.Value<bool>();

                        if (Watch)
                            InitializeFileWatcher(configFile);
                        else
                            ShutdownFileWatcher();

                        Debug = false;
                        var debugJson = jsonConfig["debug"];
                        if (debugJson != null)
                            Debug = debugJson.Value<bool>();
                    }

                    NotifyObservers();
                }
                else
                {
                    throw new LoggingException(String.Format("Configuration file \"{0}\" for NuLog not found", Path.GetFullPath(configFile)));
                }
            }
            catch (Exception e)
            {
                if (ExceptionHandler == null)
                    throw;
                ExceptionHandler.Invoke(e, LoggingConfig.ConfigLoadFailedMessage);
            }
        }

        private void ConfigurationChanged(object source, FileSystemEventArgs e)
        {
            // The file watcher for some reason fires two system events right on top of eachother
            //   we prevent this by having a minimum amount of time before reloads

            var myLastChange = DateTime.Now;
            LastChange = myLastChange;

            lock (_configLock)
            {
                var futureLimit = myLastChange.AddMilliseconds(ChangeLimitInMilliseconds);
                while (LastChange == myLastChange && futureLimit > DateTime.Now)
                {
                    Thread.Yield();
                    Thread.Sleep(100);
                }

                if (LastChange == myLastChange)
                {
                    try
                    {
                        LoadConfig(e.FullPath);
                    }
                    catch (Exception ex)
                    {
                        if (ExceptionHandler == null)
                            throw;

                        ExceptionHandler.Invoke(ex, LoggingConfig.ConfigLoadFailedMessage);
                    }
                }
            }
        }

        #region Helpers

        #region Tag Group Config

        private static IList<TagGroupConfig> LoadTagGroupConfigs(JToken tagGroupsJson)
        {
            var tagGroups = new List<TagGroupConfig>();

            var parsed = tagGroupsJson.ToObject<Dictionary<string, List<string>>>();

            foreach (string key in parsed.Keys)
            {
                tagGroups.Add(new TagGroupConfig
                {
                    Tag = key,
                    ChildTags = parsed[key]
                });
            }

            return tagGroups;
        }

        private static IList<TargetConfig> LoadTargetConfigs(ICollection<JToken> targetsJson)
        {
            var targets = new List<TargetConfig>();

            foreach (var targetJson in targetsJson)
            {
                targets.Add(new TargetConfig
                {
                    Name = targetJson["name"].Value<string>(),
                    Type = targetJson["type"].Value<string>(),
                    Config = targetJson
                });
            }

            return targets;
        }

        private static IList<RuleConfig> LoadRuleConfigs(ICollection<JToken> rulesJson)
        {
            var rules = new List<RuleConfig>();

            RuleConfig newRule;
            JToken include;
            JToken strictInclude;
            JToken exclude;
            JToken writeTo;
            JToken final;
            foreach (var ruleJson in rulesJson)
            {
                newRule = new RuleConfig();

                include = ruleJson["include"];
                if (include != null)
                    newRule.Include = include.Values<string>().ToList();

                strictInclude = ruleJson["strictInclude"];
                newRule.StrictInclude = strictInclude != null && strictInclude.Value<bool>();

                exclude = ruleJson["exclude"];
                if (exclude != null)
                    newRule.Exclude = exclude.Values<string>().ToList();

                writeTo = ruleJson["writeTo"];
                if (writeTo != null)
                    newRule.WriteTo = writeTo.Values<string>().ToList();

                final = ruleJson["final"];
                newRule.Final = final != null && final.Value<bool>();

                rules.Add(newRule);
            }

            return rules;
        }

        #endregion

        private static string ReadFileText(string configFile)
        {
            using (var fileStream = File.Open(configFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var streamReader = new StreamReader(fileStream))
                return streamReader.ReadToEnd();
        }

        private static string GetConfigFile(string configFile = null)
        {
            configFile =
                String.IsNullOrEmpty(configFile)
                    ? ConfigurationManager.AppSettings.Get(ConfigurationAppSetting)
                    : configFile;

            configFile =
                String.IsNullOrEmpty(configFile)
                    ? DefaultConfigurationFile
                    : configFile;

            return configFile;
        }

        private LoggingConfig ReadOnlyCopy()
        {
            lock (_configLock)
            {
                this.Targets = this.Targets == null
                    ? new List<TargetConfig>()
                    : this.Targets;

                this.Rules = this.Rules == null
                    ? new List<RuleConfig>()
                    : this.Rules;

                this.TagGroups = this.TagGroups == null
                    ? new List<TagGroupConfig>()
                    : this.TagGroups;

                return new LoggingConfig(loadConfig: false)
                {
                    _configFile = this._configFile,
                    Watch = this.Watch,
                    Targets = new ReadOnlyCollection<TargetConfig>(this.Targets),
                    Rules = new ReadOnlyCollection<RuleConfig>(this.Rules),
                    TagGroups = new ReadOnlyCollection<TagGroupConfig>(this.TagGroups)
                };
            }
        }

        private void InitializeFileWatcher(string watchFile)
        {
            lock (_configLock)
            {
                if (String.IsNullOrEmpty(WatchFile) || watchFile != WatchFile)
                {
                    ShutdownFileWatcher();

                    WatchFile = watchFile;
                    FileSystemWatcher = new FileSystemWatcher
                    {
                        Path = Path.GetDirectoryName(Path.GetFullPath(watchFile)),
                        NotifyFilter = NotifyFilters.LastWrite,
                        Filter = Path.GetFileName(watchFile)
                    };

                    FileSystemWatcher.Changed += new FileSystemEventHandler(ConfigurationChanged);

                    FileSystemWatcher.EnableRaisingEvents = true;
                }
            }
        }

        private void ShutdownFileWatcher()
        {
            lock (_configLock)
            {
                if (FileSystemWatcher != null)
                {
                    WatchFile = null;
                    FileSystemWatcher.EnableRaisingEvents = false;
                    FileSystemWatcher.Changed -= new FileSystemEventHandler(ConfigurationChanged);
                    FileSystemWatcher.Dispose();
                    FileSystemWatcher = null;
                }
            }
        }

        #endregion

    }
}
