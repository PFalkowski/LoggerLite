using System;

namespace LoggerLite;

public interface ILoggerLite
{
	bool FlushAuto { get; }

	bool IsThreadSafe { get; }

	int Requests { get; }

	int Successes { get; }

	int Failures { get; }

	void LogSuccess(string message);

	void LogInfo(string message);

	void LogInformation(string message);

	void LogWarning(string warning);

	void LogError(Exception exception);

	void LogError(string error);

	void LogError(Exception exception, string description);

	void Log(string message, MessageSeverity severity);
}
