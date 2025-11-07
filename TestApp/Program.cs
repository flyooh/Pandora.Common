using Pandora.Common;
using Pandora.Logging;
using System;

namespace TestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TestGlobalLoggerAndDefaultLogFileNameProvider();
            TestGlobalLoggerAndUserLogFileNameProvider();
            TestUserLogger();
        }

        static void TestGlobalLoggerAndDefaultLogFileNameProvider()
        {
            var logger = new LoggerBuilder()
            .InstallConsoleLogger()
            .InstallDefaultFileLogger("global_logger_default_name_provider.log")
            .BuildGlobal();


            RetryExecutor.TryUntilSuccessOrMaxAttempts(10, TimeSpan.FromSeconds(1), () =>
            {
                Logger.LogDebug("Hello, World!")
                      .LogInfo("This is a flower")
                      .LogErrorFormat("It is not right time. {0}", DateTime.Now);
                return false;
            });
        }

        static void TestGlobalLoggerAndUserLogFileNameProvider()
        {
            var logger = new LoggerBuilder()
            .InstallConsoleLogger()
            .InstallFileLogger(new MyLogFileNameProvider())
            .BuildGlobal();


            RetryExecutor.TryUntilSuccessOrMaxAttempts(10, TimeSpan.FromSeconds(1), () =>
            {
                Logger.LogDebug("Hello, World!")
                      .LogInfo("This is a flower")
                      .LogErrorFormat("It is not right time. {0}", DateTime.Now);
                return false;
            });
        }

        static void TestUserLogger()
        {
            var logger = new LoggerBuilder()
           .InstallConsoleLogger()
           .InstallDefaultFileLogger("user_logger_default_name_provider.log")
           .Build();


            RetryExecutor.TryUntilSuccessOrMaxAttempts(10, TimeSpan.FromSeconds(1), () =>
            {
                logger.LogDebug("Hello, World!")
                      .LogInfo("This is a flower")
                      .LogErrorFormat("It is not right time. {0}", DateTime.Now);
                return false;
            });
        }
    }

    class MyLogFileNameProvider : ILogFileNameProvider
    {
        public string GetLogFileName()
        {
            return string.Format("global_logger_user_name_provider_{0:yyyyMMdd}.log", DateTime.Now);
        }
    }
}
