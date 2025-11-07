using System;

namespace Pandora.Logging
{
    /// <summary>
    /// The class of ConsolerLogWriter.
    /// </summary>
    internal class ConsolerLoggerImpl : ILoggerImpl
    {
        /// <summary>
        /// Writes log to system Console.
        /// </summary>
        /// <param name="message">The logging line.</param>
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }
}
