using System;
using System.Collections.Concurrent;
using System.Text;

namespace LoggerLite
{
    // TODO: split debouncing logger from queued
    public sealed class QueuedLoggerWrapper : FormattedLoggerBase, IDisposable
    {
        private readonly object _syncRoot = new object();

        private readonly ConcurrentQueue<string> _buffer = new ConcurrentQueue<string>();
        private readonly IDebouncer _debouncer;
        private readonly FormattedLoggerBase _logger;

        public override bool FlushAuto => _logger.FlushAuto;
        public override bool IsThreadSafe => true;

        public QueuedLoggerWrapper(FormattedLoggerBase logger, IDebouncer debouncer)
        {
            _logger = logger;
            _debouncer = debouncer;
        }


        protected internal override void Log(string message)
        {
            lock (_syncRoot)
            {
                _buffer.Enqueue(message);
                _debouncer.Debounce(WriteEnqueued);
                ++Sucesses;
            }
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

        public void Dispose()
        {
            _debouncer?.Dispose();
        }
    }
}