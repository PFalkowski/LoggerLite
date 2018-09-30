using System;

namespace LoggerLite
{
    public abstract class FormattedLoggerBase : LoggerBase
    {
        public Func<string, string, string> Formatter { get; protected set; } =
            (level, message) => $"{DateTime.Now:G} {level}".PadRight(40) + $"{message}{Environment.NewLine}";

        public abstract override bool FlushAuto { get; }

        protected internal abstract void Log(string message);

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
    }
}
