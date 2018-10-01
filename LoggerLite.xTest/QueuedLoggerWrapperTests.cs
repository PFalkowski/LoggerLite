using Moq;
using System;
using System.IO;
using Xunit;

namespace LoggerLite.xTest
{
    public class QueuedLoggerWrapperTests
    {
        [Fact]
        public void CreateQueuedFileLogger()
        {
            var myPath = $"{typeof(QueuedLoggerWrapperTests).Namespace}.{Path.GetRandomFileName()}.log";
            var testedQueuedFileLogger = new QueuedLoggerWrapper(new FileLoggerBase(myPath), new PassiveDebouncer { DebounceMilliseconds = 1 });
            try
            {
                testedQueuedFileLogger.LogInfo("test7013");
                Assert.True(File.Exists(myPath));
            }
            finally
            {
                testedQueuedFileLogger.Dispose();
                File.Delete(myPath);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void FlushAutoReturnsValueDependingOnWrappedLogger(bool flushesAuto)
        {
            var myPath = $"{typeof(QueuedLoggerWrapperTests).Namespace}.{Path.GetRandomFileName()}.log";
            var mockLogger1 = new Mock<FileLoggerBase>(myPath);
            var mockDebouncer1 = new Mock<IDebouncer>();
            mockLogger1.SetupGet(m => m.FlushAuto).Returns(flushesAuto);
            var testedQueuedFileLogger = new QueuedLoggerWrapper(mockLogger1.Object, mockDebouncer1.Object);
            Assert.Equal(flushesAuto, testedQueuedFileLogger.FlushAuto);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsThreadSafeAlwaysReturnsTrue(bool isThreadSafe)
        {
            var myPath = $"{typeof(QueuedLoggerWrapperTests).Namespace}.{Path.GetRandomFileName()}.log";
            var mockLogger1 = new Mock<FileLoggerBase>(myPath);
            var mockDebouncer1 = new Mock<IDebouncer>();
            mockLogger1.SetupGet(m => m.IsThreadSafe).Returns(isThreadSafe);
            var testedQueuedFileLogger = new QueuedLoggerWrapper(mockLogger1.Object, mockDebouncer1.Object);
            Assert.True(testedQueuedFileLogger.IsThreadSafe);
        }

        [Fact]
        public void DisposeDisposesUnderlyingLogger()
        {
            var myPath = $"{typeof(QueuedLoggerWrapperTests).Namespace}.{Path.GetRandomFileName()}.log";
            var mockLogger1 = new Mock<FileLoggerBase>(myPath);
            var mockDebouncer1 = new Mock<IDebouncer>();
            mockDebouncer1.Setup(e => e.Dispose());
            QueuedLoggerWrapper testedQueuedFileLogger = null;

            try
            {
                testedQueuedFileLogger = new QueuedLoggerWrapper(mockLogger1.Object, mockDebouncer1.Object);
            }
            finally
            {
                testedQueuedFileLogger.Dispose();
            }
            mockDebouncer1.Verify(e => e.Dispose());
        }
    }
}
