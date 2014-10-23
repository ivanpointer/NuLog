/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/9/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using NuLog.Configuration.Targets;
using NuLog.Dispatch;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NuLog.Targets
{
    /// <summary>
    /// Writes log events to text file
    /// </summary>
    public class TextFileTarget : LayoutTargetBase
    {

        #region Constants

        private const string RolloverPolicyNotImplementedMessage = "Rollover policy {0} not implemented";

        private const int RollIdleWait = 3000; //3 seconds
        private const int MaxRollWait = 10000; //10 seconds
        private const int AsyncWriteWait = 1000; //1 second

        private const string ZipPattern = "{0}.zip";
        private const int ZipBufferSize = 4096; //4K

        #endregion

        #region Members, Constructors and Initialization

        // My configuration
        private TextFileTargetConfig Config { get; set; }

        // Write utilities
        private static readonly object _fileLock = new object();
        private Stopwatch _sw;
        private DateTime _lastWrite;

        /// <summary>
        /// Builds a default, empty, unconfigured text file target
        /// </summary>
        public TextFileTarget()
        {
            _sw = new Stopwatch();
        }

        /// <summary>
        /// Initializes this text file target
        /// </summary>
        /// <param name="targetConfig">The configuration for this target</param>
        /// <param name="dispatcher">The dispatcher this target belongs to</param>
        /// <param name="synchronous">An override to the synchronous flag in the target config</param>
        public override void Initialize(TargetConfig targetConfig, LogEventDispatcher dispatcher = null, bool? synchronous = null)
        {
            // Initialize the target using the base
            base.Initialize(targetConfig, dispatcher, synchronous);

            // Make sure we have an initialized text file target
            if (targetConfig != null)
                Config = targetConfig is TextFileTargetConfig
                    ? (TextFileTargetConfig)targetConfig
                    : new TextFileTargetConfig(targetConfig.Config);

            // Setup a bit.  Make sure that the directory exists where we are going to write the log file.
            //  Start a worker task for rolling the log file and cleaning the old files

            EnsurePathExists(Config.FileName);

            Task.Factory.StartNew(() =>
            {
                // Run this task until the target is told to shutdown
                // Watch for the internal stopwatch running, which indicates that data has been written to log
                // Wait for the minimum idle time since the last write, or the maximum wait time to roll/cleanup the files

                Stopwatch maxWaitStopwatch = new Stopwatch();
                while (!DoShutdown)
                {
                    if (_sw.IsRunning)
                    {
                        if (maxWaitStopwatch.IsRunning == false)
                            maxWaitStopwatch.Start();

                        if (_sw.ElapsedMilliseconds > RollIdleWait || maxWaitStopwatch.ElapsedMilliseconds > MaxRollWait)
                        {
                            lock (_fileLock)
                            {
                                RollFile();
                                CleanupOldFiles();
                                _sw.Stop();
                                maxWaitStopwatch.Stop();
                            }
                        }
                    }

                    Thread.Yield();
                    Thread.Sleep(RollIdleWait);
                }
            });
        }

        #endregion

        #region Logging

        /// <summary>
        /// Log a single log event to file
        /// </summary>
        /// <param name="logEvent">The log event to write</param>
        public override void Log(LogEvent logEvent)
        {
            lock (_fileLock)
            {
                if (QuickRollCheck())
                    RollFile();

                using (var fileStream = new StreamWriter(new BufferedStream(File.Open(Config.FileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))))
                    fileStream.Write(Layout.FormatLogEvent(logEvent));
            }

            _sw.Restart();
        }

        protected override void ProcessLogQueue(ConcurrentQueue<LogEvent> logQueue, LogEventDispatcher dispatcher)
        {
            LogEvent logEvent;
            lock (_fileLock)
                using (var fileStream = new StreamWriter(new BufferedStream(File.Open(Config.FileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))))
                    while (logQueue.IsEmpty == false && !QuickRollCheck())
                    {
                        if (logQueue.TryDequeue(out logEvent))
                        {
                            try
                            {
                                fileStream.Write(Layout.FormatLogEvent(logEvent));
                            }
                            catch (Exception e)
                            {
                                if (dispatcher != null)
                                    dispatcher.HandleException(e, logEvent);
                                else
                                    throw e;
                            }
                        }
                    }

            if (QuickRollCheck())
                RollFile();

            _sw.Restart();
        }

        #endregion

        #region Rollover, Cleanup and Utility

        // Performs a quick check to see if a rollover is needed, and if it is, performs a rollover
        private bool QuickRollCheck()
        {
            if (Config.RolloverPolicy == RolloverPolicy.Day)
            {
                if (_lastWrite != null && _lastWrite.Day != GetDateTime().Day)
                {
                    return true;
                }
            }

            return false;
        }

        // Rolls the log file based on the configured rollover policy
        private void RollFile()
        {
            // This is simply a switch for calling the appropriate function for rollover, based on policy

            var fileInfo = new FileInfo(Config.FileName);
            if (Config.RolloverPolicy != RolloverPolicy.None && Config.RolloverTrigger > 0 && fileInfo.Exists)
            {
                if (Config.RolloverPolicy == RolloverPolicy.Day)
                {
                    RolloverDayLimit();
                }
                else if (Config.RolloverPolicy == RolloverPolicy.Size)
                {
                    RolloverSizeLimit();
                }
                else
                {
                    throw new LoggingException(String.Format(RolloverPolicyNotImplementedMessage, Config.RolloverPolicy));
                }
            }
        }

        // Rollover the file based on a daily strategy
        private void RolloverDayLimit()
        {
            // Figure out if it has been more than the configured number of days, and rollover if it has been
            var creationTime = File.GetCreationTime(Config.FileName);
            if (DateTime.Today.Subtract(creationTime.Date).Days >= Config.RolloverTrigger)
                RolloverFile();
        }

        // Rollover the file based on size limit
        private void RolloverSizeLimit()
        {
            var fileInfo = new FileInfo(Config.FileName);
            if (fileInfo.Length > Config.RolloverTrigger)
                RolloverFile();
        }

        // Rollover the log file
        private void RolloverFile()
        {
            // Figure out what the new (old) file name is
            var oldFileName = GetOldFileName();

            // Copy the log over, delete it, and create a new, empty file
            File.Copy(Config.FileName, oldFileName);
            File.Delete(Config.FileName);
            using (File.Create(Config.FileName)) { }

            // Compress it if it is needed
            if (Config.CompressOldFiles)
                CompressFile(oldFileName);
        }

        // Figure out the new name for the log file being rolled over
        private string GetOldFileName()
        {
            var oldFileName = String.Format(Config.OldFileNamePattern, GetDateTime());

            int fileNumber = 1;
            string newOldFileName = oldFileName;
            while (File.Exists(newOldFileName))
                newOldFileName = String.Format("{0}.{1}", oldFileName, fileNumber++);

            return oldFileName;
        }

        // Cleanup old log files, recursively
        private void CleanupOldFiles(string path = null, string pattern = null)
        {
            if (Config.OldFileLimit > 0)
            {
                // Setup the path and pattern
                if (String.IsNullOrEmpty(path))
                    path = String.Format(Config.OldFileNamePattern, "");
                if (String.IsNullOrEmpty(pattern))
                    pattern = String.Format(Config.OldFileNamePattern, "*");

                // Used for making sure that we grab the right number of files
                int countOffset = 1;
                if (Config.CompressOldFiles)
                {
                    pattern = String.Format(ZipPattern, pattern);
                    countOffset = 0;
                }

                // Get a hold of the files to delete, and delete them (making sure not to delete the current log file)
                //  Delete the oldest of the files
                var file = new FileInfo(path);
                var files = file.Directory.GetFiles(pattern);
                var orderedFiles = files.OrderByDescending(_ => _.CreationTime);
                var deleteFiles = files.Take(orderedFiles.Count() - Config.OldFileLimit - countOffset);
                foreach (var deleteFile in deleteFiles)
                    if (deleteFile.Name != Path.GetFileName(Config.FileName))
                        deleteFile.Delete();
            }
        }

        // Compresses the given file, and optionally deletes the source file when finished
        private void CompressFile(string fileName, bool deleteFile = true)
        {
            // zip up the file, taking the compression level and password from the configuration
            var zipFileName = String.Format(ZipPattern, fileName);
            using (FileStream zipFileStream = File.Create(zipFileName))
            using (ZipOutputStream zipStream = new ZipOutputStream(zipFileStream))
            {
                zipStream.SetLevel(Config.CompressionLevel);

                if (String.IsNullOrEmpty(Config.CompressionPassword) == false)
                    zipStream.Password = Config.CompressionPassword;

                var newEntry = new ZipEntry(fileName);
                zipStream.PutNextEntry(newEntry);

                var buffer = new byte[ZipBufferSize];
                using (FileStream streamReader = File.OpenRead(fileName))
                    StreamUtils.Copy(streamReader, zipStream, buffer);

                zipStream.CloseEntry();
                zipStream.IsStreamOwner = true;
            }

            if (deleteFile)
                File.Delete(fileName);
        }

        #endregion

        #region Helpers

        // Create the directories leading up to the file as identified by the file name
        private static void EnsurePathExists(string fileName)
        {
            var parentDir = Path.GetDirectoryName(Path.GetFullPath(fileName));
            Directory.CreateDirectory(parentDir);
        }

        // Gets the date time, this is pulled off into a separate function like this so we can easily
        //  change this from local, to UTC, or even loaded from config
        private DateTime GetDateTime()
        {
            return DateTime.Now;
        }

        #endregion

    }
}
