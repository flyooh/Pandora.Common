using Pandora.Common;

namespace Pandora.Logging
{
    public class LoggerBuilder : ILoggerBuilder
    {
        private readonly CompositionLoggerImpl _composition;

        private readonly LogLevel _level = LogLevel.Debug;

        public LoggerBuilder()
        {
            _composition = new CompositionLoggerImpl();
        }

        public LoggerBuilder InstallConsoleLogger()
        { 
            _composition.Add(new ConsolerLoggerImpl());
            return this;
        }

        public LoggerBuilder InstallFileLogger(ILogFileNameProvider logFileNameProvider)
        {
            Ensure.ArgumentIsNotNull(logFileNameProvider, nameof(logFileNameProvider));
            _composition.Add(new FileLoggerImpl(logFileNameProvider));
            return this;
        }

        public LoggerBuilder InstallLogger(ILoggerImpl loggerImpl)
        {
            Ensure.ArgumentIsNotNull(loggerImpl, nameof(loggerImpl));
            _composition.Add(loggerImpl);
            return this;
        }

        public ILogger Build()
        {
            return new DynamicLogger(_composition, _level);
        }

        public void BuildGlobal()
        {
            Logger.GlobalLogger = new DynamicLogger(_composition, _level);
        }
    }
}
