using Pandora.Common;
using System.IO;

namespace Pandora.Logging
{
    internal class DefaultLogFileNameProvider : ILogFileNameProvider
    {
        private readonly string _logFileName;

        public DefaultLogFileNameProvider(string logFileName) 
        {
            Ensure.ArgumentIsNotNullOrEmpty(logFileName, nameof(logFileName));
            _logFileName = Path.GetFullPath(logFileName);

            var logDir = Path.GetDirectoryName(_logFileName);
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }
        }

        public string GetLogFileName()
        {
            return _logFileName;
        }
    }
}
