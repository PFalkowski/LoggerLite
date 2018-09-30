using System.Collections.Concurrent;
using System.Text;

namespace LoggerLite
{
    public sealed class QueuedLoggerWrapper : FormattedLoggerBase
    {
        private readonly ConcurrentQueue<string> _buffer = new ConcurrentQueue<string>();
        private readonly IDebouncer _debouncer;
        private readonly FormattedLoggerBase _logger;


        public QueuedLoggerWrapper(FormattedLoggerBase logger, IDebouncer debouncer)
        {
            _logger = logger;
            _debouncer = debouncer;
        }

        public override bool FlushAuto => _logger.FlushAuto;

        protected internal override void Log(string message)
        {
            _buffer.Enqueue(message);
            _debouncer.Debounce(WriteEnqueued);
        }

        private void WriteEnqueued()
        {
            var builder = new StringBuilder();
            while (!_buffer.IsEmpty)
            {
                if (_buffer.TryDequeue(out var temp))
                {
                    builder.Append(temp);
                }
            }
            if (builder.Length > 0)
            {
                _logger.Log(builder.ToString());
            }
        }

        public void Flush()
        {
            WriteEnqueued();
        }
    }
}