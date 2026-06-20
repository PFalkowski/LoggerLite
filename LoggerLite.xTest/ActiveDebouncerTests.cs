using System;
using System.Diagnostics;
using System.Threading;
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
        public void DebouncerDebounces()
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
                Assert.True(counter < numRepeats);
            }
            else if (stopwatch.ElapsedMilliseconds * 2 < debounceMs)
            {
                Assert.True(counter > 0);
            }
        }

        [Fact]
        public void DisposeStopsStartedTimer()
        {
            // Volatile flags (no IDisposable) so the background timer thread can never
            // touch a synchronization primitive that this method has already disposed.
            var callbackRunning = false;
            var release = false;
            // Long period so the single due-time-0 tick is the only one in the test window.
            var tested = new ActiveDebouncer { DebounceMilliseconds = 10000 };

            tested.Debounce(() =>
            {
                Volatile.Write(ref callbackRunning, true);
                SpinWait.SpinUntil(() => Volatile.Read(ref release), TimeSpan.FromSeconds(5));
            });

            // Block until the debounced callback is actually running, so the timer is
            // still started when Dispose() runs - exercising the StopTimer + Dispose path.
            Assert.True(SpinWait.SpinUntil(() => Volatile.Read(ref callbackRunning), TimeSpan.FromSeconds(5)),
                "debounced callback never started");

            tested.Dispose();

            Volatile.Write(ref release, true);
        }
    }
}
