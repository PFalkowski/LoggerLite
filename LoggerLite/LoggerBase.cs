using System;

namespace LoggerLite;

public abstract class LoggerBase : ILoggerLite
{
	public const string TruncateInfo = "(truncated)";

	public int MaxSingleMessageLength { get; protected set; } = 1000000;

	public abstract bool FlushAuto { get; }

	public abstract bool IsThreadSafe { get; }

	public int Requests { get; protected set; }

	public int Successes { get; protected set; }

	public int Failures { get; protected set; }

	protected string TrimExcess(string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return input;
		}
		if (input.Length <= MaxSingleMessageLength)
		{
			return input;
		}
		return input.Substring(0, MaxSingleMessageLength - "(truncated)".Length) + "(truncated)";
	}

	public abstract void Log(string message, MessageSeverity severity);

	public void LogInformation(string message)
	{
		LogInfo(message);
	}

	public void LogInfo(string message)
	{
		int requests = Requests + 1;
		Requests = requests;
		try
		{
			Log(message, MessageSeverity.Information);
			requests = Successes + 1;
			Successes = requests;
		}
		catch (Exception)
		{
			requests = Failures + 1;
			Failures = requests;
		}
	}

	public void LogSuccess(string message)
	{
		int requests = Requests + 1;
		Requests = requests;
		try
		{
			Log(message, MessageSeverity.Success);
			requests = Successes + 1;
			Successes = requests;
		}
		catch (Exception)
		{
			requests = Failures + 1;
			Failures = requests;
		}
	}

	public void LogWarning(string warning)
	{
		int requests = Requests + 1;
		Requests = requests;
		try
		{
			Log(warning, MessageSeverity.Warning);
			requests = Successes + 1;
			Successes = requests;
		}
		catch (Exception)
		{
			requests = Failures + 1;
			Failures = requests;
		}
	}

	public void LogError(string error)
	{
		int requests = Requests + 1;
		Requests = requests;
		try
		{
			Log(error, MessageSeverity.Error);
			requests = Successes + 1;
			Successes = requests;
		}
		catch (Exception)
		{
			requests = Failures + 1;
			Failures = requests;
		}
	}

	public void LogError(Exception exception)
	{
		int requests = Requests + 1;
		Requests = requests;
		try
		{
			Log(exception.ToString(), MessageSeverity.Error);
			requests = Successes + 1;
			Successes = requests;
		}
		catch (Exception)
		{
			requests = Failures + 1;
			Failures = requests;
		}
	}

	public void LogError(Exception exception, string description)
	{
		string text = exception.Message.TrimEnd();
		if (!text.EndsWith(":"))
		{
			text += ":";
		}
		LogError(text + " " + description);
	}
}
