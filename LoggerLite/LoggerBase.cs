using System;

namespace LoggerLite
{
    public abstract class LoggerBase : ILogger
    {
        public const string TruncateInfo = "(truncated)";

        public int MaxSingleMessageLength { get; protected set; } = 1000 * 1000;

        public abstract bool FlushAuto { get; }
        public abstract bool IsThreadSafe { get; }

        protected string TrimExcess(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input.Length > MaxSingleMessageLength
                ? input.Substring(0, MaxSingleMessageLength - TruncateInfo.Length) + TruncateInfo
                : input;
        }

        public abstract void Log(string message, MessageSeverity severity);

        public void LogInfo(string message)
        {
            Log(message, MessageSeverity.Information);
        }
        public void LogWarning(string warning)
        {
            Log(warning, MessageSeverity.Warning);
        }
        public void LogError(string error)
        {
            Log(error, MessageSeverity.Error);
        }
        public void LogError(Exception exception)
        {
            Log(exception.ToString(), MessageSeverity.Error);
        }

    }
}