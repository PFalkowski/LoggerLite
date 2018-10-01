using System;
using System.Diagnostics;
using Xunit;

namespace LoggerLite.xTest
{
    public class ActiveDebouncerTest
    {
        [Fact]
        public void DebouncerDefault()
        {
            var testedDebouncer = new ActiveDebouncer { DebounceMilliseconds = 100 };
            Assert.Equal(100, testedDebouncer.DebounceMilliseconds);
            testedDebouncer = new ActiveDebouncer();
            Assert.Equal(1000, testedDebouncer.DebounceMilliseconds);
        }

        [Fact]
        public void NeedsDisposingAlwaysReturnsTrue()
        {
            var testedDebouncer = new ActiveDebouncer();
            Assert.True(testedDebouncer.NeedsDisposing);
        }

        [Fact]
        public void DebouncerSetterThrowsArgumentException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ActiveDebouncer { DebounceMilliseconds = -1 });
        }

        [Fact]
        public void Debouncer()
        {
            const int numRepeats = 300;
            const int debounceMs = 1;
            var counter = 0;
            var testedDebouncer = new ActiveDebouncer { DebounceMilliseconds = debounceMs };
            void actionToDebounce() { ++counter; }
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < numRepeats; ++i)
            {
                testedDebouncer.Debounce(actionToDebounce);
            }
            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds < debounceMs)
            {
                Assert.Equal(0, counter);
            }
            else if (stopwatch.ElapsedMilliseconds * 2 < debounceMs)
            {
                Assert.True(counter > 0);
            }
        }
    }
}
