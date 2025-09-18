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
        /// <param name="line">The logging line.</param>
        public void WriteDebug(string line)
        {
            Console.WriteLine("[DBG][{0:yyyy/MM/dd_HH:mm:ss.fff}] {1}", DateTime.Now, line);
        }

        /// <summary>
        /// Writes log to system Console.
        /// </summary>
        /// <param name="line">The logging line.</param>
        public void WriteTrace(string line)
        {
            Console.WriteLine("[TRC][{0:yyyy/MM/dd_HH:mm:ss.fff}] {1}", DateTime.Now, line);
        }

        /// <summary>
        /// Writes error logging to target.
        /// </summary>
        /// <param name="line">The logging line.</param>
        public void WriteError(string line)
        {
            Console.WriteLine("[ERR][{0:yyyy/MM/dd_HH:mm:ss.fff}] {1}", DateTime.Now, line);
        }

        /// <summary>
        /// Writes error logging to target.
        /// </summary>
        /// <param name="line">The logging line.</param>
        public void WriteInfo(string line)
        {
            Console.WriteLine("[INF][{0:yyyy/MM/dd_HH:mm:ss.fff}] {1}", DateTime.Now, line);
        }
    }
}
