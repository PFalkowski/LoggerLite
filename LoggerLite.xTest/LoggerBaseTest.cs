using Moq;
using System;
using Xunit;

namespace LoggerLite.xTest
{
    public class LoggerBaseTest
    {
        private class LoggerBaseThrowingExceptionInLog : LoggerBase
        {
            public override bool FlushAuto => true;

            public override bool IsThreadSafe => true;

            protected override void Log(string message, MessageSeverity severity)
            {
                throw new ArgumentException();
            }
        }
        private class LoggerBaseJustAStubInherit : LoggerBase
        {
            public override bool FlushAuto => true;

            public override bool IsThreadSafe => true;

            protected override void Log(string message, MessageSeverity severity)
            {
            }
        }
        [Fact]
        public void ExceptionWhileLogging()
        {
            const string testMessage = "testMessage!@#";
            var tested = new LoggerBaseThrowingExceptionInLog();

            Assert.Equal(0, tested.Requests);
            Assert.Equal(0, tested.Failures);
            Assert.Equal(0, tested.Sucesses);
            tested.LogInfo(testMessage);
            Assert.Equal(1, tested.Requests);
            Assert.Equal(1, tested.Failures);
            Assert.Equal(0, tested.Sucesses);
            tested.LogError(testMessage);
            Assert.Equal(2, tested.Requests);
            Assert.Equal(2, tested.Failures);
            Assert.Equal(0, tested.Sucesses);
            tested.LogWarning(testMessage);
            Assert.Equal(3, tested.Requests);
            Assert.Equal(3, tested.Failures);
            Assert.Equal(0, tested.Sucesses);
        }

        [Fact]
        public void LoggingInfoSucessfully()
        {
            const string testMessage = "testMessage!@#";
            var tested = new LoggerBaseJustAStubInherit();

            Assert.Equal(0, tested.Requests);
            Assert.Equal(0, tested.Failures);
            Assert.Equal(0, tested.Sucesses);
            tested.LogInfo(testMessage);
            Assert.Equal(1, tested.Requests);
            Assert.Equal(0, tested.Failures);
            Assert.Equal(1, tested.Sucesses);
            tested.LogInfo(testMessage);
            Assert.Equal(2, tested.Requests);
            Assert.Equal(0, tested.Failures);
            Assert.Equal(2, tested.Sucesses);
        }
        [Fact]
        public void LoggingWarningSucessfully()
        {
            const string testMessage = "testMessage!@#";
            var tested = new LoggerBaseJustAStubInherit();

            Assert.Equal(0, tested.Requests);
            Assert.Equal(0, tested.Failures);
            Assert.Equal(0, tested.Sucesses);
            tested.LogWarning(testMessage);
            Assert.Equal(1, tested.Requests);
            Assert.Equal(0, tested.Failures);
            Assert.Equal(1, tested.Sucesses);
            tested.LogWarning(testMessage);
            Assert.Equal(2, tested.Requests);
            Assert.Equal(0, tested.Failures);
            Assert.Equal(2, tested.Sucesses);
        }
        [Fact]
        public void LoggingErrorSucessfully()
        {
            const string testMessage = "testMessage!@#";
            var tested = new LoggerBaseJustAStubInherit();

            Assert.Equal(0, tested.Requests);
            Assert.Equal(0, tested.Failures);
            Assert.Equal(0, tested.Sucesses);
            tested.LogError(testMessage);
            Assert.Equal(1, tested.Requests);
            Assert.Equal(0, tested.Failures);
            Assert.Equal(1, tested.Sucesses);
            tested.LogError(testMessage);
            Assert.Equal(2, tested.Requests);
            Assert.Equal(0, tested.Failures);
            Assert.Equal(2, tested.Sucesses);
        }
        [Fact]
        public void LoggingErrorSucessfully2()
        {
            const string testMessage = "testMessage!@#";
            var argException = new ArgumentException(testMessage);
            var tested = new LoggerBaseJustAStubInherit();

            Assert.Equal(0, tested.Requests);
            Assert.Equal(0, tested.Failures);
            Assert.Equal(0, tested.Sucesses);
            tested.LogError(argException);
            Assert.Equal(1, tested.Requests);
            Assert.Equal(0, tested.Failures);
            Assert.Equal(1, tested.Sucesses);
            tested.LogError(argException);
            Assert.Equal(2, tested.Requests);
            Assert.Equal(0, tested.Failures);
            Assert.Equal(2, tested.Sucesses);
        }
    }
}
