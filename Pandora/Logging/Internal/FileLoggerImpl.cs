using Pandora.Common;
using System;
using System.IO;

namespace Pandora.Logging
{
    /// <summary>
    /// Class of FileLogWriter.
    /// </summary>
    internal class FileLoggerImpl : ILoggerImpl
    {
        /// <summary>
        /// Log file name prefix.
        /// </summary>
        private readonly ILogFileNameProvider _logFileNameProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLoggerImpl"/> class.
        /// </summary>
        /// <param name="logFileName">The log file name prefix.</param>
        /// <param name="logFolder"> The log file stored folder.</param>
        public FileLoggerImpl(ILogFileNameProvider logFileNameProvider)
        {
            Ensure.ArgumentIsNotNull(logFileNameProvider, nameof(logFileNameProvider));
            _logFileNameProvider = logFileNameProvider;
        }

        /// <summary>
        /// Writes log to system Console.
        /// </summary>
        /// <param name="line">The logging line.</param>
        public void WriteDebug(string line)
        {
            string message = string.Format("[DEBUG][{0:HH:mm:ss.fff}] {1}", DateTime.Now, line);
            lock (this)
            {
                using (StreamWriter writer = File.AppendText(_logFileNameProvider.GetLogFileName()))
                {
                    writer.WriteLine(message);
                }
            }
        }

        /// <summary>
        /// Writes log to system Console.
        /// </summary>
        /// <param name="line">The logging line.</param>
        public void WriteTrace(string line)
        {
            string message = string.Format("[TRACE][{0:HH:mm:ss.fff}] {1}", DateTime.Now, line);
            lock (this)
            {
                using (StreamWriter writer = File.AppendText(_logFileNameProvider.GetLogFileName()))
                {
                    writer.WriteLine(message);
                }
            }
        }

        /// <summary>
        /// Writes error logging to target.
        /// </summary>
        /// <param name="line">The logging line.</param>
        public void WriteError(string line)
        {
            string message = string.Format("[ERROR][{0:HH:mm:ss.fff}] {1}", DateTime.Now, line);
            lock (this)
            {
                using (StreamWriter writer = File.AppendText(_logFileNameProvider.GetLogFileName()))
                {
                    writer.WriteLine(message);
                }
            }
        }

        /// <summary>
        /// Writes error logging to target.
        /// </summary>
        /// <param name="line">The logging line.</param>
        public void WriteInfo(string line)
        {
            string message = string.Format("[INFO][{0:HH:mm:ss.fff}] {1}", DateTime.Now, line);
            lock (this)
            {
                using (StreamWriter writer = File.AppendText(_logFileNameProvider.GetLogFileName()))
                {
                    writer.WriteLine(message);
                }
            }
        }
    }
}
