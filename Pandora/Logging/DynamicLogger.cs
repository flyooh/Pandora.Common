using Pandora.Common;

namespace Pandora.Logging
{
    /// <summary>
    /// Class used to log message.
    /// </summary>
    public class DynamicLogger : ILogger
    {
        private readonly ILoggerImpl _loggerImpl;

        private readonly ILogPrefixFormatter _prefixFormatter;

        private readonly LogLevel _logLevel;

        public DynamicLogger(ILoggerImpl loggerImpl, ILogPrefixFormatter prefixFormatter, LogLevel logLevel)
        { 
            Ensure.ArgumentIsNotNull(loggerImpl, nameof(loggerImpl));
            Ensure.ArgumentIsNotNull(prefixFormatter, nameof(prefixFormatter));

            _loggerImpl = loggerImpl;
            _prefixFormatter = prefixFormatter;
            _logLevel = logLevel;
        }

        /// <summary>
        /// Gets or sets log level.
        /// </summary>
        public LogLevel LogLevel => _logLevel;

        /// <summary>
        /// Logs debug message.
        /// </summary>
        /// <param name="message">The format.</param>
        public void LogDebug(string message)
        {
            if (LogLevel >= LogLevel.Debug)
            {
                _loggerImpl.Write(GetFullMessage(message));
            }
        }

        /// <summary>
        /// Logs debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void LogDebugFormat(string format, params object[] args)
        {
            if (LogLevel >= LogLevel.Debug)
            {
                _loggerImpl.Write(GetFullMessage(string.Format(format, args)));
            }
        }

        /// <summary>
        /// Logs debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void LogTrace(string message)
        {
            if (LogLevel >= LogLevel.Trace)
            {
                _loggerImpl.Write(GetFullMessage(message));
            }
        }

        /// <summary>
        /// Logs debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void LogTraceFormat(string format, params object[] args)
        {
            if (LogLevel >= LogLevel.Trace)
            {
                _loggerImpl.Write(GetFullMessage(string.Format(format, args)));
            }
        }

        /// <summary>
        /// Logs information message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void LogInfo(string message)
        {
            if (LogLevel >= LogLevel.Info)
            {
                _loggerImpl.Write(GetFullMessage(message));
            }
        }

        /// <summary>
        /// Logs debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void LogInfoFormat(string format, params object[] args)
        {
            if (LogLevel >= LogLevel.Info)
            {
                _loggerImpl.Write(GetFullMessage(string.Format(format, args)));
            }
        }

        /// <summary>
        /// Logs error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void LogError(string message)
        {
            if (LogLevel >= LogLevel.Error)
            {
                _loggerImpl.Write(GetFullMessage(message));
            }
        }

        /// <summary>
        /// Logs error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void LogErrorFormat(string format, params object[] args)
        {
            if (LogLevel >= LogLevel.Error)
            {
                _loggerImpl.Write(GetFullMessage(string.Format(format, args)));
            }
        }

        private string GetFullMessage(string message)
        {
            return $"{_prefixFormatter.GetPrefix(LogLevel)}{message}";
        }
    }
}
