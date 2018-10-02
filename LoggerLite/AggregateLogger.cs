using System;
using System.Collections.Generic;
using System.Linq;

namespace LoggerLite
{
    public class AggregateLogger : LoggerBase
    {
        public override bool FlushAuto => Loggers.All(l => l.FlushAuto);
        public override bool IsThreadSafe => Loggers.All(l => l.IsThreadSafe);

        public List<ILogger> Loggers { get; } = new List<ILogger>();

        public AggregateLogger(IEnumerable<ILogger> loggers)
        {
            Loggers.AddRange(loggers);
        }

        public AggregateLogger(params ILogger[] loggers) : this(loggers.AsEnumerable()) { }

        protected internal override void Log(string message, MessageSeverity severity)
        {
            var exceptions = new List<Exception>();
            foreach (var logger in Loggers)
            {
                try
                {
                    switch (severity)
                    {
                        case MessageSeverity.Information:
                            logger.LogInfo(message);
                            break;
                        case MessageSeverity.Warning:
                            logger.LogWarning(message);
                            break;
                        case MessageSeverity.Error:
                            logger.LogError(message);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            if (exceptions.Any()) throw new AggregateException(exceptions);
        }
    }
}