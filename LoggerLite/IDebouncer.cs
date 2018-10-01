using System;

namespace LoggerLite
{
    public interface IDebouncer : IDisposable
    {
        int DebounceMilliseconds { get; set; }
        bool NeedsDisposing { get; }
        void Debounce(Action action);
    }
}