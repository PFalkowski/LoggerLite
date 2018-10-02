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
            tested.Object.LogInfo(testMessage);

            tested.Verify(x => x.LogInfo(testMessage), Times.Once());
        }
        [Fact]
        public void LogCallsLogWarningWithWarningSev()
        {
            const string testMessage = "testMessage!@#";
            var tested = new Mock<LoggerBase>();
            tested.Setup(x => x.LogInfo(It.IsAny<string>()));
            tested.Object.LogWarning(testMessage);

            tested.Verify(x => x.LogWarning(testMessage), Times.Once());
        }
        [Fact]
        public void LogCallsLogErrorWithErrorSev()
        {
            const string testMessage = "testMessage!@#";
            var tested = new Mock<LoggerBase>();
            tested.Setup(x => x.LogInfo(It.IsAny<string>()));
            tested.Object.LogError(testMessage);

            tested.Verify(x => x.LogError(testMessage), Times.Once());
        }
    }
}
