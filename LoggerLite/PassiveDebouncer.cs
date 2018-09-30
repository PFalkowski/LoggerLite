using System;
using System.Diagnostics;

namespace LoggerLite
{
    public sealed class PassiveDebouncer : IDebouncer
    {
        private readonly Stopwatch _watch = new Stopwatch();

        private int _debounceMilliseconds = 1000;
        private bool _started;

        public int DebounceMilliseconds
        {
            get => _debounceMilliseconds;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
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
            // Nothing to do here
        }
    }
}