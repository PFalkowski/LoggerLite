using System;

namespace LoggerLite
{
    public interface IDebouncer : IDisposable
    {
        int DebounceMilliseconds { get; set; }
        void Debounce(Action action);
    }
}