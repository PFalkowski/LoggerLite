using System;
using System.Threading;

namespace LoggerLite;

public class ActiveDebouncer : IDebouncer, IDisposable
{
	private readonly System.Threading.Timer _timer;

	private Action _pendingAction;

	private bool _started;

	private int _debounceMilliseconds = 1000;

	public bool NeedsDisposing => true;

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
			if (value != _debounceMilliseconds)
			{
				_debounceMilliseconds = value;
				_timer.Change(0, _debounceMilliseconds);
			}
		}
	}

	private void StartTimer()
	{
		if (!_started)
		{
			_timer.Change(0, _debounceMilliseconds);
			_started = true;
		}
	}

	private void StopTimer()
	{
		if (_started)
		{
			_timer.Change(-1, _debounceMilliseconds);
			_started = false;
		}
	}

	public ActiveDebouncer()
	{
		_timer = new System.Threading.Timer(Callback, null, -1, DebounceMilliseconds);
	}

	private void Callback(object state)
	{
		_pendingAction?.Invoke();
		StopTimer();
	}

	public void Debounce(Action action)
	{
		_pendingAction = action;
		if (!_started)
		{
			StartTimer();
		}
	}

	public void Dispose()
	{
		if (_started)
		{
			StopTimer();
		}
		_timer?.Dispose();
	}
}
