using System;
using System.Threading.Tasks;

namespace LoggerLite
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogWarning(string warning);
        void LogError(Exception exception);
        void LogError(string error);
        Task LogInfoAsync(string message);
        Task LogWarningAsync(string warning);
        Task LogErrorAsync(Exception exception);
        Task LogErrorAsync(string error);
        /// <summary>
        /// Indicates, whether the logger will automatically update underlying object/stream, or a special call to some method (save/flush) is required.
        /// </summary>
        bool FlushAuto { get; }
    }
}
