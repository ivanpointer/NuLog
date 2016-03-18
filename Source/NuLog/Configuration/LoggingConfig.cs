/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/6/2014
 * Updated 11/20/2014: Ivan Pointer: Added extenders to the configuration
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Newtonsoft.Json.Linq;
using NuLog.Configuration.Extenders;
using NuLog.Configuration.Targets;

namespace NuLog.Configuration
{
	/// <summary>
	/// Represents the whole configuration for the logging framework
	/// </summary>
	public class LoggingConfig
	{
		#region Constants

		public const string ConfigLoadFailedMessage = "Exception while loading config";
		private const string ConfigurationFileNotFoundMessage = "Configuration file \"{0}\" for NuLog not found";

		private const string DefaultConfigurationFile = "NuLog.json";
		private const string ConfigurationAppSetting = "NuLog.File";

		private const string TargetsTokenName = "targets";
		private const string RulesTokenName = "rules";
		private const string TagGroupsTokenName = "tagGroups";
		private const string ConfigurationExtendersTokenName = "configurationExtenders";
		private const string StaticMetaDataProvidersTokenName = "staticMetaDataProviders";
		private const string ExtendersTokenName = "extenders";
		private const string WatchTokenName = "watch";
		private const string DebugTokenName = "debug";
		private const string SynchronousTokenName = "synchronous";

		private const string TargetNameTokenName = "name";
		private const string TargetTypeTokenName = "type";

		private const string RuleIncludeTokenName = "include";
		private const string RuleStrictIncludeTokenName = "strictInclude";
		private const string RuleExcludeTokenName = "exclude";
		private const string RuleWriteToTokenName = "writeTo";
		private const string RuleFinalTokenName = "final";

		#endregion Constants

		private static readonly object _configLock = new object();
		public string ConfigFile { get; private set; }

		// The file watcher for some reason fires two system events right on top of each other
		//   when a file is changed.  We adjust for this by having a minimum
		//    amount of time between reloads
		private DateTime LastChange { get; set; }

		private const long ChangeLimitInMilliseconds = 1000;
		private string WatchFile { get; set; }
		private FileSystemWatcher FileSystemWatcher { get; set; }

		/// <summary>
		/// The list of target configurations defining the targets to be used
		/// </summary>
		public IList<TargetConfig> Targets { get; set; }

		/// <summary>
		/// The list of rules defining how log events are to be dispatched to the defined targets
		/// </summary>
		public IList<RuleConfig> Rules { get; set; }

		/// <summary>
		/// The list of tag groups used to group tags together under a single tag
		/// </summary>
		public IList<TagGroupConfig> TagGroups { get; set; }

		/// <summary>
		/// A list of configuration extenders for the framework
		/// </summary>
		public IList<string> ConfigurationExtenders { get; set; }

		/// <summary>
		/// A list of static meta data providers for the framework
		/// </summary>
		public IList<string> StaticMetaDataProviders { get; set; }

		/// <summary>
		/// A list of extenders for extending the framework.
		/// Extenders are executed after the dispatcher is initialized.
		/// </summary>
		public IList<ExtenderConfig> Extenders { get; set; }

		/// <summary>
		/// A flag indicating whether the framework should be running in "synchronous" mode or not.  If the synchronous
		/// flag is set, no background or worker threads will be used to log.  Control will not return to the logging
		/// application until the log event has been logged to each of the configured targets.
		/// </summary>
		public bool Synchronous { get; set; }

		/// <summary>
		/// A flag indicating whether or not debug information is to be included in the log events.  This is specifically
		/// the stack frame information of the logging method.
		/// </summary>
		public bool Debug { get; set; }

		/// <summary>
		/// A flag indicating whether or not the file from which the configuration was loaded is to be watched for changes or not.
		/// This property only applies to configurations loaded from file and not runtime configurations.
		/// </summary>
		public bool Watch { get; set; }

		// We use an observer pattern here to keep observers informed of configuration changes
		private static readonly object _observerLock = new object();

		private ICollection<IConfigObserver> ConfigObservers { get; set; }

		/// <summary>
		/// An optional exception handler can be assigned for debugging purposes.  If assigned, exceptions that occur
		/// inside of the logging framework will be sent to this exception handler.  Otherwise, exceptions will be
		/// logged to Trace (and will not bubble up through the framework into the logging framework)
		/// </summary>
		public Action<Exception, string> ExceptionHandler { get; set; }

