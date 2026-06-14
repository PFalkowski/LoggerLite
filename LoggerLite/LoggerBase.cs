using System;

namespace LoggerLite
{
    public abstract class LoggerBase : ILogger
    {
        public const string TruncateInfo = "(truncated)";

        public int MaxSingleMessageLength { get; protected set; } = 1000 * 1000;

        public abstract bool FlushAuto { get; }
        public abstract bool IsThreadSafe { get; }

        public int Requests { get; protected set; }
        public int Successes { get; protected set; }

        [Obsolete("Misspelled. Use Successes instead. Will be removed in a future major version.")]
        public int Sucesses => Successes;

        public int Failures { get; protected set; }

        protected string TrimExcess(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input.Length > MaxSingleMessageLength
                ? input.Substring(0, MaxSingleMessageLength - TruncateInfo.Length) + TruncateInfo
                : input;
        }

        protected internal abstract void Log(string message, MessageSeverity severity);

        public void LogInfo(string message)
        {
            ++Requests;
            try
            {
                Log(message, MessageSeverity.Information);
                ++Successes;
            }
            catch (Exception)
            {
                ++Failures;
            }
        }
        public void LogWarning(string warning)
        {
            ++Requests;
            try
            {
                Log(warning, MessageSeverity.Warning);
                ++Successes;
            }
            catch (Exception)
            {
                ++Failures;
            }
        }
        public void LogError(string error)
        {
            ++Requests;
            try
            {
                Log(error, MessageSeverity.Error);
                ++Successes;
            }
            catch (Exception)
            {
                ++Failures;
            }
        }
        public void LogError(Exception exception)
        {
            ++Requests;
            try
            {
                Log(exception.ToString(), MessageSeverity.Error);
                ++Successes;
            }
            catch (Exception)
            {
                ++Failures;
            }
        }

    }
}