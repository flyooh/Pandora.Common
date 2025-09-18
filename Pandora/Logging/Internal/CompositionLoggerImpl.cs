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

        public void Write(string line)
        {
            Parallel.ForEach(this.loggers, (writer) => writer.Write(line));
        }
    }
}
