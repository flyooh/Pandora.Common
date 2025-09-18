namespace Pandora.Logging
{
    /// <summary>
    /// Interface of ILogWriter.
    /// </summary>
    public interface ILoggerImpl
    {
        /// <summary>
        /// Writes log message to target.
        /// </summary>
        /// <param name="message">The log message.</param>
        void Write(string message);
    }
}
