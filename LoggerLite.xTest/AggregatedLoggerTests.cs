using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace LoggerLite.xTest
{
    public class AggregatedLoggerTests
    {
        [Fact]
        public void AggregatedLoggerCreateTest()
        {
            var mockLogger1 = new Mock<ILoggerLite>();
            var mockLogger2 = new Mock<ILoggerLite>();
            var tested = new AggregateLogger(mockLogger1.Object, mockLogger2.Object);
            Assert.Equal(2, tested.Loggers.Count);
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void FlushAutoReturnsWhatAllLogersDo(bool flushesAuto)
        {
            var mockLogger1 = new Mock<ILoggerLite>();
            var mockLogger2 = new Mock<ILoggerLite>();
            var mockLogger3 = new Mock<ILoggerLite>();
            mockLogger1.SetupGet(m => m.FlushAuto).Returns(flushesAuto);
            mockLogger2.SetupGet(m => m.FlushAuto).Returns(flushesAuto);
            mockLogger3.SetupGet(m => m.FlushAuto).Returns(flushesAuto);
            var tested = new AggregateLogger(mockLogger1.Object, mockLogger2.Object, mockLogger3.Object);
            Assert.Equal(flushesAuto, tested.FlushAuto);
        }
        [Fact]
        public void FlushAutoReturnsFalseWhenAnyLoggerDoesent()
        {
            var mockLogger1 = new Mock<ILoggerLite>();
            var mockLogger2 = new Mock<ILoggerLite>();
            var mockLogger3 = new Mock<ILoggerLite>();
            mockLogger1.SetupGet(m => m.FlushAuto).Returns(true);
            mockLogger2.SetupGet(m => m.FlushAuto).Returns(true);
            mockLogger3.SetupGet(m => m.FlushAuto).Returns(false);
            var tested = new AggregateLogger(mockLogger1.Object, mockLogger2.Object, mockLogger3.Object);
            Assert.False(tested.FlushAuto);
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsThreadSafeReturnsWhatAllLogersDo(bool isThreadSafe)
        {
            var mockLogger1 = new Mock<ILoggerLite>();
            var mockLogger2 = new Mock<ILoggerLite>();
            var mockLogger3 = new Mock<ILoggerLite>();
            mockLogger1.SetupGet(m => m.IsThreadSafe).Returns(isThreadSafe);
            mockLogger2.SetupGet(m => m.IsThreadSafe).Returns(isThreadSafe);
            mockLogger3.SetupGet(m => m.IsThreadSafe).Returns(isThreadSafe);
            var tested = new AggregateLogger(mockLogger1.Object, mockLogger2.Object, mockLogger3.Object);
            Assert.Equal(isThreadSafe, tested.IsThreadSafe);
        }
        [Fact]
        public void IsThreadSafeReturnsFalseWhenAnyLoggerIsnt()
        {
            var mockLogger1 = new Mock<ILoggerLite>();
            var mockLogger2 = new Mock<ILoggerLite>();
            var mockLogger3 = new Mock<ILoggerLite>();
            mockLogger1.SetupGet(m => m.IsThreadSafe).Returns(true);
            mockLogger2.SetupGet(m => m.IsThreadSafe).Returns(true);
            mockLogger3.SetupGet(m => m.IsThreadSafe).Returns(false);
            var tested = new AggregateLogger(mockLogger1.Object, mockLogger2.Object, mockLogger3.Object);
            Assert.False(tested.IsThreadSafe);
        }
        [Fact]
        public void AggregatedLoggerCreateTest2()
        {
            var mockLogger1 = new Mock<ILoggerLite>();
            var mockLogger2 = new Mock<ILoggerLite>();
            var tested = new AggregateLogger(new List<ILoggerLite> { mockLogger1.Object, mockLogger2.Object });
            Assert.Equal(2, tested.Loggers.Count);
        }
        [Fact]
        public void AggregatedLoggerDependantLoggerLogInfoTest()
        {
            const int repeats = 10;
            var calls = 0;
            var mockLogger1 = new Mock<ILoggerLite>();
            var mockLogger2 = new Mock<ILoggerLite>();
            mockLogger1.Setup(l => l.LogInfo(It.IsAny<string>())).Callback(() => ++calls);
            mockLogger2.Setup(l => l.LogInfo(It.IsAny<string>())).Callback(() => ++calls);
            var tested = new AggregateLogger(new List<ILoggerLite> { mockLogger1.Object, mockLogger2.Object });
            for (var i = 0; i < repeats; )
            {
                Assert.Equal(i, calls);
                if (i % 2 == 0) 
                    mockLogger1.Object.LogInfo(string.Empty);
                else
                    mockLogger2.Object.LogInfo(string.Empty);
                Assert.Equal(++i, calls);
            }
        }
        [Fact]
        public void AggregatedLoggerDependantLoggerLogWarningTest()
        {
            const int repeats = 10;
            var calls = 0;
            var mockLogger1 = new Mock<ILoggerLite>();
            var mockLogger2 = new Mock<ILoggerLite>();
            mockLogger1.Setup(l => l.LogWarning(It.IsAny<string>())).Callback(() => ++calls);
            mockLogger2.Setup(l => l.LogWarning(It.IsAny<string>())).Callback(() => ++calls);
            var tested = new AggregateLogger(new List<ILoggerLite> { mockLogger1.Object, mockLogger2.Object });
            for (var i = 0; i < repeats;)
            {
                Assert.Equal(i, calls);
                if (i % 2 == 0)
                    mockLogger1.Object.LogWarning(string.Empty);
                else
                    mockLogger2.Object.LogWarning(string.Empty);
                Assert.Equal(++i, calls);
            }
        }
        [Fact]
        public void AggregatedLoggerDependantLoggerLogErrorTest()
        {
            const int repeats = 10;
            var calls = 0;
            var mockLogger1 = new Mock<ILoggerLite>();
            var mockLogger2 = new Mock<ILoggerLite>();
            mockLogger1.Setup(l => l.LogError(It.IsAny<string>())).Callback(() => ++calls);
            mockLogger2.Setup(l => l.LogError(It.IsAny<string>())).Callback(() => ++calls);
            var tested = new AggregateLogger(new List<ILoggerLite> { mockLogger1.Object, mockLogger2.Object });
            for (var i = 0; i < repeats;)
            {
                Assert.Equal(i, calls);
                if (i % 2 == 0)
                    mockLogger1.Object.LogError(string.Empty);
                else
                    mockLogger2.Object.LogError(string.Empty);
                Assert.Equal(++i, calls);
            }
        }
        [Fact]
        public void AggregatedLoggerDependantLoggerLogErrorTest2()
        {
            const int repeats = 10;
            var calls = 0;
            var mockLogger1 = new Mock<ILoggerLite>();
            var mockLogger2 = new Mock<ILoggerLite>();
            mockLogger1.Setup(l => l.LogError(It.IsAny<Exception>())).Callback(() => ++calls);
            mockLogger2.Setup(l => l.LogError(It.IsAny<Exception>())).Callback(() => ++calls);
            var tested = new AggregateLogger(new List<ILoggerLite> { mockLogger1.Object, mockLogger2.Object });
            for (var i = 0; i < repeats;)
            {
                Assert.Equal(i, calls);
                if (i % 2 == 0)
                    mockLogger1.Object.LogError(new ArgumentException());
                else
                    mockLogger2.Object.LogError(new ArgumentException());
                Assert.Equal(++i, calls);
            }
        }

        [Fact]
        public void AggregatedLoggerLogErrorTest()
        {
            var mockLogger1 = new Mock<ILoggerLite>(MockBehavior.Strict);
            var mockLogger2 = new Mock<ILoggerLite>(MockBehavior.Strict);
            mockLogger1.Setup(l => l.LogError(It.IsAny<string>()));
            mockLogger1.Setup(l => l.LogError(It.IsAny<string>()));
            mockLogger2.Setup(l => l.LogError(It.IsAny<string>()));
            mockLogger2.Setup(l => l.LogError(It.IsAny<string>()));
            var tested = new AggregateLogger(new List<ILoggerLite> { mockLogger1.Object, mockLogger2.Object });

            tested.LogError(new Exception("test, not exception."));
            tested.LogError("test, not exception.");
            mockLogger1.VerifyAll();
        }
        [Fact]
        public void AggregatedLoggerLogInfoTest()
        {
            var mockLogger1 = new Mock<ILoggerLite>(MockBehavior.Strict);
            var mockLogger2 = new Mock<ILoggerLite>(MockBehavior.Strict);
            mockLogger1.Setup(l => l.LogInfo(It.IsAny<string>()));
            mockLogger2.Setup(l => l.LogInfo(It.IsAny<string>()));
            var tested = new AggregateLogger(new List<ILoggerLite> { mockLogger1.Object, mockLogger2.Object });

            tested.LogInfo("test");
            mockLogger1.VerifyAll();
        }
        [Fact]
        public void AggregatedLoggerLogWarningTest()
        {
            var mockLogger1 = new Mock<ILoggerLite>(MockBehavior.Strict);
            var mockLogger2 = new Mock<ILoggerLite>(MockBehavior.Strict);
            mockLogger1.Setup(l => l.LogWarning(It.IsAny<string>()));
            mockLogger2.Setup(l => l.LogWarning(It.IsAny<string>()));
            var tested = new AggregateLogger(new List<ILoggerLite> { mockLogger1.Object, mockLogger2.Object });

            tested.LogWarning("test");
            mockLogger1.VerifyAll();
        }
        [Fact]
        public void AggregatedLoggerLogSuccessTest()
        {
            var mockLogger1 = new Mock<ILoggerLite>(MockBehavior.Strict);
            var mockLogger2 = new Mock<ILoggerLite>(MockBehavior.Strict);
            mockLogger1.Setup(l => l.LogSuccess(It.IsAny<string>()));
            mockLogger2.Setup(l => l.LogSuccess(It.IsAny<string>()));
            var tested = new AggregateLogger(new List<ILoggerLite> { mockLogger1.Object, mockLogger2.Object });

            tested.LogSuccess("done");

            mockLogger1.Verify(l => l.LogSuccess("done"), Times.Once);
            mockLogger2.Verify(l => l.LogSuccess("done"), Times.Once);
        }
        [Fact]
        public void LogThrowsAggregateExceptionCollectingChildFailures()
        {
            var failing = new Mock<ILoggerLite>();
            failing.Setup(l => l.LogError(It.IsAny<string>()))
                .Throws(new InvalidOperationException("child failed"));
            var working = new Mock<ILoggerLite>();
            var tested = new AggregateLogger(failing.Object, working.Object);

            var aggregate = Assert.Throws<AggregateException>(
                () => tested.Log("oops", MessageSeverity.Error));

            Assert.Single(aggregate.InnerExceptions);
            Assert.IsType<InvalidOperationException>(aggregate.InnerExceptions[0]);
            // A sibling logger is still invoked even though an earlier one threw.
            working.Verify(l => l.LogError("oops"), Times.Once);
        }
    }
}
