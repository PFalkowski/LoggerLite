using System;
using System.Threading.Tasks;

namespace LoggerLite
{
    public abstract class LoggerBase : ILogger
    {
        public const string ErrorName = "Error";
        public const string InfoName = "Info";
        public const string WarningName = "Warning";
        public const string TruncateInfo = "(truncated)";

        public int MaxSingleMessageLength { get; protected set; } = 1000 * 1000;
        public abstract bool FlushAuto { get; }

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
        public abstract Task LogInfoAsync(string message);
        public abstract Task LogWarningAsync(string warning);
        public abstract Task LogErrorAsync(Exception exception);
        public abstract Task LogErrorAsync(string error);
    }
}