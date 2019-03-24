using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Xunit;

namespace LoggerLite.xTest
{
    public class XLoggerTest
    {
        [Fact]
        public void FlushAutoAlwaysReturnsFalse()
        {
            var myLogger = new XLogger();
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

            Assert.Single(myLogger.OutputDocument.Root.Elements(XLogger.EntryElementName));
            Assert.Equal(MessageSeverity.Error.ToString(), myLogger.OutputDocument.Root.Element(XLogger.EntryElementName).Element(XLogger.TypeElementName).Value);
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

            Assert.Single(myLogger.OutputDocument.Root.Elements(XLogger.EntryElementName));
            Assert.Equal(MessageSeverity.Error.ToString(), myLogger.OutputDocument.Root.Element(XLogger.EntryElementName).Element(XLogger.TypeElementName).Value);
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

            Assert.Single(myLogger.OutputDocument.Root.Elements(XLogger.EntryElementName));
            Assert.Equal(MessageSeverity.Warning.ToString(), myLogger.OutputDocument.Root.Element(XLogger.EntryElementName).Element(XLogger.TypeElementName).Value);
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

            Assert.Single(myLogger.OutputDocument.Root.Elements(XLogger.EntryElementName));
            Assert.Equal(MessageSeverity.Information.ToString(), myLogger.OutputDocument.Root.Element(XLogger.EntryElementName).Element(XLogger.TypeElementName).Value);
            Assert.Equal(expected, myLogger.OutputDocument.Root.Element(XLogger.EntryElementName).Element(XLogger.DescriptionElementName).Value);
            if (loggedApproxDate.Minute != 59 || loggedApproxDate.Second < 59 || loggedApproxDate.Millisecond < 999)
            {
                Assert.Equal(loggedApproxDate.ToString("yy-MM-dd-HH"),
                    Convert.ToDateTime(myLogger.OutputDocument.Root.Element(XLogger.EntryElementName).Element(XLogger.TimeElementName).Value).ToString("yy-MM-dd-HH"));
            }
        }

        [Fact]
        public void XLoggerSavesToStream()
        {
            var testInfo = "info";
            var myLogger = new XLogger();
            var output = new StringWriter();
            try
            {
                myLogger.LogInfo(testInfo);
                myLogger.Save(output);

                var actual = XDocument.Parse(output.ToString());
                var expected = myLogger.OutputDocument;
                Assert.True(XNode.DeepEquals(expected, actual));
            }
            finally
            {
                output.Dispose();
            }
        }
    }
}
