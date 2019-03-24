using System;
using System.Threading;

namespace LoggerLite
{
    public class ActiveDebouncer : IDebouncer, IDisposable
    {
        private readonly Timer _timer;
        private Action _pendingAction;
        private bool _started;

        public bool NeedsDisposing => true;

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
                _timer.Change(Timeout.Infinite, _debounceMilliseconds);
                _started = false;
            }
        }
        public ActiveDebouncer()
        {
            var test = new System.Timers.Timer();
            _timer = new Timer(Callback, null, Timeout.Infinite, DebounceMilliseconds);
        }

        private void Callback(object state)
        {
            _pendingAction?.Invoke();
            StopTimer();
        }

        private int _debounceMilliseconds = 1000;
        public int DebounceMilliseconds
        {
            get => _debounceMilliseconds;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
                if (value == _debounceMilliseconds) return;
                _debounceMilliseconds = value;
                _timer.Change(0, _debounceMilliseconds);
            }
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
            if (_started) { StopTimer(); }
            _timer?.Dispose();
        }
    }
}
