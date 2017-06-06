using System.Diagnostics;

namespace LoggerLite
{
    public sealed class DebugLogger : FormattedLoggerBase
    {
        public override bool FlushAuto => true;

        protected internal override void Log(string message)
        {
            Debug.WriteLine(message);
        }
    }
}