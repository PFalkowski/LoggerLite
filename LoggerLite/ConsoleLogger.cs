using System;

namespace LoggerLite
{
    public sealed class ConsoleLogger : FormattedLoggerBase
    {
        private readonly object _syncRoot = new object();
        public ConsoleColor ErrorColor { get; set; } = ConsoleColor.Red;
        public ConsoleColor InfoColor { get; set; } = ConsoleColor.Gray;
        public ConsoleColor WarningColor { get; set; } = ConsoleColor.Yellow;

        public override bool FlushAuto => true;
        public override bool IsThreadSafe => true;

        public ConsoleLogger()
        {
            Formatter = (level, message) => $"{message}{Environment.NewLine}";
        }
        protected internal override void Log(string message)
        {
            Console.Write(message);
        }

        protected internal override void Log(string message, MessageSeverity severity)
        {
            lock (_syncRoot)
            {
                var prevColor = Console.ForegroundColor;
                try
                {
                    switch (severity)
                    {
                        case MessageSeverity.Information:
                            Console.ForegroundColor = InfoColor;
                            break;
                        case MessageSeverity.Warning:
                            Console.ForegroundColor = WarningColor;
                            break;
                        case MessageSeverity.Error:
                            Console.ForegroundColor = ErrorColor;
                            break;
                        default:
                            break;
                    }
                    Log(Formatter(severity.ToString(), TrimExcess(message)));
                    ++Sucesses;
                }
                finally
                {
                    Console.ForegroundColor = prevColor;
                }
            }
        }
    }
}