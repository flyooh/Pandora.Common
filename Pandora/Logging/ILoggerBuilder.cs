namespace Pandora.Logging
{
    public interface ILoggerBuilder
    {
        ILogger Build();

        ILogger BuildGlobal();
    }
}
