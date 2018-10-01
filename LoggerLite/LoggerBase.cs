using System;

namespace LoggerLite
{
    public abstract class LoggerBase : ILogger
    {
        public const string ErrorName = "Error";
        public const string InfoName = "Information";
        public const string WarningName = "Warning";
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

        public abstract void LogInfo(string message);
        public abstract void LogWarning(string warning);
        public abstract void LogError(Exception exception);
        public abstract void LogError(string error);

        public void Log(string message, MessageSeverity severity)
        {
            switch (severity)
            {
                case MessageSeverity.Information:
                    LogInfo(message);
                    break;
                case MessageSeverity.Warning:
                    LogWarning(message);
                    break;
                case MessageSeverity.Error:
                    LogError(message);
                    break;
                default:
                    break;
            }
        }
    }
}