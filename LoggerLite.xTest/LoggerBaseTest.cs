using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LoggerLite.xTest
{
    public class LoggerBaseTest
    {
        [Fact]
        public void LogCallsLogInfoWithInfoSev()
        {
            const string testMessage = "testMessage!@#";
            var tested = new Mock<LoggerBase>();
            tested.Setup(x => x.LogInfo(It.IsAny<string>()));
            tested.Object.Log(testMessage, MessageSeverity.Information);

            tested.Verify(x => x.LogInfo(testMessage), Times.Once());
        }
        [Fact]
        public void LogCallsLogWarningWithWarningSev()
        {
            const string testMessage = "testMessage!@#";
            var tested = new Mock<LoggerBase>();
            tested.Setup(x => x.LogInfo(It.IsAny<string>()));
            tested.Object.Log(testMessage, MessageSeverity.Warning);

            tested.Verify(x => x.LogWarning(testMessage), Times.Once());
        }
        [Fact]
        public void LogCallsLogErrorWithErrorSev()
        {
            const string testMessage = "testMessage!@#";
            var tested = new Mock<LoggerBase>();
            tested.Setup(x => x.LogInfo(It.IsAny<string>()));
            tested.Object.Log(testMessage, MessageSeverity.Error);

            tested.Verify(x => x.LogError(testMessage), Times.Once());
        }
    }
}