		/// <summary>
		/// Builds the logging configuration using the values provided.
		/// </summary>
		/// <param name="configFile">The configuration file from which to load the configuration from.
		/// If no value is provided, the system looks for "NuLog.json", and if found, uses it
		/// to build this configuration.  Otherwise, a configuration built in which a trace target
		/// is created and all log events are sent to trace.</param>
		/// <param name="loadConfig">Whether or not to load the configuration</param>
		public LoggingConfig(string configFile = null, bool loadConfig = true)
		{
			Targets = new List<TargetConfig>();
			Rules = new List<RuleConfig>();
			TagGroups = new List<TagGroupConfig>();
			ConfigurationExtenders = new List<string>();
			StaticMetaDataProviders = new List<string>();
			Extenders = new List<ExtenderConfig>();

			LastChange = DateTime.MinValue;
			ConfigObservers = new List<IConfigObserver>();

			if (loadConfig)
				LoadConfig(configFile);
		}

		/// <summary>
		/// Registers an observer of this configuration (Using the observer pattern)
		/// </summary>
		/// <param name="observer">The observer to register to this configuration</param>
		public void RegisterObserver(IConfigObserver observer)
		{
			lock (_observerLock)
				if (ConfigObservers.Contains(observer) == false)
					ConfigObservers.Add(observer);
		}

		/// <summary>
		/// Unregisters an observer from this configuration (Using the observer pattern)
		/// </summary>
		/// <param name="observer">The observer to unregister from this configuration</param>
		public void UnregisterObserver(IConfigObserver observer)
		{
			lock (_observerLock)
				if (ConfigObservers.Contains(observer))
					ConfigObservers.Remove(observer);
		}

		/// <summary>
		/// Shuts down this configuration.  This includes clearing any observers and shutting down the file watcher.
		/// </summary>
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

		/// <summary>
		/// Notifies the observers of this configuration of a configuration change
		/// </summary>
		public void NotifyObservers()
		{
			LoggingConfig readOnlyCopy = ReadOnlyCopy();
			lock (_observerLock)
				foreach (IConfigObserver observer in ConfigObservers)
					observer.NotifyNewConfig(readOnlyCopy);
		}

		/// <summary>
		/// Loads the configuration from the given file
		/// </summary>
		/// <param name="configFile">The configuration file from which to load the configuration</param>
		public void LoadConfig(string configFile = null)
		{
			try
			{
				// Establish the correct path of the configuration file to be loaded
				configFile = GetConfigFile(configFile);
				ConfigFile = configFile;

				// Make sure that the path is rooted
				//  In the case of ASP MVC, the default behavior
				//    would be to look in the IIS folder for
				//    this configuration file, which is not correct.
				//    We will instead look in the directory of the NuLogging
				//    DLL
				if (File.Exists(configFile) == false && Path.IsPathRooted(configFile) == false)
				{
					var configFileName = Path.GetFileName(configFile);
					var codeBase = AppDomain.CurrentDomain.BaseDirectory;
					var uriBuilder = new UriBuilder(codeBase);
					var path = Uri.UnescapeDataString(uriBuilder.Path);
					configFile = Path.Combine(Path.GetDirectoryName(path), configFileName);
				}

				// Make sure that the configuration file from which we are to load the configuration
				//  exists
				if (File.Exists(configFile))
				{
					// Pull the file into a JSON object
					string jsonString = ReadFileText(configFile);
					JObject jsonConfig = JObject.Parse(jsonString);

					lock (_configLock)
					{
						// Load the targets
						var targetsJson = jsonConfig[TargetsTokenName].Children().ToList();
						Targets = LoadTargetConfigs(targetsJson);

						// Load the rules
						var rulesJson = jsonConfig[RulesTokenName].Children().ToList();
						Rules = LoadRuleConfigs(rulesJson);

						// Load the tag groups
						var tagGroupsJson = jsonConfig[TagGroupsTokenName];
						TagGroups = tagGroupsJson != null
							? LoadTagGroupConfigs(tagGroupsJson)
							: new List<TagGroupConfig>();

						// Configuration Extenders
						var configExtendersJson = jsonConfig[ConfigurationExtendersTokenName];
						ConfigurationExtenders = LoadConfigurationExtenders(configExtendersJson);

						// Static Meta Data Providers
						var staticMetaDataProvidersJson = jsonConfig[StaticMetaDataProvidersTokenName];
						StaticMetaDataProviders = LoadStaticMetaDataProviders(staticMetaDataProvidersJson);

						// Extenders
						var extendersJson = jsonConfig[ExtendersTokenName];
						Extenders = LoadExtenders(extendersJson);

						// Synchronous flag
						Synchronous = false;
						var synchronousJson = jsonConfig[SynchronousTokenName];
						if (synchronousJson != null)
							Synchronous = synchronousJson.Value<bool>();

						// Watch flag
						Watch = false;
						var watchJson = jsonConfig[WatchTokenName];
						if (watchJson != null)
							Watch = watchJson.Value<bool>();

						if (Watch)
							InitializeFileWatcher(configFile);
						else
							ShutdownFileWatcher();

						// Debug flag
						Debug = false;
						var debugJson = jsonConfig[DebugTokenName];
						if (debugJson != null)
							Debug = debugJson.Value<bool>();
					}

					// Notify the observers of the new configuration
					NotifyObservers();
				}
				else
				{
					// The file we are supposed to load from doesn't exist
					throw new LoggingException(String.Format(ConfigurationFileNotFoundMessage, Path.GetFullPath(configFile)));
				}
			}
			catch (Exception e)
			{
				if (ExceptionHandler == null)
					throw;
				ExceptionHandler.Invoke(e, LoggingConfig.ConfigLoadFailedMessage);
			}
		}

