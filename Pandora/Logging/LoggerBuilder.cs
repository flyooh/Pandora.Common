using Pandora.Common;

namespace Pandora.Logging
{
    public class LoggerBuilder : ILoggerBuilder
    {
        private readonly CompositionLoggerImpl _composition;

        private ILogTimeFormatter _timeFormatter = new DefaultLogTimeFormatter();

        private LogLevel _logLevel = LogLevel.Debug;

        public LoggerBuilder()
        {
            _composition = new CompositionLoggerImpl();
        }

        public LoggerBuilder SetLogLevel(LogLevel logLevel)
        {
            _logLevel = logLevel;
            return this;
        }

        public LoggerBuilder InstallTimeFormatter(ILogTimeFormatter timeFormatter)
        { 
            _timeFormatter = timeFormatter;
            return this;
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
            return new DynamicLogger(_composition, _timeFormatter, _logLevel);
        }

        public void BuildGlobal()
        {
            Logger.GlobalLogger = new DynamicLogger(_composition, _timeFormatter, _logLevel);
        }
    }
}
