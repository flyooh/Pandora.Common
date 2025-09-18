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
        public static void LogDebug(string message)
        {
            GlobalLogger?.LogDebug(message);
        }

        /// <summary>
        /// Logs debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void LogDebugFormat(string format, params object[] args)
        {
            GlobalLogger.LogDebugFormat(format, args);
        }

        /// <summary>
        /// Logs debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void LogTrace(string message)
        {
            GlobalLogger?.LogTrace(message);
        }

        /// <summary>
        /// Logs debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void LogTraceFormat(string format, params object[] args)
        {
            GlobalLogger?.LogTraceFormat(format, args);
        }

        /// <summary>
        /// Logs information message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void LogInfo(string message)
        {
            GlobalLogger?.LogInfo(message);
        }

        /// <summary>
        /// Logs debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void LogInfoFormat(string format, params object[] args)
        {
            GlobalLogger?.LogInfoFormat(format, args);
        }

        /// <summary>
        /// Logs error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void LogError(string message)
        {
            GlobalLogger?.LogError(message);
        }

        /// <summary>
        /// Logs error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void LogErrorFormat(string format, params object[] args)
        {
            GlobalLogger?.LogErrorFormat(format, args);
        }
    }
}
