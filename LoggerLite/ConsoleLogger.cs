using System;
using System.Threading.Tasks;

namespace LoggerLite
{
    public sealed class ConsoleLogger : FormattedLoggerBase
    {
        private readonly object _syncRoot = new object();
        public ConsoleColor ErrorColor { get; set; } = ConsoleColor.Red;
        public ConsoleColor InfoColor { get; set; } = ConsoleColor.White;
        public ConsoleColor WarningColor { get; set; } = ConsoleColor.DarkYellow;

        public override bool FlushAuto => true;

        protected internal override void Log(string message)
        {
            Console.Write(message);
        }

        protected internal override Task LogAsync(string message) // TODO - is it best way? 
        {
            return new Task(() =>
            Console.Write(message));
        }

        public override void LogError(string error)
        {
            lock (_syncRoot)
            {
                var prevColor = Console.ForegroundColor;
                try
                {
                    Console.ForegroundColor = ErrorColor;
                    base.LogError(error);
                }
                finally
                {
                    Console.ForegroundColor = prevColor;
                }
            }
        }

        public override void LogError(Exception exception)
        {
            lock (_syncRoot)
            {
                var prevColor = Console.ForegroundColor;
                try
                {
                    Console.ForegroundColor = ErrorColor;
                    base.LogError(exception);
                }
                finally
                {
                    Console.ForegroundColor = prevColor;
                }
            }
        }

        public override void LogInfo(string message)
        {
            lock (_syncRoot)
            {
                var prevColor = Console.ForegroundColor;
                try
                {
                    Console.ForegroundColor = InfoColor;
                    base.LogInfo(message);
                }
                finally
                {
                    Console.ForegroundColor = prevColor;
                }
            }
        }

        public override void LogWarning(string warning)
        {
            lock (_syncRoot)
            {
                var prevColor = Console.ForegroundColor;
                try
                {
                    Console.ForegroundColor = WarningColor;
                    base.LogWarning(warning);
                }
                finally
                {
                    Console.ForegroundColor = prevColor;
                }
            }
        }
    }
}