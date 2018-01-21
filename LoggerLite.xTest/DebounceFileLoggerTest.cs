using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace LoggerLite.xTest
{
    public class DebounceFileLoggerTest
    {
        [Fact]
        public void CreateQueuedFileLogger()
        {
            var myPath = $"{typeof(DebounceFileLoggerTest).Namespace}.{Path.GetRandomFileName()}.log";
            var testedQueuedFileLogger = new QueuedLoggerWrapper(new FileLoggerBase(myPath), new PassiveDebouncer { DebounceMilliseconds = 1 });
            testedQueuedFileLogger.LogInfo("test7013");
            Assert.True(File.Exists(myPath));
            File.Delete(myPath);
        }

        [Fact]
        public void LogInfo()
        {
            var myPath = $"{typeof(DebounceFileLoggerTest).Namespace}.{Path.GetRandomFileName()}.log";
            var testedQueuedFileLogger = new QueuedLoggerWrapper(new FileLoggerBase(myPath), new PassiveDebouncer { DebounceMilliseconds = 1 });
            var expected = "test";
            testedQueuedFileLogger.LogInfo(expected);
            var received = File.ReadAllText(myPath);
            Assert.True(File.Exists(myPath));
            Assert.Contains(expected, received);
            Assert.Contains(LoggerBase.InfoName, received);
            File.Delete(myPath);
        }

        [Fact]
        public void LogWarning()
        {
            var myPath = $"{typeof(DebounceFileLoggerTest).Namespace}.{Path.GetRandomFileName()}.log";
            var testedQueuedFileLogger = new QueuedLoggerWrapper(new FileLoggerBase(myPath), new PassiveDebouncer { DebounceMilliseconds = 1 });
            var expected = "test";
            testedQueuedFileLogger.LogWarning(expected);
            var received = File.ReadAllText(myPath);
            Assert.True(File.Exists(myPath));
            Assert.Contains(expected, received);
            Assert.Contains(LoggerBase.WarningName, received);
            File.Delete(myPath);
        }

        [Fact]
        public void LogError1()
        {
            var myPath = $"{typeof(DebounceFileLoggerTest).Namespace}.{Path.GetRandomFileName()}.log";
            var testedQueuedFileLogger = new QueuedLoggerWrapper(new FileLoggerBase(myPath), new PassiveDebouncer { DebounceMilliseconds = 1 });
            var expected = "test";
            testedQueuedFileLogger.LogError(expected);
            var received = File.ReadAllText(myPath);
            Assert.True(File.Exists(myPath));
            Assert.Contains(expected, received);
            Assert.Contains(LoggerBase.ErrorName, received);
            File.Delete(myPath);
        }

        [Fact]
        public void LogError2()
        {
            var myPath = $"{typeof(DebounceFileLoggerTest).Namespace}.{Path.GetRandomFileName()}.log";
            var testedQueuedFileLogger = new QueuedLoggerWrapper(new FileLoggerBase(myPath), new PassiveDebouncer { DebounceMilliseconds = 1 });
            var expected = new Exception("test");
            testedQueuedFileLogger.LogError(expected);
            var received = File.ReadAllText(myPath);
            Assert.True(File.Exists(myPath));
            Assert.Contains(expected.Message, received);
            Assert.Contains(LoggerBase.ErrorName, received);
            File.Delete(myPath);
        }
        [Fact]
        public void DebounceTest()
        {
            var myPath = $"{typeof(DebounceFileLoggerTest).Namespace}.{Path.GetRandomFileName()}.log";
            var testedQueuedFileLogger = new QueuedLoggerWrapper(new FileLoggerBase(myPath), new PassiveDebouncer { DebounceMilliseconds = 10000 });
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
            File.Delete(myPath);
        }

        [Fact]
        public void AppendTest()
        {
            var myPath = $"{typeof(DebounceFileLoggerTest).Namespace}.{Path.GetRandomFileName()}.log";
            var testedQueuedFileLogger = new QueuedLoggerWrapper(new FileLoggerBase(myPath), new PassiveDebouncer());
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


        // TODO: Write possible multithreaded scenarios
        //private static void AssertFileFormedWell(string myPath)
        //{
        //    const int numberOfRepeats = 10;
        //    const int numberOfIntegersInTestString = 10;

        //    var fileRead = File.ReadAllLines(myPath);
        //    Assert.Equal(numberOfRepeats, fileRead.Length);
        //    foreach (
        //        var splittedNumbers in
        //        fileRead.Select(line => line.Split(new[] { "):" }, StringSplitOptions.RemoveEmptyEntries)[1])
        //            .Select(hundredSameNumbers => hundredSameNumbers.Split(',')))
        //    {
        //        Assert.Equal(numberOfIntegersInTestString, splittedNumbers.Length);
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
        //    var myPath = $"{typeof(DebounceFileLoggerTest).Namespace}.{nameof(Synchronous)}.log";
        //    var loggerTested = new QueuedLoggerWrapper(new FileLoggerBase(myPath), new PassiveDebouncer());
        //    var a = 0;
        //    // Test
        //    for (; a < numberOfRepeats; ++a)
        //    {
        //        var message = string.Join(",", Enumerable.Repeat(a.ToString(), numberOfIntegersInTestString));
        //        loggerTested.LogInfo(message);
        //    }
        //    loggerTested.Flush();

        //    // Assert
        //    AssertFileFormedWell(myPath);
        //}
        ////[Fact]
        //public void SynchronousNoDebouncing()
        //{
        //    // Prepare
        //    var myPath = $"{typeof(DebounceFileLoggerTest).Namespace}.{nameof(SynchronousNoDebouncing)}.log";
        //    var loggerTested = new FileLoggerBase(myPath);
        //    var a = 0;
        //    // Test
        //    for (; a < numberOfRepeats; ++a)
        //    {
        //        var message = string.Join(",", Enumerable.Repeat(a.ToString(), numberOfIntegersInTestString));
        //        loggerTested.LogInfo(message);
        //    }

        //    // Assert
        //    AssertFileFormedWell(myPath);
        //}

        //#endregion

        //#region Test using ThreadPool

        //private static QueuedLoggerWrapper _logger;

        //[Fact]
        //public void TestRaceConditionWithThreadPool()
        //{
        //    // Prepare
        //    var myPath = $"{typeof(DebounceFileLoggerTest).Namespace}.{nameof(TestRaceConditionWithThreadPool)}.log";
        //    _logger = new QueuedLoggerWrapper(new FileLoggerBase(myPath), new PassiveDebouncer());

        //    // Test
        //    for (var i = 0; i < numberOfRepeats; ++i)
        //    {
        //        Interlocked.Increment(ref _numberActive);
        //        var message = string.Join(",", Enumerable.Repeat(i.ToString(), numberOfIntegersInTestString));
        //        ThreadPool.QueueUserWorkItem(InfoCallback, message);
        //    }
        //    PostActionCheck();
        //    _ewentHandlerAllDone.WaitOne();
        //    _logger.Flush();

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
        //    var myPath = $"{typeof(DebounceFileLoggerTest).Namespace}.{nameof(TestUsingParallelForEach)}.log";
        //    var loggerTested = new QueuedLoggerWrapper(new FileLoggerBase(myPath), new PassiveDebouncer());

        //    // Test
        //    Parallel.For(0, numberOfRepeats, a =>
        //    {
        //        var message = string.Join(",", Enumerable.Repeat(a.ToString(), numberOfIntegersInTestString));
        //        loggerTested.LogInfo(message);
        //    });
        //    loggerTested.Flush();

        //    // Assert
        //    AssertFileFormedWell(myPath);
        //}

        //#endregion

        //#region Test using Thread

        //[Fact]
        //public void TestUsingThread()
        //{
        //    // Prepare
        //    var myPath = $"{typeof(DebounceFileLoggerTest).Namespace}.{nameof(TestUsingThread)}.log";
        //    var loggerTested = new QueuedLoggerWrapper(new FileLoggerBase(myPath), new PassiveDebouncer());
        //    var a = 0;
        //    Action log = () =>
        //    {
        //        loggerTested.LogInfo(
        //            string.Join(",", Enumerable.Repeat((++a).ToString(), numberOfIntegersInTestString)));
        //    };
        //    var thrds = new Thread[numberOfRepeats];

        //    // Test
        //    for (var i = 0; i < numberOfRepeats; ++i)
        //    {
        //        try
        //        {
        //            thrds[i] = new Thread(new ThreadStart(log));
        //            thrds[i].Start();
        //        }
        //        catch (ThreadStateException te)
        //        {
        //            Trace.WriteLine(te.Message);
        //        }
        //    }
        //    foreach (var t in thrds)
        //    {
        //        t.Join();
        //    }
        //    loggerTested.Flush();
        //    Assert.Equal(numberOfRepeats, a);
        //    var fileRead = File.ReadAllLines(myPath);
        //    Assert.Equal(numberOfRepeats, fileRead.Length);

        //    // Assert
        //    AssertFileFormedWell(myPath);
        //}
        //#endregion

        //#region Test using Task 

        //[Fact]
        //public void TestRaceConditionWithTasks()
        //{
        //    // Prepare
        //    var myPath = $"{typeof(DebounceFileLoggerTest).Namespace}.{nameof(TestRaceConditionWithTasks)}.log";
        //    var loggerTested = new QueuedLoggerWrapper(new FileLoggerBase(myPath), new PassiveDebouncer());
        //    var a = 0;
        //    var tasks = new Task[numberOfRepeats];
        //    // Test
        //    for (; a < numberOfRepeats; ++a)
        //    {
        //        var message = string.Join(",", Enumerable.Repeat(a.ToString(), numberOfIntegersInTestString));
        //        tasks[a] = (Task.Run(() => loggerTested.LogInfo(message)));
        //    }
        //    Task.WaitAll(tasks);
        //    loggerTested.Flush();

        //    // Assert
        //    AssertFileFormedWell(myPath);
        //    Assert.Equal(numberOfRepeats, a);
        //}
        //#endregion

    }
}
