﻿using System;
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
        public void FlushAutoAlwaysReturnsTrue()
        {
            var tested1 = new YamlFileLogger();
            Assert.True(tested1.FlushAuto);
        }

        [Fact]
        public void YamlCreate()
        {
            var myPath = $"{typeof(YamlLoggerTest).Namespace}.{nameof(YamlCreate)}.yaml";
            try
            {
                var tested = new YamlFileLogger();

                var testedFileLogger = new YamlFileLogger(myPath);
                testedFileLogger.LogInfo("test70113");
                Assert.True(File.Exists(myPath));
                Assert.Equal(".yaml", Path.GetExtension(testedFileLogger.OutputFile.Name));
            }
            finally
            {
                File.Delete(myPath);
            }
        }
        [Fact]
        public void LogInfo()
        {
            var myPath = $"{typeof(YamlLoggerTest).Namespace}.{nameof(LogInfo)}.yaml";
            try
            {
                var testedFileLogger = new YamlFileLogger(myPath);
                var expected = "test";
                testedFileLogger.LogInfo(expected);
                var received = File.ReadAllText(myPath);
                Assert.True(File.Exists(myPath));
                Assert.Contains(expected, received);
                Assert.Contains(MessageSeverity.Information.ToString(), received);
            }
            finally
            {
                File.Delete(myPath);
            }
        }

        [Fact]
        public void LogWarning()
        {
            var myPath = $"{typeof(YamlLoggerTest).Namespace}.{nameof(LogWarning)}.yaml";
            try
            {
                var testedFileLogger = new YamlFileLogger(myPath);
                var expected = "test";
                testedFileLogger.LogWarning(expected);
                var received = File.ReadAllText(myPath);
                Assert.True(File.Exists(myPath));
                Assert.Contains(expected, received);
                Assert.Contains(MessageSeverity.Warning.ToString(), received);
            }
            finally
            {
                File.Delete(myPath);
            }
        }

        [Fact]
        public void LogError1()
        {
            var myPath = $"{typeof(YamlLoggerTest).Namespace}.{nameof(LogError1)}.yaml";
            try
            {
                var testedFileLogger = new YamlFileLogger(myPath);
                var expected = "test";
                testedFileLogger.LogError(expected);
                var received = File.ReadAllText(myPath);
                Assert.True(File.Exists(myPath));
                Assert.Contains(expected, received);
                Assert.Contains(MessageSeverity.Error.ToString(), received);
            }
            finally
            {
                File.Delete(myPath);
            }
        }

        [Fact]
        public void LogError2()
        {
            var myPath = $"{typeof(YamlLoggerTest).Namespace}.{nameof(LogError2)}.yaml";
            try
            {
                var testedFileLogger = new YamlFileLogger(myPath);
                var expected = new Exception("testExc");
                testedFileLogger.LogError(expected);
                var received = File.ReadAllText(myPath);
                Assert.True(File.Exists(myPath));
                Assert.Contains(expected.Message, received);
                Assert.Contains(MessageSeverity.Error.ToString(), received);
            }
            finally
            {
                File.Delete(myPath);
            }
        }

        [Fact]
        public void AppendTest()
        {
            var myPath = $"{typeof(YamlLoggerTest).Namespace}.{nameof(AppendTest)}.yaml";
            try
            {
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
                Assert.Contains(expectedError1.Message, received);
                Assert.Contains(expectedError2, received);
                Assert.Contains(expectedWarning, received);
                Assert.Contains(expectedinfo, received);
                Assert.Contains(MessageSeverity.Error.ToString(), received);
                Assert.Contains(MessageSeverity.Information.ToString(), received);
                Assert.Contains(MessageSeverity.Warning.ToString(), received);
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
            var tested = new YamlFileLogger(path);
            Assert.Contains(path, tested.PathToLog);
        }
    }
}
