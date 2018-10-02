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
            tested.Setup(x => x.Log(It.IsAny<string>(), It.IsAny<MessageSeverity>()));
            tested.Object.Log(testMessage, MessageSeverity.Information);

            tested.Verify(x => x.Log(testMessage, MessageSeverity.Information), Times.Once());
        }
        [Fact]
        public void LogCallsLogWarningWithWarningSev()
        {
            const string testMessage = "testMessage!@#";
            var tested = new Mock<LoggerBase>();
            tested.Setup(x => x.Log(It.IsAny<string>(), It.IsAny<MessageSeverity>()));
            tested.Object.LogWarning(testMessage);

            tested.Verify(x => x.Log(testMessage, MessageSeverity.Warning), Times.Once());
        }
        [Fact]
        public void LogCallsLogErrorWithErrorSev()
        {
            const string testMessage = "testMessage!@#";
            var tested = new Mock<LoggerBase>();
            tested.Setup(x => x.Log(It.IsAny<string>(), It.IsAny<MessageSeverity>()));
            tested.Object.LogError(testMessage);

            tested.Verify(x => x.Log(testMessage, MessageSeverity.Error), Times.Once());
        }
    }
}
