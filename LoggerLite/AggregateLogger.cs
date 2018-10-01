using System;
using System.Collections.Generic;
using System.Linq;

namespace LoggerLite
{
    public class AggregateLogger : ILogger
    {
        public AggregateLogger(IEnumerable<ILogger> loggers)
        {
            Loggers.AddRange(loggers);
        }

        public AggregateLogger(params ILogger[] loggers) : this(loggers.AsEnumerable())
        {
        }

        public List<ILogger> Loggers { get; } = new List<ILogger>();

        public void LogInfo(string message)
        {
            Loggers?.ForEach(l => l.LogInfo(message));
        }

        public void LogWarning(string warning)
        {
            Loggers?.ForEach(l => l.LogWarning(warning));
        }

        public void LogError(Exception exception)
        {
            Loggers?.ForEach(l => l.LogError(exception));
        }

        public void LogError(string error)
        {
            Loggers?.ForEach(l => l.LogError(error));
        }

        public void Log(string message, MessageSeverity severity)
        {
            Loggers?.ForEach(l => l.Log(message, severity));
        }

        public bool FlushAuto => Loggers.All(l => l.FlushAuto);

        public bool IsThreadSafe => Loggers.All(l => l.IsThreadSafe);
    }
}