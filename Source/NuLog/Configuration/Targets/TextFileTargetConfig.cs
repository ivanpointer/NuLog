/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/5/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using Newtonsoft.Json.Linq;
using NuLog.Targets;
using System;

namespace NuLog.Configuration.Targets
{
    /// <summary>
    /// An enum to define the different rollover policies for the text files
    /// </summary>
    public enum RolloverPolicy
    {
        /// <summary>
        /// No rolover policy
        /// </summary>
        None,
        /// <summary>
        /// Rollover based on size
        /// </summary>
        Size,
        /// <summary>
        /// Rollover daily
        /// </summary>
        Day
    }

    /// <summary>
    /// The configuration representing a text file target
    /// </summary>
    public class TextFileTargetConfig : LayoutTargetConfig
    {
        #region Constants
        public const string FileNameTokenName = "fileName";
        public const string OldFileNamePatternTokenName = "oldFileNamePattern";
        public const string RolloverPolicyTokenName = "rolloverPolicy";
        public const string RolloverTriggerTokenName = "rolloverTrigger";
        public const string OldFileLimitTokenName = "oldFileLimit";
        public const string CompressOldFilesTokenName = "compressOldFiles";
        public const string CompressionLevelTokenName = "compressionLevel";
        public const string CompressionPasswordTokenName = "compressionPassword";

        public const string GBSuffix = "GB";
        public const string MBSuffix = "MB";
        public const string KBSuffix = "KB";

        public const long GBMultiplier = 1024 ^ 3;
        public const long MBMultiplier = 1024 ^ 2;
        public const long KBMultiplier = 1024;
        public const long ZeroTrigger = 0L;

        public const string DefaultName = "file";
        public static readonly string DefaultType = typeof(TextFileTarget).FullName;
        public const string DefaultRolloverTriggerString = "1MB";
        public const long DefaultRolloverTrigger = 10 * MBMultiplier;
        public const int DefaultOldFileLimit = 10;
        public const string DefaultFileName = "log.log";
        public const string DefaultLoadFileNamePattern = "log{0:.yyyy.MM.dd.hh.mm.ss}.log";
        public const RolloverPolicy DefaultRolloverPolicy = RolloverPolicy.None;
        public const int DefaultCompressionLevel = 3;
        #endregion

        /// <summary>
        /// The path/name of the file to log to
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// The pattern to use for naming the rolled over log files
        /// </summary>
        public string OldFileNamePattern { get; set; }
        /// <summary>
        /// The rollover policy to use for the text file
        /// </summary>
        public RolloverPolicy RolloverPolicy { get; set; }
        /// <summary>
        /// The size of the file to trigger rollover in a "Size" rollover policy
        /// </summary>
        public long RolloverTrigger { get; set; }
        /// <summary>
        /// The maximum number of files to keep when rolling over
        /// </summary>
        public int OldFileLimit { get; set; }
        /// <summary>
        /// Whether or not to compress old log files when rolling over
        /// </summary>
        public bool CompressOldFiles { get; set; }
        /// <summary>
        /// The compression leve to use for compressing the files
        /// </summary>
        public int CompressionLevel { get; set; }
        /// <summary>
        /// An optional password to apply to the compressed log file
        /// </summary>
        public string CompressionPassword { get; set; }

        /// <summary>
        /// Builds a default text target config
        /// </summary>
        public TextFileTargetConfig()
            : base()
        {
            Name = DefaultName;
            Type = DefaultType;

            Defaults();
        }

        /// <summary>
        /// Builds a text file target config using a provided JSON token
        /// </summary>
        /// <param name="jToken">The JSON token to use to build the target config</param>
        public TextFileTargetConfig(JToken jToken)
            : base(jToken)
        {
            // Set the defaults for the config
            Defaults();

            // If a token was in-fact provided
            if (jToken != null)
            {
                // Setup the file name and old file name pattern
                FileName = GetValue<string>(jToken, FileNameTokenName, FileName);
                OldFileNamePattern = GetValue<string>(jToken, OldFileNamePatternTokenName, OldFileNamePattern);

                // Parse the rollover policy
                var rolloverPolicyToken = jToken[RolloverPolicyTokenName];
                if (rolloverPolicyToken != null)
                {
                    var rolloverPolicyName = rolloverPolicyToken.Value<string>();
                    RolloverPolicy = (RolloverPolicy)Enum.Parse(typeof(RolloverPolicy), rolloverPolicyName);
                }

                // If the policy is size
                if (RolloverPolicy == Targets.RolloverPolicy.Size)
                {
                    // Parse the trigger string
                    var triggerString = GetValue<string>(jToken, RolloverTriggerTokenName, DefaultRolloverTriggerString);
                    triggerString = triggerString.ToUpper();
                    long triggerLong = Convert.ToInt64(triggerString.Substring(0, triggerString.Length - 2));

                    if (triggerString.EndsWith(GBSuffix))
                    {
                        RolloverTrigger = triggerLong * GBMultiplier;
                    }
                    else if (triggerString.EndsWith(MBSuffix))
                    {
                        RolloverTrigger = triggerLong * MBMultiplier;
                    }
                    else if (triggerString.EndsWith(KBSuffix))
                    {
                        RolloverTrigger = triggerLong * KBMultiplier;
                    }
                    else
                    {
                        RolloverTrigger = triggerLong;
                    }
                }
                else
                {
                    // Default to no rollover
                    RolloverTrigger = ZeroTrigger;
                }

                // Parse the other settings
                OldFileLimit = GetValue<int>(jToken, OldFileLimitTokenName, OldFileLimit);
                CompressOldFiles = GetValue<bool>(jToken, CompressOldFilesTokenName, CompressOldFiles);
                CompressionLevel = GetValue<int>(jToken, CompressionLevelTokenName, CompressionLevel);
                CompressionPassword = GetValue<string>(jToken, CompressionPasswordTokenName, CompressionPassword);
            }
        }

        /// <summary>
        /// Sets the defaults for this configuration
        /// </summary>
        private void Defaults()
        {
            FileName = DefaultFileName;
            OldFileNamePattern = DefaultLoadFileNamePattern;
            RolloverPolicy = DefaultRolloverPolicy;
            RolloverTrigger = DefaultRolloverTrigger;
            OldFileLimit = DefaultOldFileLimit;
            CompressOldFiles = false;
            CompressionLevel = DefaultCompressionLevel;
            CompressionPassword = null;
        }

        #region Helpers
        /// <summary>
        /// Returns a ConsoleColor based on the given string representation of the color
        /// </summary>
        private T GetValue<T>(JToken jToken, string name, T defVal)
        {
            var child = jToken[name];
            if (child != null)
                return child.Value<T>();
            return defVal;
        }

        #endregion
    }
}
