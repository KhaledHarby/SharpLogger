using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharp.Logging;
using Sharp.Formatters;
using System.IO;
using System.Threading;

namespace Sharp.Loggers
{
    public class SharpFileLogger : ISharpLoggerHandler
    {
        private object locker = new object();
        private readonly string _fileName;
        private readonly string _directory;
        private readonly ISharpLoggerFormatter _loggerFormatter;


        public SharpFileLogger() : this(CreateFileName())
        {
            _directory = AppDomain.CurrentDomain.BaseDirectory;
        }

        public SharpFileLogger(string fileName) : this(fileName, string.Empty)
        {
            _directory = AppDomain.CurrentDomain.BaseDirectory;
        }

        public SharpFileLogger(string fileName, string directory) : this(new SharpLoggerFormatter(), fileName, directory) { }

        public SharpFileLogger(ISharpLoggerFormatter loggerFormatter) : this(loggerFormatter, CreateFileName()) { }

        public SharpFileLogger(ISharpLoggerFormatter loggerFormatter, string fileName) : this(loggerFormatter, fileName, string.Empty) { }

        public SharpFileLogger(ISharpLoggerFormatter loggerFormatter, string fileName, string directory)
        {
            _loggerFormatter = loggerFormatter;
            _fileName = fileName;
            _directory = directory;
        }

        public void Publish(LogMessage logMessage)
        {
            lock (locker)
            {
                string LogFilePath = CreateFileName();
                string ArchiveFilePath = string.Format("{0}{1}Log_{2}_{3}.log", _directory, @"Logs\", DateTime.Now.ToString("yyyyMMddHHmmss") ,  Guid.NewGuid());
                if (!string.IsNullOrEmpty(_directory))
                {
                    var directoryInfo = new DirectoryInfo(Path.Combine(_directory));
                    if (!directoryInfo.Exists)
                        directoryInfo.Create();
                }

                // check file existing
                if (File.Exists(LogFilePath))
                {

                    // check log directory existing
                    string LogDirectoryPath = _directory + @"Logs\";
                    if (!Directory.Exists(LogDirectoryPath))
                    {
                        Directory.CreateDirectory(LogDirectoryPath);
                    }

                    // check file exisiting
                    FileInfo finfo = new FileInfo(LogFilePath);
                    long FileSzinKB = (finfo.Length / 1024);
                    if (FileSzinKB >= 1024) // archive log files more than or equal to 1024 KB
                    {
                        File.Copy(LogFilePath, string.Format(ArchiveFilePath, Guid.NewGuid().ToString().Replace("-", "")));
                        File.Delete(LogFilePath);
                    }
                }

                // writing message
                using (var writer = new StreamWriter(File.Open(Path.Combine(_directory, _fileName), FileMode.Append)))
                    writer.WriteLine(_loggerFormatter.ApplyFormat(logMessage));
            }
        }

        private static string CreateFileName()
        {
            var currentDate = DateTime.Now;
            var guid = Guid.NewGuid();
            return string.Format("Log.log",
                currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, currentDate.Minute, guid);
        }
    }
}
