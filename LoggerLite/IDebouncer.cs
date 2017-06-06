using System;

namespace LoggerLite
{
    public interface IDebouncer
    {
        int DebounceMilliseconds { get; set; }
        void Debounce(Action action);
    }
}