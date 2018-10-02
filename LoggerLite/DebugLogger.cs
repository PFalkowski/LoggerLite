using System.Diagnostics;

namespace LoggerLite
{
    public sealed class DebugLogger : FormattedLoggerBase
    {
        public override bool FlushAuto => true;

        public override bool IsThreadSafe => true;

        public override int Requests { get; protected set; }
        public override int Sucesses { get; protected set; }
        public override int Failures { get; protected set; }

        protected internal override void Log(string message)
        {
            ++Requests;
            try
            {
                Debug.WriteLine(message);
                ++Sucesses;
            }
            catch (System.Exception)
            {
                ++Failures;
            }
        }
    }
}