using System;
using System.IO;
using Xunit;

namespace LoggerLite.xTest
{
    public class JsonFileLoggerTest
    {

        [Fact]
        public void DefaultExtensionTest1()
        {
            using (var tested1 = new JsonFileLogger())
            {
                Assert.True(tested1.PathToLog.EndsWith(".json"), $"{tested1.PathToLog} does not end with \".json\"");
            }
        }

        [Fact]
        public void JsonLoggerCreate()
        {
            var myPath = $"{typeof(JsonFileLoggerTest).Namespace}.{nameof(JsonLoggerCreate)}.json";
            try
            {
                var tested = new JsonFileLogger();

                var testedFileLogger = new JsonFileLogger(myPath);
                testedFileLogger.LogInfo("test70113");
                Assert.True(File.Exists(myPath));
                Assert.Equal(".json", Path.GetExtension(testedFileLogger.OutputFile.Name));
            }
            finally
            {
                File.Delete(myPath);
            }
        }
        [Fact]
        public void LogInfo()
        {
            var myPath = $"{typeof(JsonFileLoggerTest).Namespace}.{nameof(LogInfo)}.json";
            try
            {
                var testedFileLogger = new JsonFileLogger(myPath);
                var expected = "test";
                testedFileLogger.LogInfo(expected);
                var received = File.ReadAllText(myPath);
                Assert.True(File.Exists(myPath));
                Assert.Contains(expected, received);
                Assert.Contains(LoggerBase.InfoName, received);
            }
            finally
            {
                File.Delete(myPath);
            }
        }

        [Fact]
        public void LogWarning()
        {
            var myPath = $"{typeof(JsonFileLoggerTest).Namespace}.{nameof(LogWarning)}.json";
            try
            {
                var testedFileLogger = new JsonFileLogger(myPath);
                var expected = "test";
                testedFileLogger.LogWarning(expected);
                var received = File.ReadAllText(myPath);
                Assert.True(File.Exists(myPath));
                Assert.Contains(expected, received);
                Assert.Contains(LoggerBase.WarningName, received);
            }
            finally
            {
                File.Delete(myPath);
            }
        }

        [Fact]
        public void LogError1()
        {
            var myPath = $"{typeof(JsonFileLoggerTest).Namespace}.{nameof(LogError1)}.json";
            try
            {
                var testedFileLogger = new JsonFileLogger(myPath);
                var expected = "test";
                testedFileLogger.LogError(expected);
                var received = File.ReadAllText(myPath);
                Assert.True(File.Exists(myPath));
                Assert.Contains(expected, received);
                Assert.Contains(LoggerBase.ErrorName, received);
            }
            finally
            {
                File.Delete(myPath);
            }
        }

        [Fact]
        public void LogError2()
        {
            var myPath = $"{typeof(JsonFileLoggerTest).Namespace}.{nameof(LogError2)}.json";
            try
            {
                var testedFileLogger = new JsonFileLogger(myPath);
                var expected = new Exception("testExc");
                testedFileLogger.LogError(expected);
                var received = File.ReadAllText(myPath);
                Assert.True(File.Exists(myPath));
                Assert.Contains(expected.Message, received);
                Assert.Contains(LoggerBase.ErrorName, received);
            }
            finally
            {
                File.Delete(myPath);
            }
        }

        [Fact]
        public void AppendTest()
        {
            var myPath = $"{typeof(JsonFileLoggerTest).Namespace}.{nameof(AppendTest)}.json";
            try
            {
                var testedFileLogger = new JsonFileLogger(myPath);
                var expectedError1 = new Exception("test1");
                var expectedError2 = "test2";
                var expectedWarning = "test3";
                var expectedinfo = "test4";
                testedFileLogger.LogError(expectedError1);
                testedFileLogger.LogError(expectedError2);
                testedFileLogger.LogWarning(expectedWarning);
                testedFileLogger.LogInfo(expectedinfo);
                var received = File.ReadAllText(myPath);
                Assert.True(File.Exists(myPath));
                Assert.Contains(expectedError1.Message, received);
                Assert.Contains(expectedError2, received);
                Assert.Contains(expectedWarning, received);
                Assert.Contains(expectedinfo, received);
                Assert.Contains(LoggerBase.ErrorName, received);
                Assert.Contains(LoggerBase.InfoName, received);
                Assert.Contains(LoggerBase.WarningName, received);
            }
            finally
            {
                File.Delete(myPath);
            }
        }

        [Fact]
        public void FilePathNotNull()
        {
            const string path = "testPath.test";
            using (var tested = new JsonFileLogger(path))
            {
                Assert.Contains(path, tested.PathToLog);
            }
        }

    }
}
