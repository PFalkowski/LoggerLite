using System;

namespace LoggerLite
{
    public abstract class FormattedLoggerBase : LoggerBase
    {
        public Func<string, string, string> Formatter { get; protected set; } =
            (level, message) => $"{DateTime.Now:G} {level}".PadRight(40) + $"{message}{Environment.NewLine}";

        public abstract override bool FlushAuto { get; }

        protected internal abstract void Log(string message);
        protected internal override void Log(string message, MessageSeverity severity)
        {
            Log(Formatter(severity.ToString(), TrimExcess(message)));
        }
    }
}