		/// <summary>
		/// The event handler for when the file changes
		/// </summary>
		/// <param name="source">The source of the event</param>
		/// <param name="e">Details of the event</param>
		private void ConfigurationChanged(object source, FileSystemEventArgs e)
		{
			// The file watcher for some reason fires two system events right on top of each other
			//   we prevent this by having a minimum amount of time before reloads

			// Record the change
			var myLastChange = DateTime.Now;
			LastChange = myLastChange;

			lock (_configLock)
			{
				// Wait until at least 100 milliseconds have passed since the last change
				var futureLimit = myLastChange.AddMilliseconds(ChangeLimitInMilliseconds);
				while (LastChange == myLastChange && futureLimit > DateTime.Now)
				{
					Thread.Yield();
					Thread.Sleep(100);
				}

				// If this instance of the event is the last one that changed the file
				if (LastChange == myLastChange)
				{
					try
					{
						// Reload the configuration
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

		/// <summary>
		/// Loads the tag groups from the configuration JSON token provided
		/// </summary>
		/// <param name="tagGroupsJson">The JSON token containing the tag groups configuration</param>
		/// <returns>A list of the tag groups configurations</returns>
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

		/// <summary>
		/// Loads the configuration extenders from the configuration JSON token provided
		/// </summary>
		/// <param name="configExtendersJson">The JSON token containing the configuration extenders</param>
		/// <returns>A list of names of configuration extenders</returns>
		private static IList<string> LoadConfigurationExtenders(JToken configExtendersJson)
		{
			return configExtendersJson != null && configExtendersJson.Type == JTokenType.Array
				? configExtendersJson.Values<string>().ToList<string>()
				: new List<string>();
		}

		/// <summary>
		/// Loads the static meta data providers from the configuration JSON token provided
		/// </summary>
		/// <param name="staticMetaDataProvidersJson">The JSON token containing the static meta data providers</param>
		/// <returns>A list of names of static meta data providers</returns>
		private static IList<string> LoadStaticMetaDataProviders(JToken staticMetaDataProvidersJson)
		{
			return staticMetaDataProvidersJson != null && staticMetaDataProvidersJson.Type == JTokenType.Array
				? staticMetaDataProvidersJson.Values<string>().ToList<string>()
				: new List<string>();
		}

		/// <summary>
		/// Loads the extenders from the JSON token provided
		/// </summary>
		/// <param name="extendersJson">The JSON token from which to load the extenders</param>
		/// <returns></returns>
		private static IList<ExtenderConfig> LoadExtenders(JToken extendersJson)
		{
			var extenders = new List<ExtenderConfig>();

			// Make sure we have something to work with
			if (extendersJson != null)
			{
				// If it is an array, iterate over it and create a new
				//  extender config for each entry
				if (extendersJson.Type == JTokenType.Array)
				{
					foreach (var extenderJson in extendersJson.Values())
					{
						extenders.Add(new ExtenderConfig(extenderJson));
					}
				}
				// If it is a single item, instantiate that
				else
				{
					extenders.Add(new ExtenderConfig(extendersJson));
				}
			}

			// Return what we got, if anything
			return extenders;
		}

		/// <summary>
		/// Loads the targets from the configuration JSON tokens provided
		/// </summary>
		/// <param name="targetsJson">The JSON tokens from which to load the target configurations</param>
		/// <returns></returns>
		private static IList<TargetConfig> LoadTargetConfigs(ICollection<JToken> targetsJson)
		{
			var targets = new List<TargetConfig>();

			foreach (var targetJson in targetsJson)
			{
				targets.Add(new TargetConfig
				{
					Name = targetJson[TargetNameTokenName].Value<string>(),
					Type = targetJson[TargetTypeTokenName].Value<string>(),
					Config = targetJson
				});
			}

			return targets;
		}

		/// <summary>
		/// Loads the rules from the configuration JSON tokens provided
		/// </summary>
		/// <param name="rulesJson">The JSON tokens from which to load the rules</param>
		/// <returns></returns>
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

				include = ruleJson[RuleIncludeTokenName];
				if (include != null)
					newRule.Include = include.Values<string>().ToList();

				strictInclude = ruleJson[RuleStrictIncludeTokenName];
				newRule.StrictInclude = strictInclude != null && strictInclude.Value<bool>();

				exclude = ruleJson[RuleExcludeTokenName];
				if (exclude != null)
					newRule.Exclude = exclude.Values<string>().ToList();

				writeTo = ruleJson[RuleWriteToTokenName];
				if (writeTo != null)
					newRule.WriteTo = writeTo.Values<string>().ToList();

				final = ruleJson[RuleFinalTokenName];
				newRule.Final = final != null && final.Value<bool>();

				rules.Add(newRule);
			}

			return rules;
		}

		#endregion Tag Group Config

		/// <summary>
		/// Reads the text from the file
		/// </summary>
		/// <param name="configFile">The file from which to read the text</param>
		/// <returns>The text contents of the configuration file</returns>
		private static string ReadFileText(string configFile)
		{
			using (var fileStream = File.Open(configFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			using (var streamReader = new StreamReader(fileStream))
				return streamReader.ReadToEnd();
		}

		/// <summary>
		/// Determines the correct configuration file name from the given configuration file name
		/// </summary>
		/// <param name="configFile">The original configuration file name to check</param>
		/// <returns>The corrected configuration file name from which to load the configuration from</returns>
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

		/// <summary>
		/// Creates a read-only copy of this configuration
		/// </summary>
		/// <returns>A read-only copy of this configuration</returns>
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

				this.ConfigurationExtenders = this.ConfigurationExtenders == null
					? new List<string>()
					: this.ConfigurationExtenders;

				this.StaticMetaDataProviders = this.StaticMetaDataProviders == null
					? new List<string>()
					: this.StaticMetaDataProviders;

				this.Extenders = this.Extenders == null
					? new List<ExtenderConfig>()
					: this.Extenders;

				return new LoggingConfig(loadConfig: false)
				{
					ConfigFile = this.ConfigFile,
					Watch = this.Watch,
					Targets = new ReadOnlyCollection<TargetConfig>(this.Targets),
					Rules = new ReadOnlyCollection<RuleConfig>(this.Rules),
					TagGroups = new ReadOnlyCollection<TagGroupConfig>(this.TagGroups),
					ConfigurationExtenders = new ReadOnlyCollection<string>(this.ConfigurationExtenders),
					StaticMetaDataProviders = new ReadOnlyCollection<string>(this.StaticMetaDataProviders),
					Extenders = new ReadOnlyCollection<ExtenderConfig>(this.Extenders)
				};
			}
		}

		/// <summary>
		/// Starts a file watcher for the given file
		/// </summary>
		/// <param name="watchFile">The file to watch</param>
		private void InitializeFileWatcher(string watchFile)
		{
			lock (_configLock)
			{
				// If the file is not yet being watched
				if (String.IsNullOrEmpty(WatchFile) || watchFile != WatchFile)
				{
					// Shut down any existing watchers, we only want to watch one at a time
					ShutdownFileWatcher();

					// Setup the new file watcher
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

		/// <summary>
		/// Shuts down any running file system watchers
		/// </summary>
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

		#endregion Helpers
	}
}