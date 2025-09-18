namespace Pandora.Logging
{
    public interface ILogger
    {
        LogLevel LogLevel { get; }

        void LogDebug(string message);

        void LogDebugFormat(string format, params object[] args);

        void LogTrace(string message);

        void LogTraceFormat(string format, params object[] args);

        void LogInfo(string message);

        void LogInfoFormat(string format, params object[] args);

        void LogError(string message);

        void LogErrorFormat(string format, params object[] args);
    }
}
