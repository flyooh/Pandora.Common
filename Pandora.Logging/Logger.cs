namespace Pandora.Logging
{
    /// <summary>
    /// Class used to log message.
    /// </summary>
    public static class Logger
    {
        internal static ILogger GlobalLogger { get; set; }

        /// <summary>
        /// Logs debug message.
        /// </summary>
        /// <param name="message">The format.</param>
        public static ILogger LogDebug(string message)
        {
            return GlobalLogger?.LogDebug(message);
        }

        /// <summary>
        /// Logs debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static ILogger LogDebugFormat(string format, params object[] args)
        {
            return GlobalLogger?.LogDebugFormat(format, args);
        }

        /// <summary>
        /// Logs debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static ILogger LogTrace(string message)
        {
            return GlobalLogger?.LogTrace(message);
        }

        /// <summary>
        /// Logs debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static ILogger LogTraceFormat(string format, params object[] args)
        {
            return GlobalLogger?.LogTraceFormat(format, args);
        }

        /// <summary>
        /// Logs information message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static ILogger LogInfo(string message)
        {
            return GlobalLogger?.LogInfo(message);
        }

        /// <summary>
        /// Logs debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static ILogger LogInfoFormat(string format, params object[] args)
        {
            return  GlobalLogger?.LogInfoFormat(format, args);
        }

        /// <summary>
        /// Logs error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static ILogger LogError(string message)
        {
            return GlobalLogger?.LogError(message);
        }

        /// <summary>
        /// Logs error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static ILogger LogErrorFormat(string format, params object[] args)
        {
            return GlobalLogger?.LogErrorFormat(format, args);
        }
    }
}
