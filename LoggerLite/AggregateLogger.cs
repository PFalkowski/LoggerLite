using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public Task LogInfoAsync(string message)
        {
            var tasks = new List<Task>(Loggers.Capacity);
            foreach (var logger in Loggers)
            {
                tasks.Add(logger.LogInfoAsync(message));
            }
            return Task.WhenAll(tasks);
        }

        public Task LogWarningAsync(string warning)
        {
            var tasks = new List<Task>(Loggers.Capacity);
            foreach (var logger in Loggers)
            {
                tasks.Add(logger.LogWarningAsync(warning));
            }
            return Task.WhenAll(tasks);
        }

        public Task LogErrorAsync(Exception exception)
        {
            var tasks = new List<Task>(Loggers.Capacity);
            foreach (var logger in Loggers)
            {
                tasks.Add(logger.LogErrorAsync(exception));
            }
            return Task.WhenAll(tasks);
        }

        public Task LogErrorAsync(string error)
        {
            var tasks = new List<Task>(Loggers.Capacity);
            foreach (var logger in Loggers)
            {
                tasks.Add(logger.LogErrorAsync(error));
            }
            return Task.WhenAll(tasks);
        }

        public void Log(string message, MessageSeverity severity)
        {
            Loggers?.ForEach(l => l.Log(message, severity));
        }

        public bool FlushAuto => Loggers.All(l => l.FlushAuto);
    }
}