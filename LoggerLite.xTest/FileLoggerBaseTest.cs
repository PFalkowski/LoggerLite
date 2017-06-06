using System;
using System.IO;
using Xunit;

namespace LoggerLite.xTest
{
    public class FileLoggerBaseTest
    {
        [Fact]
        public void LoggerEaseOfUse()
        {
            var myPath = $"{typeof(FileLoggerBaseTest).Namespace}.{nameof(LoggerEaseOfUse)}.log";
            var expected = "testing ease of use.";
            var myLogger = new FileLoggerBase(myPath);
            myLogger.LogInfo(expected);
            var actual = File.ReadAllText(myPath);
            Assert.True(actual.Contains(expected));
        }

        [Fact]
        public void CreateFileLogger()
        {
            var myPath = $"{typeof(FileLoggerBaseTest).Namespace}.{nameof(CreateFileLogger)}.log";
            var testedFileLogger = new FileLoggerBase(myPath);
            testedFileLogger.LogInfo("test7013");
            Assert.True(File.Exists(myPath));
        }

        [Fact]
        public void LogInfo()
        {
            var myPath = $"{typeof(FileLoggerBaseTest).Namespace}.{nameof(LogInfo)}.log";
            var testedFileLogger = new FileLoggerBase(myPath);
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
            var myPath = $"{typeof(FileLoggerBaseTest).Namespace}.{nameof(LogWarning)}.log";
            var testedFileLogger = new FileLoggerBase(myPath);
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
            var myPath = $"{typeof(FileLoggerBaseTest).Namespace}.{nameof(LogError1)}.log";
            var testedFileLogger = new FileLoggerBase(myPath);
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
            var myPath = $"{typeof(FileLoggerBaseTest).Namespace}.{nameof(LogError2)}.log";
            var testedFileLogger = new FileLoggerBase(myPath);
            var expected = new Exception("test");
            testedFileLogger.LogError(expected);
            var received = File.ReadAllText(myPath);
            Assert.True(File.Exists(myPath));
            Assert.True(received.Contains(expected.Message));
            Assert.True(received.Contains(LoggerBase.ErrorName));
        }

