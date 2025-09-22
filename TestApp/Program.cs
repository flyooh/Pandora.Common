using Pandora.Common;
using Pandora.Logging;
using System;
using System.Buffers;
using System.Reflection.Metadata;
using TestApp.Buffers.Tests;

namespace TestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ////var logger = new LoggerBuilder()
            ////    .InstallConsoleLogger()
            ////    .InstallDefaultFileLogger("a.log")
            ////    .BuildGlobal();


            ////RetryExecutor.TryUntilSuccessOrMaxAttempts(10, TimeSpan.FromSeconds(1), () =>
            ////{
            ////    Logger.LogDebug("Hello, World!")
            ////          .LogInfo("This is a flower")
            ////          .LogErrorFormat("It is not right time. {0}", DateTime.Now);
            ////    return false;
            ////});

            NativeMemoryPoolExample example = new NativeMemoryPoolExample();
            //example.ProcessLargeData();
            //example.ProcessLargeData();
            //example.ProcessLargeData();
            //example.ProcessLargeData();
            example.ProcessWithSpan();
            example.MonitorPoolUsage();
        }
    }
}
