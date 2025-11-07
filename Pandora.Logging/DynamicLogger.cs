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
        public ILogger LogDebug(string message)
        {
            if (LogLevel >= LogLevel.Debug)
            {
                _loggerImpl.Write(GetFullMessage(message));
            }

            return this;
        }

        /// <summary>
        /// Logs debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public ILogger LogDebugFormat(string format, params object[] args)
        {
            if (LogLevel >= LogLevel.Debug)
            {
                _loggerImpl.Write(GetFullMessage(string.Format(format, args)));
            }

            return this;
        }

        /// <summary>
        /// Logs debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public ILogger LogTrace(string message)
        {
            if (LogLevel >= LogLevel.Trace)
            {
                _loggerImpl.Write(GetFullMessage(message));
            }

            return this;
        }

        /// <summary>
        /// Logs debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public ILogger LogTraceFormat(string format, params object[] args)
        {
            if (LogLevel >= LogLevel.Trace)
            {
                _loggerImpl.Write(GetFullMessage(string.Format(format, args)));
            }

            return this;
        }

        /// <summary>
        /// Logs information message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public ILogger LogInfo(string message)
        {
            if (LogLevel >= LogLevel.Info)
            {
                _loggerImpl.Write(GetFullMessage(message));
            }

            return this;
        }

        /// <summary>
        /// Logs debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public ILogger LogInfoFormat(string format, params object[] args)
        {
            if (LogLevel >= LogLevel.Info)
            {
                _loggerImpl.Write(GetFullMessage(string.Format(format, args)));
            }

            return this;
        }

        /// <summary>
        /// Logs error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public ILogger LogError(string message)
        {
            if (LogLevel >= LogLevel.Error)
            {
                _loggerImpl.Write(GetFullMessage(message));
            }

            return this;
        }

        /// <summary>
        /// Logs error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public ILogger LogErrorFormat(string format, params object[] args)
        {
            if (LogLevel >= LogLevel.Error)
            {
                _loggerImpl.Write(GetFullMessage(string.Format(format, args)));
            }

            return this;
        }

        private string GetFullMessage(string message)
        {
            return $"{_prefixFormatter.GetPrefix(LogLevel)}{message}";
        }
    }
}
