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
            var mockLogger1 = new Mock<ILogger>();
            var mockLogger2 = new Mock<ILogger>();
            var tested = new AggregateLogger(mockLogger1.Object, mockLogger2.Object);
            Assert.Equal(2, tested.Loggers.Count);
        }
        [Fact]
        public void AggregatedLoggerCreateTest2()
        {
            var mockLogger1 = new Mock<ILogger>();
            var mockLogger2 = new Mock<ILogger>();
            var tested = new AggregateLogger(new List<ILogger> { mockLogger1.Object, mockLogger2.Object });
            Assert.Equal(2, tested.Loggers.Count);
        }
        [Fact]
        public void AggregatedLoggerDependantLoggerLogInfoTest()
        {
            const int repeats = 10;
            var calls = 0;
            var mockLogger1 = new Mock<ILogger>();
            var mockLogger2 = new Mock<ILogger>();
            mockLogger1.Setup(l => l.LogInfo(It.IsAny<string>())).Callback(() => ++calls);
            mockLogger2.Setup(l => l.LogInfo(It.IsAny<string>())).Callback(() => ++calls);
            var tested = new AggregateLogger(new List<ILogger> { mockLogger1.Object, mockLogger2.Object });
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
            var mockLogger1 = new Mock<ILogger>();
            var mockLogger2 = new Mock<ILogger>();
            mockLogger1.Setup(l => l.LogWarning(It.IsAny<string>())).Callback(() => ++calls);
            mockLogger2.Setup(l => l.LogWarning(It.IsAny<string>())).Callback(() => ++calls);
            var tested = new AggregateLogger(new List<ILogger> { mockLogger1.Object, mockLogger2.Object });
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
            var mockLogger1 = new Mock<ILogger>();
            var mockLogger2 = new Mock<ILogger>();
            mockLogger1.Setup(l => l.LogError(It.IsAny<string>())).Callback(() => ++calls);
            mockLogger2.Setup(l => l.LogError(It.IsAny<string>())).Callback(() => ++calls);
            var tested = new AggregateLogger(new List<ILogger> { mockLogger1.Object, mockLogger2.Object });
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
            var mockLogger1 = new Mock<ILogger>();
            var mockLogger2 = new Mock<ILogger>();
            mockLogger1.Setup(l => l.LogError(It.IsAny<Exception>())).Callback(() => ++calls);
            mockLogger2.Setup(l => l.LogError(It.IsAny<Exception>())).Callback(() => ++calls);
            var tested = new AggregateLogger(new List<ILogger> { mockLogger1.Object, mockLogger2.Object });
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
            var mockLogger1 = new Mock<ILogger>(MockBehavior.Strict);
            var mockLogger2 = new Mock<ILogger>(MockBehavior.Strict);
            mockLogger1.Setup(l => l.LogError(It.IsAny<Exception>()));
            mockLogger1.Setup(l => l.LogError(It.IsAny<string>()));
            mockLogger2.Setup(l => l.LogError(It.IsAny<Exception>()));
            mockLogger2.Setup(l => l.LogError(It.IsAny<string>()));
            var tested = new AggregateLogger(new List<ILogger> { mockLogger1.Object, mockLogger2.Object });

            tested.LogError(new Exception("test, not exception."));
            tested.LogError("test, not exception.");
            mockLogger1.VerifyAll();
        }
        [Fact]
        public void AggregatedLoggerLogInfoTest()
        {
            var mockLogger1 = new Mock<ILogger>(MockBehavior.Strict);
            var mockLogger2 = new Mock<ILogger>(MockBehavior.Strict);
            mockLogger1.Setup(l => l.LogInfo(It.IsAny<string>()));
            mockLogger2.Setup(l => l.LogInfo(It.IsAny<string>()));
            var tested = new AggregateLogger(new List<ILogger> { mockLogger1.Object, mockLogger2.Object });

            tested.LogInfo("test");
            mockLogger1.VerifyAll();
        }
        [Fact]
        public void AggregatedLoggerLogWarningTest()
        {
            var mockLogger1 = new Mock<ILogger>(MockBehavior.Strict);
            var mockLogger2 = new Mock<ILogger>(MockBehavior.Strict);
            mockLogger1.Setup(l => l.LogWarning(It.IsAny<string>()));
            mockLogger2.Setup(l => l.LogWarning(It.IsAny<string>()));
            var tested = new AggregateLogger(new List<ILogger> { mockLogger1.Object, mockLogger2.Object });

            tested.LogWarning("test");
            mockLogger1.VerifyAll();
        }
    }
}
