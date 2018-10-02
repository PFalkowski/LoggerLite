using System;
using System.Collections.Generic;
using System.Linq;

namespace LoggerLite
{
    public class AggregateLogger : LoggerBase
    {
        public override bool FlushAuto => Loggers.All(l => l.FlushAuto);
        public override bool IsThreadSafe => Loggers.All(l => l.IsThreadSafe);

        public override int Requests { get; protected set; }
        public override int Sucesses { get; protected set; }
        public override int Failures { get; protected set; }

        public List<ILogger> Loggers { get; } = new List<ILogger>();

        public AggregateLogger(IEnumerable<ILogger> loggers)
        {
            Loggers.AddRange(loggers);
        }

        public AggregateLogger(params ILogger[] loggers) : this(loggers.AsEnumerable()) { }

        public override void Log(string message, MessageSeverity severity)
        {
            var errorOccured = false;
            ++Requests;
            foreach (var logger in Loggers)
            {
                try
                {
                    logger.Log(message, severity);
                }
                catch (Exception)
                {
                    errorOccured = true;
                }
            }
            if (!errorOccured)
            { ++Sucesses; }
            else
            { ++Failures; }
        }
    }
}