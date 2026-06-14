using System;

namespace LoggerLite;

public class ConsoleLogger : FormattedLoggerBase
{
	private readonly object _syncRoot = new object();

	public ConsoleColor SuccessColor { get; set; } = ConsoleColor.Green;

	public ConsoleColor ErrorColor { get; set; } = ConsoleColor.Red;

	public ConsoleColor InfoColor { get; set; } = ConsoleColor.Gray;

	public ConsoleColor WarningColor { get; set; } = ConsoleColor.Yellow;

	public override bool FlushAuto => true;

	public override bool IsThreadSafe => true;

	public ConsoleLogger()
	{
		base.Formatter = (string level, string message) => message + Environment.NewLine;
	}

	protected internal override void Log(string message)
	{
		Console.Write(message);
	}

	public override void Log(string message, MessageSeverity severity)
	{
		lock (_syncRoot)
		{
			ConsoleColor foregroundColor = Console.ForegroundColor;
			try
			{
				switch (severity)
				{
				case MessageSeverity.Success:
					Console.ForegroundColor = SuccessColor;
					break;
				case MessageSeverity.Information:
					Console.ForegroundColor = InfoColor;
					break;
				case MessageSeverity.Warning:
					Console.ForegroundColor = WarningColor;
					break;
				case MessageSeverity.Error:
					Console.ForegroundColor = ErrorColor;
					break;
				}
				Log(base.Formatter(severity.ToString(), TrimExcess(message)));
			}
			finally
			{
				Console.ForegroundColor = foregroundColor;
			}
		}
	}
}
