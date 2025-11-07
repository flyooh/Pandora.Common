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
        /// <param name="message">The logging line.</param>
        public void Write(string message)
        {
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
