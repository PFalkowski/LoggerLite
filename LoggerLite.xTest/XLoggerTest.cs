using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace LoggerLite.xTest
{
    public class XLoggerTest
    {

        [Fact]
        public void XLoggerCreateTest()
        {
            var myLogger = new XLogger();
            Assert.NotNull(myLogger);
            Assert.False(myLogger.FlushAuto);
        }
        [Fact]
        public void LoggerEaseOfUse()
        {
            var expected = "testing";
            var myLogger = new XLogger();
            myLogger.LogInfo(expected);
            myLogger.LogError(expected);

            Assert.NotNull(myLogger);
            Assert.Equal(2, myLogger.OutputDocument.Root.Elements(XLogger.EntryElementName).Count());
            Assert.Equal(expected, myLogger.OutputDocument.Root.Element(XLogger.EntryElementName).Element(XLogger.DescriptionElementName).Value);
            Assert.Equal(expected, myLogger.OutputDocument.Root.Elements(XLogger.EntryElementName).Last().Element(XLogger.DescriptionElementName).Value);
        }
        
        [Fact]
        public void XLoggerLogError()
        {
            var expected = "testing";
            var myLogger = new XLogger();
            var loggedApproxDate = DateTime.Now;
            myLogger.LogError(expected);

            Assert.Equal(1, myLogger.OutputDocument.Root.Elements(XLogger.EntryElementName).Count());
            Assert.Equal(LoggerBase.ErrorName, myLogger.OutputDocument.Root.Element(XLogger.EntryElementName).Element(XLogger.TypeElementName).Value);
            Assert.Equal(expected, myLogger.OutputDocument.Root.Element(XLogger.EntryElementName).Element(XLogger.DescriptionElementName).Value);
            if (loggedApproxDate.Minute != 59 || loggedApproxDate.Second < 59 || loggedApproxDate.Millisecond < 999)
            {
                Assert.Equal(loggedApproxDate.ToString("yy-MM-dd-HH"),
                    Convert.ToDateTime(myLogger.OutputDocument.Root.Element(XLogger.EntryElementName).Element(XLogger.TimeElementName).Value).ToString("yy-MM-dd-HH"));
            }
        }

        [Fact]
        public void XLoggerLogError2()
        {
            var expected = "testing";
            var exception = new ArgumentException(expected);
            var myLogger = new XLogger();
            var loggedApproxDate = DateTime.Now;
            myLogger.LogError(expected);

            Assert.Equal(1, myLogger.OutputDocument.Root.Elements(XLogger.EntryElementName).Count());
            Assert.Equal(LoggerBase.ErrorName, myLogger.OutputDocument.Root.Element(XLogger.EntryElementName).Element(XLogger.TypeElementName).Value);
            Assert.Equal(expected, myLogger.OutputDocument.Root.Element(XLogger.EntryElementName).Element(XLogger.DescriptionElementName).Value);
            if (loggedApproxDate.Minute != 59 || loggedApproxDate.Second < 59 || loggedApproxDate.Millisecond < 999)
            {
                Assert.Equal(loggedApproxDate.ToString("yy-MM-dd-HH"),
                    Convert.ToDateTime(myLogger.OutputDocument.Root.Element(XLogger.EntryElementName).Element(XLogger.TimeElementName).Value).ToString("yy-MM-dd-HH"));
            }
        }


        [Fact]
        public void XLoggerLogWarningSpecialChars()
        {
            var expected = "testing123info s a<>><)(*&^%$$#@!";
            var myLogger = new XLogger();
            var loggedApproxDate = DateTime.Now;
            myLogger.LogWarning(expected);

            Assert.Equal(1, myLogger.OutputDocument.Root.Elements(XLogger.EntryElementName).Count());
            Assert.Equal(LoggerBase.WarningName, myLogger.OutputDocument.Root.Element(XLogger.EntryElementName).Element(XLogger.TypeElementName).Value);
            Assert.Equal(expected, myLogger.OutputDocument.Root.Element(XLogger.EntryElementName).Element(XLogger.DescriptionElementName).Value);
            if (loggedApproxDate.Minute != 59 || loggedApproxDate.Second < 59 || loggedApproxDate.Millisecond < 999)
            {
                Assert.Equal(loggedApproxDate.ToString("yy-MM-dd-HH"),
                    Convert.ToDateTime(myLogger.OutputDocument.Root.Element(XLogger.EntryElementName).Element(XLogger.TimeElementName).Value).ToString("yy-MM-dd-HH"));
            }
        }

        [Fact]
        public void XLoggerLogInfoSpecialChars()
        {
            var expected = "testing123info s a<>><)(*&^%$$#@!";
            var myLogger = new XLogger();
            var loggedApproxDate = DateTime.Now;
            myLogger.LogInfo(expected);

            Assert.Equal(1, myLogger.OutputDocument.Root.Elements(XLogger.EntryElementName).Count());
            Assert.Equal(LoggerBase.InfoName, myLogger.OutputDocument.Root.Element(XLogger.EntryElementName).Element(XLogger.TypeElementName).Value);
            Assert.Equal(expected, myLogger.OutputDocument.Root.Element(XLogger.EntryElementName).Element(XLogger.DescriptionElementName).Value);
            if (loggedApproxDate.Minute != 59 || loggedApproxDate.Second < 59 || loggedApproxDate.Millisecond < 999)
            {
                Assert.Equal(loggedApproxDate.ToString("yy-MM-dd-HH"),
                    Convert.ToDateTime(myLogger.OutputDocument.Root.Element(XLogger.EntryElementName).Element(XLogger.TimeElementName).Value).ToString("yy-MM-dd-HH"));
            }
        }

        [Fact]
        public void XLoggerSavesToFile()
        {
            var expected = "info";
            var myLogger = new XLogger();
            var output = new FileInfo("test.xml");
            myLogger.LogInfo(expected);
            myLogger.Save(output);
            Assert.True(File.Exists(output.FullName));

            var fromFile = XDocument.Load(output.FullName);
            Assert.True(XNode.DeepEquals(myLogger.OutputDocument, fromFile));
        }
    }
}
