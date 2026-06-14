using System;
using System.Diagnostics;

namespace LoggerLite;

public class PassiveDebouncer : IDebouncer, IDisposable
{
	private readonly Stopwatch _watch = new Stopwatch();

	private int _debounceMilliseconds = 1000;

	private bool _started;

	public bool NeedsDisposing => false;

	public int DebounceMilliseconds
	{
		get
		{
			return _debounceMilliseconds;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			_debounceMilliseconds = value;
		}
	}

	public void Debounce(Action action)
	{
		if (!_started)
		{
			action();
			_started = true;
			_watch.Restart();
		}
		else if (_started && _watch.ElapsedMilliseconds > DebounceMilliseconds)
		{
			action();
			_started = false;
			_watch.Reset();
		}
	}

	public void Dispose()
	{
	}
}
