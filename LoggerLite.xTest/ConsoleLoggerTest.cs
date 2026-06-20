using Xunit;

namespace LoggerLite.xTest
{
    public class ConsoleLoggerTest
    {
        [Fact]
        public void CountsEachSuccessExactlyOnce()
        {
            // Regression: ConsoleLogger.Log incremented the success counter itself in
            // addition to LoggerBase, so successes were double-counted (> requests).
            var tested = new ConsoleLogger();

            tested.LogInfo("info");
            tested.LogWarning("warn");
            tested.LogError("err");

            Assert.Equal(3, tested.Requests);
            Assert.Equal(3, tested.Successes);
            Assert.Equal(0, tested.Failures);
        }

        [Fact]
        public void LogSuccessUsesSuccessSeverityAndCountsSuccess()
        {
            // Exercises the MessageSeverity.Success branch (sets SuccessColor) which the
            // other severities don't reach.
            var tested = new ConsoleLogger();

            tested.LogSuccess("operation completed");

            Assert.Equal(1, tested.Requests);
            Assert.Equal(1, tested.Successes);
            Assert.Equal(0, tested.Failures);
        }
    }
}
