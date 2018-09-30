using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;

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

        protected internal override Task LogAsync(string message)
        {
            _buffer.Enqueue(message);
            return new Task(() => 
            _debouncer.Debounce(WriteEnqueued)); // TODO - is it the best way? 
        }

        private void WriteEnqueued()
        {
            var builder = new StringBuilder();
            while (!_buffer.IsEmpty)
            {
                string temp;
                if (_buffer.TryDequeue(out temp))
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