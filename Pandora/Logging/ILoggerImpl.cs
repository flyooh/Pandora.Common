namespace Pandora.Logging
{
    /// <summary>
    /// Interface of ILogWriter.
    /// </summary>
    public interface ILoggerImpl
    {
        /// <summary>
        /// Writes debug logging to target.
        /// </summary>
        /// <param name="message">The logging line.</param>
        void WriteDebug(string message);

        /// <summary>
        /// Writes debug logging to target.
        /// </summary>
        /// <param name="message">The logging line.</param>
        void WriteTrace(string message);

        /// <summary>
        /// Writes error logging to target.
        /// </summary>
        /// <param name="message">The logging line.</param>
        void WriteError(string message);

        /// <summary>
        /// Writes info logging to target.
        /// </summary>
        /// <param name="message">The logging line.</param>
        void WriteInfo(string message);
    }
}
