/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/07/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Configuration.Layouts;
using System.IO.Compression;
namespace NuLog.Configuration.Targets
{
    /// <summary>
    /// Used to build a text file config at runtime
    /// </summary>
    public class TextFileTargetConfigBuilder
    {
        /// <summary>
        /// The config being built
        /// </summary>
        protected TextFileTargetConfig Config { get; set; }

        /// <summary>
        /// The private instantiation
        /// </summary>
        private TextFileTargetConfigBuilder()
        {
            Config = new TextFileTargetConfig();
        }

        /// <summary>
        /// Creates a new instance of this builder
        /// </summary>
        /// <returns>A new instance of this builder</returns>
        public static TextFileTargetConfigBuilder Create()
        {
            return new TextFileTargetConfigBuilder();
        }

        /// <summary>
        /// Sets the layout config to a standard layout with the given layout format
        /// </summary>
        /// <param name="layoutFormat">The layout format to use for the new layout config</param>
        /// <returns>This TextFileTargetConfigBuilder</returns>
        public TextFileTargetConfigBuilder SetLayoutConfig(string layoutFormat)
        {
            Config.LayoutConfig = new LayoutConfig(layoutFormat);

            return this;
        }

        /// <summary>
        /// Sets the layout config to the provided layout config
        /// </summary>
        /// <param name="layoutConfig">The layout config to use</param>
        /// <returns>This TextFileTargetConfigBuilder</returns>
        public TextFileTargetConfigBuilder SetLayoutConfig(LayoutConfig layoutConfig)
        {
            Config.LayoutConfig = layoutConfig;

            return this;
        }

        /// <summary>
        /// Sets the file name of the text log file
        /// </summary>
        /// <param name="fileName">The file name of the text log file</param>
        /// <returns>This TextFileTargetConfigBuilder</returns>
        public TextFileTargetConfigBuilder SetFileName(string fileName)
        {
            Config.FileName = fileName;
            return this;
        }

        /// <summary>
        /// Sets the old file name pattern to use when rotating log files
        /// </summary>
        /// <param name="oldFileNamePattern">The old file name pattern to use when rotating log files</param>
        /// <returns>This TextFileTargetConfigBuilder</returns>
        public TextFileTargetConfigBuilder SetOldFileNamePattern(string oldFileNamePattern)
        {
            Config.OldFileNamePattern = oldFileNamePattern;
            return this;
        }

        /// <summary>
        /// Sets the rollover policy
        /// </summary>
        /// <param name="rolloverPolicy">The rollover policy to set</param>
        /// <returns>This TextFileTargetConfigBuilder</returns>
        public TextFileTargetConfigBuilder SetRolloverPolicy(RolloverPolicy rolloverPolicy)
        {
            Config.RolloverPolicy = rolloverPolicy;
            return this;
        }

        /// <summary>
        /// Sets the rollover policy and trigger
        /// </summary>
        /// <param name="rolloverPolicy">The rollover policy to set</param>
        /// <param name="rolloverTrigger">The rollover trigger to set</param>
        /// <returns>This TextFileTargetConfigBuilder</returns>
        public TextFileTargetConfigBuilder SetRolloverPolicy(RolloverPolicy rolloverPolicy, long rolloverTrigger)
        {
            Config.RolloverPolicy = rolloverPolicy;
            Config.RolloverTrigger = rolloverTrigger;
            return this;
        }

        /// <summary>
        /// Sets the rollover trigger
        /// </summary>
        /// <param name="rolloverTrigger">The rollover trigger</param>
        /// <returns>This TextFileTargetConfigBuilder</returns>
        public TextFileTargetConfigBuilder SetRolloverTrigger(long rolloverTrigger)
        {
            Config.RolloverTrigger = rolloverTrigger;
            return this;
        }

        /// <summary>
        /// Sets the old file limit
        /// </summary>
        /// <param name="oldFileLimit">The old file limit</param>
        /// <returns>This TextFileTargetConfigBuilder</returns>
        public TextFileTargetConfigBuilder SetOldFileLimit(int oldFileLimit)
        {
            Config.OldFileLimit = oldFileLimit;
            return this;
        }

        /// <summary>
        /// Sets whether or not to compress old files (zip them)
        /// </summary>
        /// <param name="compressOldFiles">Indicates whether or not to compress rotated log files</param>
        /// <returns>This TextFileTargetConfigBuilder</returns>
        public TextFileTargetConfigBuilder SetCompressOldFiles(bool compressOldFiles)
        {
            Config.CompressOldFiles = compressOldFiles;
            return this;
        }

        /// <summary>
        /// Sets the compression level for compressed old files
        /// </summary>
        /// <param name="compressionLevel">The compression level to use for compressing rotated log files</param>
        /// <returns>This TextFileTargetConfigBuilder</returns>
        public TextFileTargetConfigBuilder SetCompressionLevel(CompressionLevel compressionLevel)
        {
            Config.CompressionLevel = compressionLevel;
            return this;
        }

        /// <summary>
        /// Sets an optional password to use for encrypting the old log files
        /// </summary>
        /// <param name="compressionPassword">The password to use to protect the old log files</param>
        /// <returns>This TextFileTargetConfigBuilder</returns>
        public TextFileTargetConfigBuilder SetCompressionPassword(string compressionPassword)
        {
            Config.CompressionPassword = compressionPassword;
            return this;
        }

        /// <summary>
        /// Returns the built TextFileTargetConfig
        /// </summary>
        /// <returns>The built TextFileTargetConfig</returns>
        public TextFileTargetConfig Build()
        {
            return Config;
        }

    }
}
