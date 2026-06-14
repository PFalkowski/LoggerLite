using System;
using System.Collections.Concurrent;
using System.Text;

namespace LoggerLite;

public class QueuedLoggerWrapper : FormattedLoggerBase, IDisposable
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
		}
	}

	private void WriteEnqueued()
	{
		StringBuilder stringBuilder = new StringBuilder();
		while (!_buffer.IsEmpty)
		{
			if (_buffer.TryDequeue(out var result))
			{
				stringBuilder.Append(result);
			}
		}
		if (stringBuilder.Length > 0)
		{
			_logger.Log(stringBuilder.ToString());
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
