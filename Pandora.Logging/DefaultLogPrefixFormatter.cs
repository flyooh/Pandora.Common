using System;

namespace Pandora.Logging
{
    internal class DefaultLogPrefixFormatter : ILogPrefixFormatter
    {
        public static ILogPrefixFormatter Instance = new DefaultLogPrefixFormatter();

        public string GetPrefix(LogLevel logLevel)
        {
            return string.Format("[{0}][{1:yyyy/MM/dd}][{1:HH:mm:ss.fff}] ", logLevel, DateTime.Now);
        }
    }
}
