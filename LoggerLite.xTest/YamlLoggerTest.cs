using System;
using System.IO;
using Xunit;

namespace LoggerLite.xTest
{
    public class YamlLoggerTest
    {
        [Fact]
        public void DefaultExtensionTest1()
        {
            var tested1 = new YamlFileLogger();
            Assert.True(tested1.PathToLog.EndsWith(".yaml"), $"{tested1.PathToLog} does not end with \".yaml\"");
        }

        [Fact]
        public void YamlCreate()
        {
            var tested = new YamlFileLogger();

            var myPath = $"{typeof(YamlLoggerTest).Namespace}.{nameof(YamlCreate)}.yaml";
            var testedFileLogger = new YamlFileLogger(myPath);
            testedFileLogger.LogInfo("test70113");
            Assert.True(File.Exists(myPath));
            Assert.Equal(".yaml", Path.GetExtension(testedFileLogger.OutputFile.Name));
        }
        [Fact]
        public void LogInfo()
        {
            var myPath = $"{typeof(YamlLoggerTest).Namespace}.{nameof(LogInfo)}.yaml";
            var testedFileLogger = new YamlFileLogger(myPath);
            var expected = "test";
            testedFileLogger.LogInfo(expected);
            var received = File.ReadAllText(myPath);
            Assert.True(File.Exists(myPath));
            Assert.True(received.Contains(expected));
            Assert.True(received.Contains(LoggerBase.InfoName));
        }

        [Fact]
        public void LogWarning()
        {
            var myPath = $"{typeof(YamlLoggerTest).Namespace}.{nameof(LogWarning)}.yaml";
            var testedFileLogger = new YamlFileLogger(myPath);
            var expected = "test";
            testedFileLogger.LogWarning(expected);
            var received = File.ReadAllText(myPath);
            Assert.True(File.Exists(myPath));
            Assert.True(received.Contains(expected));
            Assert.True(received.Contains(LoggerBase.WarningName));
        }

        [Fact]
        public void LogError1()
        {
            var myPath = $"{typeof(YamlLoggerTest).Namespace}.{nameof(LogError1)}.yaml";
            var testedFileLogger = new YamlFileLogger(myPath);
            var expected = "test";
            testedFileLogger.LogError(expected);
            var received = File.ReadAllText(myPath);
            Assert.True(File.Exists(myPath));
            Assert.True(received.Contains(expected));
            Assert.True(received.Contains(LoggerBase.ErrorName));
        }

        [Fact]
        public void LogError2()
        {
            var myPath = $"{typeof(YamlLoggerTest).Namespace}.{nameof(LogError2)}.yaml";
            var testedFileLogger = new YamlFileLogger(myPath);
            var expected = new Exception("testExc");
            testedFileLogger.LogError(expected);
            var received = File.ReadAllText(myPath);
            Assert.True(File.Exists(myPath));
            Assert.True(received.Contains(expected.Message));
            Assert.True(received.Contains(LoggerBase.ErrorName));
        }

        [Fact]
        public void AppendTest()
        {
            var myPath = $"{typeof(YamlLoggerTest).Namespace}.{nameof(AppendTest)}.yaml";
            var testedFileLogger = new YamlFileLogger(myPath);
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
            Assert.True(received.Contains(expectedError1.Message));
            Assert.True(received.Contains(expectedError2));
            Assert.True(received.Contains(expectedWarning));
            Assert.True(received.Contains(expectedinfo));
            Assert.True(received.Contains(LoggerBase.ErrorName));
            Assert.True(received.Contains(LoggerBase.InfoName));
            Assert.True(received.Contains(LoggerBase.WarningName));
        }

        [Fact]
        public void FilePathNotNull()
        {
            const string path = "testPath.test";
            var tested = new YamlFileLogger(path);
            Assert.Equal(path, tested.PathToLog);
        }
    }
}
