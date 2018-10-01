using System;

namespace LoggerLite
{
    public interface ILogger
    {
        void Log(string message, MessageSeverity severity);
        void LogInfo(string message);
        void LogWarning(string warning);
        void LogError(Exception exception);
        void LogError(string error);
        /// <summary>
        /// Indicates, whether the logger will automatically update underlying object/stream, or it requires a call to Flush() method
        /// </summary>
        bool FlushAuto { get; }
        bool IsThreadSafe { get; }
    }
}
