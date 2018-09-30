using System;
using System.Threading.Tasks;

namespace LoggerLite
{
    public abstract class FormattedLoggerBase : LoggerBase
    {
        public Func<string, string, string> Formatter { get; protected set; } =
            (level, message) => $"{DateTime.Now:G} {level}".PadRight(40) + $"{message}{Environment.NewLine}";

        public abstract override bool FlushAuto { get; }

        protected internal abstract void Log(string message);
        protected internal abstract Task LogAsync(string message);

        public override void LogInfo(string message)
        {
            Log(Formatter(InfoName, TrimExcess(message)));
        }

        public override void LogWarning(string warning)
        {
            Log(Formatter(WarningName, TrimExcess(warning)));
        }

        public override void LogError(string error)
        {
            Log(Formatter(ErrorName, TrimExcess(error)));
        }

        public override void LogError(Exception exception)
        {
            Log(Formatter(ErrorName, TrimExcess(exception.ToString())));
        }
        public override async Task LogInfoAsync(string message)
        {
            await LogAsync(Formatter(InfoName, TrimExcess(message)));
        }

        public override async Task LogWarningAsync(string warning)
        {
            await LogAsync(Formatter(WarningName, TrimExcess(warning)));
        }

        public override async Task LogErrorAsync(string error)
        {
            await LogAsync(Formatter(ErrorName, TrimExcess(error)));
        }

        public override async Task LogErrorAsync(Exception exception)
        {
            await LogAsync(Formatter(ErrorName, TrimExcess(exception.ToString())));
        }
    }
}
