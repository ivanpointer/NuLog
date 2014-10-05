using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using NuLog.Configuration.Targets;
using NuLog.Dispatch;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NuLog.Targets
{
    public class TextFileTarget : LayoutTargetBase
    {
        private const int RollIdleWait = 3000; //3 seconds
        private const int AsyncWriteWait = 1000; //1 second

        private TextFileTargetConfig Config { get; set; }

        private static readonly object _fileLock = new object();
        private Stopwatch _sw;

        public TextFileTarget()
        {
            _sw = new Stopwatch();
        }

        public override void Log(LogEvent logEvent)
        {
            lock (_fileLock)
                using (var fileStream = new StreamWriter(new BufferedStream(File.Open(Config.FileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))))
                    fileStream.Write(Layout.FormatLogEvent(logEvent));

            _sw.Restart();
        }

        protected override void ProcessLogQueue(ConcurrentQueue<LogEvent> logQueue, LogEventDispatcher dispatcher)
        {
            LogEvent logEvent;
            lock (_fileLock)
                using (var fileStream = new StreamWriter(new BufferedStream(File.Open(Config.FileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))))
                    while (logQueue.IsEmpty == false)
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
            _sw.Restart();
        }

        public void RollFile()
        {
            var fileInfo = new FileInfo(Config.FileName);
            if (Config.RolloverPolicy != RolloverPolicy.None && Config.RolloverTrigger > 0 && fileInfo.Exists)
            {
                if (Config.RolloverPolicy == RolloverPolicy.Day)
                {
                    RolloverDayLimit();
                }
                else if (Config.RolloverPolicy == RolloverPolicy.Size)
                {
                    RolloverSizeLimit(fileInfo);
                }
                else
                {
                    throw new LoggingException(String.Format("Rollover policy {0} not implemented", Config.RolloverPolicy));
                }
            }
        }

        private void RolloverDayLimit()
        {
            var creationTime = File.GetCreationTime(Config.FileName);
            if (creationTime.AddDays(Config.RolloverTrigger) < DateTime.Now)
            {
                var oldFileName = GetOldFileName();

                File.Copy(Config.FileName, oldFileName);
                File.Delete(Config.FileName);

                if (Config.CompressOldFiles)
                    CompressFile(oldFileName);
            }
        }

        private void RolloverSizeLimit(FileInfo fileInfo)
        {
            var creationTime = File.GetCreationTime(Config.FileName);
            if (fileInfo.Length > Config.RolloverTrigger)
            {
                var oldFileName = GetOldFileName();

                File.Copy(Config.FileName, oldFileName);
                File.Delete(Config.FileName);
                using (File.Create(Config.FileName)) { }

                if (Config.CompressOldFiles)
                    CompressFile(oldFileName);
            }
        }

        private string GetOldFileName()
        {
            var oldFileName = String.Format(Config.OldFileNamePattern, DateTime.Now);

            int fileNumber = 1;
            string newOldFileName = oldFileName;
            while (File.Exists(newOldFileName))
                newOldFileName = String.Format("{0}.{1}", oldFileName, fileNumber++);

            return oldFileName;
        }

        private void CleanupOldFiles(string path = null, string pattern = null, bool recurse = false)
        {
            if (!recurse && (String.IsNullOrEmpty(path) || String.IsNullOrEmpty(pattern)))
            {
                var cleanupPath = String.Format(Config.OldFileNamePattern, "");
                var cleanupPattern = String.Format(Config.OldFileNamePattern, "*");
                CleanupOldFiles(Path.GetDirectoryName(Path.GetFullPath(cleanupPath)), cleanupPattern, true);
            }
            else
            {
                if (Config.OldFileLimit > 0)
                {
                    int countOffset = 1;
                    if (Config.CompressOldFiles)
                    {
                        pattern = String.Format("{0}.zip", pattern);
                        countOffset = 0;
                    }

                    var files = Directory.GetFiles(path, pattern);
                    var orderedFiles = files.OrderByDescending(_ => (new FileInfo(_)).CreationTime);
                    var deleteFiles = files.Take(orderedFiles.Count() - Config.OldFileLimit - countOffset);
                    foreach (var deleteFile in deleteFiles)
                        if (Path.GetFileName(deleteFile) != Path.GetFileName(Config.FileName))
                            File.Delete(deleteFile);
                }
            }
        }

        private void CompressFile(string fileName, bool deleteFile = true)
        {
            var zipFileName = String.Format("{0}.zip", fileName);
            using (FileStream zipFileStream = File.Create(zipFileName))
            using (ZipOutputStream zipStream = new ZipOutputStream(zipFileStream))
            {
                zipStream.SetLevel(Config.CompressionLevel);

                if (String.IsNullOrEmpty(Config.CompressionPassword) == false)
                    zipStream.Password = Config.CompressionPassword;

                var newEntry = new ZipEntry(fileName);
                zipStream.PutNextEntry(newEntry);

                var buffer = new byte[4096];
                using (FileStream streamReader = File.OpenRead(fileName))
                    StreamUtils.Copy(streamReader, zipStream, buffer);

                zipStream.CloseEntry();
                zipStream.IsStreamOwner = true;
            }

            if (deleteFile)
                File.Delete(fileName);
        }

        public override void Initialize(TargetConfig targetConfig, bool? synchronous = null)
        {
            base.Initialize(targetConfig, synchronous);

            if (targetConfig != null)
                Config = targetConfig is TextFileTargetConfig
                    ? (TextFileTargetConfig)targetConfig
                    : new TextFileTargetConfig(targetConfig.Config);

            EnsurePathExists(Config.FileName);

            Task.Factory.StartNew(() =>
            {
                while (!DoShutdown)
                {
                    if (_sw.IsRunning && _sw.ElapsedMilliseconds > RollIdleWait)
                    {
                        lock (_fileLock)
                        {
                            RollFile();
                            CleanupOldFiles();
                            _sw.Stop();
                        }
                    }

                    Thread.Yield();
                    Thread.Sleep(RollIdleWait);
                }
            });
        }

        private static void EnsurePathExists(string fileName)
        {
            var parentDir = Path.GetDirectoryName(Path.GetFullPath(fileName));
            Directory.CreateDirectory(parentDir);
        }
    }
}
