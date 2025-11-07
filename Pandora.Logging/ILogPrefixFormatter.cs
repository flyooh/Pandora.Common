namespace Pandora.Logging
{
    public interface ILogPrefixFormatter
    {
        string GetPrefix(LogLevel logLevel);
    }
}
