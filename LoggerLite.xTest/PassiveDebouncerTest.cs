using System;
using System.Diagnostics;
using Xunit;

namespace LoggerLite.xTest
{
    public class PassiveDebouncerTest
    {
        [Fact]
        public void DebouncerDefault()
        {
            var testedDebouncer = new PassiveDebouncer { DebounceMilliseconds = 100 };
            Assert.Equal(100, testedDebouncer.DebounceMilliseconds);
            testedDebouncer = new PassiveDebouncer ();
            Assert.Equal(1000, testedDebouncer.DebounceMilliseconds);
        }
        [Fact]
        public void NeedsDisposingAlwaysReturnsFalse()
        {
            var testedDebouncer = new PassiveDebouncer();
            Assert.False(testedDebouncer.NeedsDisposing);
        }
        [Fact]
        public void DebouncerSetterThrowsArgumentException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PassiveDebouncer { DebounceMilliseconds = -1 });
        }
        [Fact]
        public void Debouncer()
        {
            const int numRepeats = 300;
            const int debounceMs = 100;
            var counter = 0;
            var testedDebouncer = new PassiveDebouncer { DebounceMilliseconds = debounceMs };
            Assert.Equal(debounceMs, testedDebouncer.DebounceMilliseconds);
            void actionToDebounce() { ++counter; }
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < numRepeats; ++i)
            {
                testedDebouncer.Debounce(actionToDebounce);
            }
            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds < debounceMs)
            {
                Assert.Equal(1, counter);
            }
        }
    }
}
