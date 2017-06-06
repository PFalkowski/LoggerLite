using System;

namespace LoggerLite
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogWarning(string warning);
        void LogError(Exception exception);
        void LogError(string error);
        /// <summary>
        /// Indicates, whether the logger will automatically update underlying object/stream, or a special call to some method )ex. Save) is required.
        /// </summary>
        bool FlushAuto { get; }
    }
}
