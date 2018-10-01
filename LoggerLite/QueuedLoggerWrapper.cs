﻿using System;
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
        //TODO: add for all file loggers
        public int LogRequests { get; private set; } = 0;
        //TODO: add for all file loggers
        public int FailedDequeues { get; private set; } = 0;

        public QueuedLoggerWrapper(FormattedLoggerBase logger, IDebouncer debouncer)
        {
            _logger = logger;
            _debouncer = debouncer;
        }


        protected internal override void Log(string message)
        {
            lock (_syncRoot)
            {
                ++LogRequests;
                _buffer.Enqueue(message);
                _debouncer.Debounce(WriteEnqueued);
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
                else
                {
                    ++FailedDequeues;
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