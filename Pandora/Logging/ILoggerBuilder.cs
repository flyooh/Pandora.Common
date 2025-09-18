namespace Pandora.Logging
{
    public interface ILoggerBuilder
    {
        ILogger Build();

        void BuildGlobal();
    }
}
