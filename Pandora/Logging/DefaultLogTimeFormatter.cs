using System;

namespace Pandora.Logging
{
    internal class DefaultLogTimeFormatter : ILogTimeFormatter
    {
        public string FormatTime(LogLevel logLevel)
        {
            return string.Format("[{0}][{1:HH:mm:ss.fff}] ", logLevel, DateTime.Now);
        }
    }
}
