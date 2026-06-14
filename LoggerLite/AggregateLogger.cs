using System;
using System.Collections.Generic;
using System.Linq;

namespace LoggerLite;

public class AggregateLogger : LoggerBase
{
	public override bool FlushAuto => Loggers.All((ILoggerLite l) => l.FlushAuto);

	public override bool IsThreadSafe => Loggers.All((ILoggerLite l) => l.IsThreadSafe);

	public List<ILoggerLite> Loggers { get; } = new List<ILoggerLite>();

	public AggregateLogger(IEnumerable<ILoggerLite> loggers)
	{
		Loggers.AddRange(loggers);
	}

	public AggregateLogger(params ILoggerLite[] loggers)
		: this(loggers.AsEnumerable())
	{
	}

	public override void Log(string message, MessageSeverity severity)
	{
		List<Exception> list = new List<Exception>();
		foreach (ILoggerLite logger in Loggers)
		{
			try
			{
				switch (severity)
				{
				case MessageSeverity.Success:
					logger.LogSuccess(message);
					break;
				case MessageSeverity.Information:
					logger.LogInfo(message);
					break;
				case MessageSeverity.Warning:
					logger.LogWarning(message);
					break;
				case MessageSeverity.Error:
					logger.LogError(message);
					break;
				}
			}
			catch (Exception item)
			{
				list.Add(item);
			}
		}
		if (list.Any())
		{
			throw new AggregateException(list);
		}
	}
}
