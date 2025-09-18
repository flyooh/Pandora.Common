using Pandora.Logging;

namespace TestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new LoggerBuilder()
                .InstallConsoleLogger()
                .BuildGlobal();
            Logger.LogDebug("Hello, World!");
        }
    }
}
