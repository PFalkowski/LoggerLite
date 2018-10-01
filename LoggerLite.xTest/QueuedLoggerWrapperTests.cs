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
        public void LogInfo()
        {
            const string expected = "test";
            var myPath = $"{typeof(QueuedLoggerWrapperTests).Namespace}.{Path.GetRandomFileName()}.log";
            var mockLogger1 = new Mock<FileLoggerBase>(myPath);
            //mockLogger1.Setup(m => m.Log(expected));
            var mockDebouncer1 = new Mock<IDebouncer>();
            QueuedLoggerWrapper testedQueuedFileLogger = null;

            try
            {
                testedQueuedFileLogger = new QueuedLoggerWrapper(mockLogger1.Object, mockDebouncer1.Object);
                testedQueuedFileLogger.LogInfo(expected);

                //var received = File.ReadAllText(myPath);

                //Assert.Contains(expected, received);
                //Assert.Contains(LoggerBase.InfoName, received);
            }
            finally
            {
                testedQueuedFileLogger.Dispose();
            }
        }

        [Fact]
        public void LogWarningLogsWarning()
        {
            var myPath = $"{typeof(QueuedLoggerWrapperTests).Namespace}.{Path.GetRandomFileName()}.log";
            var testedQueuedFileLogger = new QueuedLoggerWrapper(new FileLoggerBase(myPath), new PassiveDebouncer { DebounceMilliseconds = 1 });
            try
            {
                var expected = "test";
                testedQueuedFileLogger.LogWarning(expected);
                var received = File.ReadAllText(myPath);
                Assert.True(File.Exists(myPath));
                Assert.Contains(expected, received);
                Assert.Contains(LoggerBase.WarningName, received);
            }
            finally
            {
                testedQueuedFileLogger.Dispose();
                File.Delete(myPath);
            }
        }

        // TODO: this is not a unit. refactor
        [Fact]
        public void LogError1()
        {
            var myPath = $"{typeof(QueuedLoggerWrapperTests).Namespace}.{Path.GetRandomFileName()}.log";
            var testedQueuedFileLogger = new QueuedLoggerWrapper(new FileLoggerBase(myPath), new PassiveDebouncer { DebounceMilliseconds = 1 });
            try
            {
                var expected = "test";
                testedQueuedFileLogger.LogError(expected);
                var received = File.ReadAllText(myPath);
                Assert.True(File.Exists(myPath));
                Assert.Contains(expected, received);
                Assert.Contains(LoggerBase.ErrorName, received);
            }
            finally
            {
                testedQueuedFileLogger.Dispose();
                File.Delete(myPath);
            }
        }

        // TODO: this is not a unit. refactor
        [Fact]
        public void LogError2()
        {
            var myPath = $"{typeof(QueuedLoggerWrapperTests).Namespace}.{Path.GetRandomFileName()}.log";
            var testedQueuedFileLogger = new QueuedLoggerWrapper(new FileLoggerBase(myPath), new PassiveDebouncer { DebounceMilliseconds = 1 });
            try
            {
                var expected = new Exception("test");
                testedQueuedFileLogger.LogError(expected);
                var received = File.ReadAllText(myPath);
                Assert.True(File.Exists(myPath));
                Assert.Contains(expected.Message, received);
                Assert.Contains(LoggerBase.ErrorName, received);
            }
            finally
            {
                testedQueuedFileLogger.Dispose();
                File.Delete(myPath);
            }
        }

        // TODO: this is not a unit. refactor
        [Fact]
        public void DebounceTest()
        {
            var myPath = $"{typeof(QueuedLoggerWrapperTests).Namespace}.{Path.GetRandomFileName()}.log";
            var testedQueuedFileLogger = new QueuedLoggerWrapper(new FileLoggerBase(myPath), new PassiveDebouncer { DebounceMilliseconds = 10000 });
            try
            {
                var expectedError1 = new Exception("testError1");
                var expectedError2 = "testError2";
                var expectedWarning = "testError3";
                var expectedinfo = "testError4";
                testedQueuedFileLogger.LogError(expectedError1);
                testedQueuedFileLogger.LogError(expectedError2);
                testedQueuedFileLogger.LogWarning(expectedWarning);
                testedQueuedFileLogger.LogInfo(expectedinfo);


                var received = File.ReadAllText(myPath);
                Assert.True(File.Exists(myPath));
                Assert.Contains(expectedError1.Message, received);
                Assert.DoesNotContain(expectedError2, received);
                Assert.DoesNotContain(expectedWarning, received);
                Assert.DoesNotContain(expectedinfo, received);
                Assert.Contains(LoggerBase.ErrorName, received);
                Assert.DoesNotContain(LoggerBase.WarningName, received);
                Assert.DoesNotContain(LoggerBase.InfoName, received);
            }
            finally
            {
                testedQueuedFileLogger.Dispose();
                File.Delete(myPath);
            }
        }

        // TODO: this is not a unit. refactor
        [Fact]
        public void AppendTest()
        {
            var myPath = $"{typeof(QueuedLoggerWrapperTests).Namespace}.{Path.GetRandomFileName()}.log";
            var testedQueuedFileLogger = new QueuedLoggerWrapper(new FileLoggerBase(myPath), new PassiveDebouncer());
            try
            {
                var expectedError1 = new Exception("testError1");
                var expectedError2 = "testError2";
                var expectedWarning = "testWarning1";
                var expectedinfo = "testInfo1";
                testedQueuedFileLogger.LogError(expectedError1);
                testedQueuedFileLogger.LogError(expectedError2);
                testedQueuedFileLogger.LogWarning(expectedWarning);
                testedQueuedFileLogger.LogInfo(expectedinfo);
                testedQueuedFileLogger.Flush();


                var received = File.ReadAllText(myPath);
                Assert.True(File.Exists(myPath));
                Assert.Contains(expectedError1.Message, received);
                Assert.Contains(expectedError2, received);
                Assert.Contains(expectedWarning, received);
                Assert.Contains(expectedinfo, received);
                Assert.Contains(LoggerBase.ErrorName, received);
                Assert.Contains(LoggerBase.WarningName, received);
                Assert.Contains(LoggerBase.InfoName, received);
            }
            finally
            {
                testedQueuedFileLogger.Dispose();
                File.Delete(myPath);
            }
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