        [Fact]
        public void AppendTest()
        {
            var myPath = $"{typeof(FileLoggerBaseTest).Namespace}.{nameof(AppendTest)}.log";
            var testedFileLogger = new FileLoggerBase(myPath);
            var expectedError1 = new Exception("testError1");
            var expectedError2 = "testError2";
            var expectedWarning = "testWarning1";
            var expectedinfo = "testInfo1";
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
        public void FilePathNotChangable()
        {
            const string path = "testPath.test";
            var tested = new FileLoggerBase(path);
            Assert.Equal(path, tested.PathToLog);
        }

        [Fact]
        public void DefaultExtensionTest1()
        {
            var tested1 = new FileLoggerBase();
            Assert.True(tested1.PathToLog.EndsWith(".log"), $"{tested1.PathToLog} does not end with \".log\"");
        }

        // TODO: Write possible multithreaded scenarios

        //private static void AssertFileFormedWell(string myPath)
        //{
        //    var fileRead = File.ReadAllLines(myPath);
        //    Assert.Equal(NumberOfRepeats, fileRead.Length);
        //    foreach (
        //        var splittedNumbers in
        //        fileRead.Select(line => line.Split(new[] { "):" }, StringSplitOptions.RemoveEmptyEntries)[1])
        //            .Select(hundredSameNumbers => hundredSameNumbers.Split(',')))
        //    {
        //        Assert.Equal(NumberOfIntegersInTestString, splittedNumbers.Length);
        //        var first = splittedNumbers.First();
        //        foreach (var number in splittedNumbers)
        //        {
        //            Assert.Equal(first, number);
        //        }
        //    }
        //}
        //#region Synchronous

        //[Fact]
        //public void Synchronous()
        //{
        //    // Prepare
        //    var myPath = $"{typeof(FileLoggerBaseTest).Namespace}.{nameof(Synchronous)}.log";
        //    var loggerTested = new FileLoggerBase(myPath);
        //    var a = 0;
        //    // Test
        //    for (; a < NumberOfRepeats; ++a)
        //    {
        //        var message = string.Join(",", Enumerable.Repeat(a.ToString(), NumberOfIntegersInTestString));
        //        loggerTested.LogInfo(message);
        //    }

        //    // Assert
        //    AssertFileFormedWell(myPath);
        //}
        ////[Fact]
        //public void SynchronousNoDebouncing()
        //{
        //    // Prepare
        //    var myPath = $"{typeof(FileLoggerBaseTest).Namespace}.{nameof(SynchronousNoDebouncing)}.log";
        //    var loggerTested = new FileLoggerBase(myPath);
        //    var a = 0;
        //    // Test
        //    for (; a < NumberOfRepeats; ++a)
        //    {
        //        var message = string.Join(",", Enumerable.Repeat(a.ToString(), NumberOfIntegersInTestString));
        //        loggerTested.LogInfo(message);
        //    }

        //    // Assert
        //    AssertFileFormedWell(myPath);
        //}

        //#endregion

        //#region Test using ThreadPool

        //private static FileLoggerBase _logger;

        //[Fact]
        //public void TestRaceConditionWithThreadPool()
        //{
        //    // Prepare
        //    var myPath = $"{typeof(FileLoggerBaseTest).Namespace}.{nameof(TestRaceConditionWithThreadPool)}.log";
        //    _logger = new FileLoggerBase(myPath);

        //    // Test
        //    for (var i = 0; i < NumberOfRepeats; ++i)
        //    {
        //        Interlocked.Increment(ref _numberActive);
        //        var message = string.Join(",", Enumerable.Repeat(i.ToString(), NumberOfIntegersInTestString));
        //        ThreadPool.QueueUserWorkItem(InfoCallback, message);
        //    }
        //    PostActionCheck();
        //    _ewentHandlerAllDone.WaitOne();

        //    // Assert
        //    AssertFileFormedWell(myPath);
        //}

        //int _numberActive = 1;
        //readonly EventWaitHandle _ewentHandlerAllDone = new EventWaitHandle(false, EventResetMode.ManualReset);

        //void PostActionCheck()
        //{
        //    if (Interlocked.Decrement(ref _numberActive) == 0)
        //        _ewentHandlerAllDone.Set();
        //}

        //private void InfoCallback(object message)
        //{
        //    try
        //    {
        //        _logger.LogInfo(message.ToString());
        //    }
        //    finally
        //    {
        //        PostActionCheck();
        //    }
        //}

        //#endregion


        //#region Test using Parallel.ForEach


        //[Fact]
        //public void TestUsingParallelForEach()
        //{
        //    // Prepare
        //    var myPath = $"{typeof(FileLoggerBaseTest).Namespace}.{nameof(TestUsingParallelForEach)}.log";
        //    var loggerTested = new FileLoggerBase(myPath);

        //    // Test
        //    Parallel.For(0, NumberOfRepeats, a =>
        //    {
        //        var message = string.Join(",", Enumerable.Repeat(a.ToString(), NumberOfIntegersInTestString));
        //        loggerTested.LogInfo(message);
        //    });

        //    // Assert
        //    AssertFileFormedWell(myPath);
        //}

        //#endregion

        //#region Test using Thread

        //[Fact]
        //public void TestUsingThread()
        //{
        //    // Prepare
        //    var myPath = $"{typeof(FileLoggerBaseTest).Namespace}.{nameof(TestUsingThread)}.log";
        //    var loggerTested = new FileLoggerBase(myPath);
        //    var a = 0;
        //    Action log = () =>
        //    {
        //        loggerTested.LogInfo(
        //            string.Join(",", Enumerable.Repeat((++a).ToString(), NumberOfIntegersInTestString)));
        //    };
        //    var thrds = new Thread[NumberOfRepeats];

        //    // Test
        //    for (var i = 0; i < NumberOfRepeats; ++i)
        //    {
        //        try
        //        {
        //            thrds[i] = new Thread(new ThreadStart(log));
        //            thrds[i].Start();
        //        }
        //        catch (ThreadStateException te)
        //        {
        //            Trace.WriteLine(te.Message);
        //            Trace.WriteLine(te.Message);
        //        }
        //    }
        //    foreach (var t in thrds)
        //    {
        //        t.Join();
        //    }
        //    Assert.Equal(NumberOfRepeats, a);
        //    var fileRead = File.ReadAllLines(myPath);
        //    Assert.Equal(NumberOfRepeats, fileRead.Length);


        //    // Assert
        //    AssertFileFormedWell(myPath);
        //}
        //#endregion

        //#region Test using Task 

        //[Fact]
        //public void TestRaceConditionUsingTaskStart()
        //{
        //    // Prepare
        //    var myPath = $"{typeof(FileLoggerBaseTest).Namespace}.{nameof(TestUsingThread)}.log";
        //    var loggerTested = new FileLoggerBase(myPath);
        //    var a = 0;
        //    var tasks = new Task[NumberOfRepeats];
        //    // Test
        //    for (; a < NumberOfRepeats; ++a)
        //    {
        //        var message = string.Join(",", Enumerable.Repeat(a.ToString(), NumberOfIntegersInTestString));
        //        tasks[a] = (Task.Run(() => loggerTested.LogInfo(message)));
        //    }
        //    Task.WaitAll(tasks);

        //    // Assert
        //    AssertFileFormedWell(myPath);
        //}
        //#endregion
    }
}
