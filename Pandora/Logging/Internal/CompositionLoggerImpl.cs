using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pandora.Logging
{
    internal class CompositionLoggerImpl : ILoggerImpl
    {
        private List<ILoggerImpl> loggers;

        public CompositionLoggerImpl()
        {
            this.loggers = new List<ILoggerImpl>();
        }

        public void Add(ILoggerImpl writer)
        {
            lock (loggers)
            {
                this.loggers.Add(writer);
            }
        }

        public void WriteDebug(string line)
        {
            Parallel.ForEach(this.loggers, (writer) => writer.WriteDebug(line));
        }

        public void WriteTrace(string line)
        {
            Parallel.ForEach(this.loggers, (writer) => writer.WriteTrace(line));
        }

        public void WriteError(string line)
        {
            Parallel.ForEach(this.loggers, (writer) => writer.WriteError(line));
        }

        public void WriteInfo(string line)
        {
            Parallel.ForEach(this.loggers, (writer) => writer.WriteInfo(line));
        }
    }
}
