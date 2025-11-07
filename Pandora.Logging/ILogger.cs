namespace Pandora.Logging
{
    public interface ILogger
    {
        LogLevel LogLevel { get; }

        ILogger LogDebug(string message);

        ILogger LogDebugFormat(string format, params object[] args);

        ILogger LogTrace(string message);

        ILogger LogTraceFormat(string format, params object[] args);

        ILogger LogInfo(string message);

        ILogger LogInfoFormat(string format, params object[] args);

        ILogger LogError(string message);

        ILogger LogErrorFormat(string format, params object[] args);
    }
}
