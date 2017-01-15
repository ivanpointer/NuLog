/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration.Targets;
using NuLog.Dispatch;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;

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
        private const int MaxRollWait = 15000; //15 seconds
        private const int AsyncWriteWait = 1000; //1 second

        private const string ZipPattern = "{0}.zip";
        private const int ZipBufferSize = 4096; //4K

        #endregion Constants

        #region Members, Constructors and Initialization

        // My configuration
        private TextFileTargetConfig Config { get; set; }

        // Write utilities
        private static readonly object _fileLock = new object();

        protected readonly Stopwatch MaxWaitStopwatch;
        protected readonly Stopwatch FileCleanupStopwatch;
        protected DateTime? LastWriteTime;

        protected Timer _cleanupTimer;

        private readonly object _cleanupTimerLock;

        /// <summary>
        /// The interval for cleaning up/rotating old logs.
        /// </summary>
        public static int FileCleanupTimerInterval = 5000;

        /// <summary>
        /// Builds a default, empty, unconfigured text file target
        /// </summary>
        public TextFileTarget()
        {
            MaxWaitStopwatch.Start();
            FileCleanupStopwatch.Start();

            _cleanupTimerLock = new object();
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

            // Make sure that the directory exists where we are going to write the log file.
            EnsurePathExists(Config.FileName);

            // Start a timer for cleaning up our files.
        }

        private void StartupFileCleanupTimer()
        {
            lock (_cleanupTimerLock)
            {
                if (_cleanupTimer == null)
                {
                    _cleanupTimer = new Timer(CleanupWorkerThread, this, TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(FileCleanupTimerInterval));
                }
            }
        }

        private void ShutdownFileCleanupTimer()
        {
            lock (_cleanupTimerLock)
            {
                if (_cleanupTimer != null)
                {
                    // Cleanup first
                    CleanupWorkerThread(this);

                    // Shutdown the timer
                    if (_cleanupTimer != null)
                    {
                        _cleanupTimer.Dispose();
                        _cleanupTimer = null;
                    }
                }
            }
        }

        private static void CleanupWorkerThread(object source)
        {
            var target = source as TextFileTarget;

            if (target.MaxWaitStopwatch.IsRunning == false)
                target.MaxWaitStopwatch.Start();

            if (target.FileCleanupStopwatch.ElapsedMilliseconds > RollIdleWait || target.MaxWaitStopwatch.ElapsedMilliseconds > MaxRollWait)
            {
                lock (_fileLock)
                {
                    target.RollFile();
                    target.CleanupOldFiles();
                    target.FileCleanupStopwatch.Stop();
                    target.MaxWaitStopwatch.Stop();
                }
            }
        }

        #endregion Members, Constructors and Initialization

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

                LastWriteTime = GetDateTime();
            }

            FileCleanupStopwatch.Restart();
        }

        protected override void ProcessLogQueue()
        {
            LogEvent logEvent;
            lock (_fileLock)
                using (var fileStream = new StreamWriter(new BufferedStream(File.Open(Config.FileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))))
                    while (LogQueue.IsEmpty == false && !QuickRollCheck())
                    {
                        if (LogQueue.TryDequeue(out logEvent))
                        {
                            try
                            {
                                fileStream.Write(Layout.FormatLogEvent(logEvent));
                                LastWriteTime = GetDateTime();
                            }
                            catch (Exception e)
                            {
                                if (Dispatcher != null)
                                    Dispatcher.HandleException(e, logEvent);
                                else
                                    throw e;
                            }
                        }
                    }

            if (QuickRollCheck())
                RollFile();

            FileCleanupStopwatch.Restart();
        }

        #endregion Logging

        #region Rollover, Cleanup and Utility

        // Performs a quick check to see if a rollover is needed, and if it is, performs a rollover
        private bool QuickRollCheck()
        {
            if (Config.RolloverPolicy == RolloverPolicy.Day)
            {
                if (LastWriteTime.HasValue && LastWriteTime.Value.Day != GetDateTime().Day)
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
                    throw new LoggingException(string.Format(RolloverPolicyNotImplementedMessage, Config.RolloverPolicy));
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
            using (var fileStream = new StreamWriter(new BufferedStream(File.Open(Config.FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))))
                fileStream.Write(string.Empty);

#if !NET40
            // Compress it if it is needed
            if (Config.CompressOldFiles)
                CompressFile(oldFileName);
#endif
        }

        // Figure out the new name for the log file being rolled over
        private string GetOldFileName()
        {
            // Build out the name of the file we're about to archive into,
            //  based on the current date/time
            var oldFileName = string.Format(Config.OldFileNamePattern, GetDateTime());

            // Determine if the old file name is rooted, and if not, place the
            //  old file relatively to the main log file
            if (Path.IsPathRooted(oldFileName) == false)
            {
                var mainFile = new FileInfo(Config.FileName);
                oldFileName = Path.Combine(mainFile.DirectoryName, oldFileName);
            }

            // Append a number to the file to make it unique
            int fileNumber = 1;
            string newOldFileName = oldFileName;
            while (File.Exists(newOldFileName))
                newOldFileName = string.Format("{0}.{1}", oldFileName, fileNumber++);

            // Return what we've got
            return oldFileName;
        }

        // Cleanup old log files, recursively
        private void CleanupOldFiles(string path = null, string pattern = null)
        {
            if (Config.OldFileLimit > 0)
            {
                // Setup the path and pattern
                if (string.IsNullOrEmpty(path))
                    path = string.Format(Config.OldFileNamePattern, "");
                if (string.IsNullOrEmpty(pattern))
                    pattern = string.Format(Config.OldFileNamePattern, "*");

                // Used for making sure that we grab the right number of files
                int countOffset = 1;
                if (Config.CompressOldFiles)
                {
                    pattern = string.Format(ZipPattern, pattern);
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

#if !NET40

        // Compresses the given file, and optionally deletes the source file when finished
        private void CompressFile(string fileName, bool deleteFile = true)
        {
            // zip up the file, taking the compression level from the configuration
            var zipFileName = string.Format(ZipPattern, fileName);

            using (var fileStream = File.OpenRead(fileName))
            using (var destFile = File.Create(zipFileName))
            using (var compStream = new GZipStream(destFile, CompressionLevel.Optimal))
            {
                int theByte = fileStream.ReadByte();
                while (theByte != -1)
                {
                    compStream.WriteByte((byte)theByte);
                    theByte = fileStream.ReadByte();
                }
            }

            if (deleteFile)
                File.Delete(fileName);
        }

#endif

        #endregion Rollover, Cleanup and Utility

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

        #endregion Helpers
    }
}