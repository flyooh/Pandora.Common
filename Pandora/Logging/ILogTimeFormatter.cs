using System;

namespace Pandora.Logging
{
    public interface ILogTimeFormatter
    {
        string FormatTime(LogLevel logLevel);
    }
}
