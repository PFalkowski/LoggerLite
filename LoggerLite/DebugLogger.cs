using System.Diagnostics;
using System.Threading.Tasks;

namespace LoggerLite
{
    public sealed class DebugLogger : FormattedLoggerBase
    {
        public override bool FlushAuto => true;

        protected internal override void Log(string message)
        {
            Debug.WriteLine(message);
        }
        protected internal override Task LogAsync(string message)
        {
            return new Task(() => Debug.WriteLine(message));  // TODO - is it the best way? 
        }
    }
}