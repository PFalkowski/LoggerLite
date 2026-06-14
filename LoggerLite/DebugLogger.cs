namespace LoggerLite;

public class DebugLogger : FormattedLoggerBase
{
	public override bool FlushAuto => true;

	public override bool IsThreadSafe => true;

	protected internal override void Log(string message)
	{
	}
}
