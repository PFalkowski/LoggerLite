using LoggerLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Xunit;

namespace IntegrationTests
{
    public class XLoggerIntegrationTests
    {
        [Fact]
        public void XLoggerSavesToFile()
        {
            var expected = "info";
            var myLogger = new XLogger();
            var output = new FileInfo("test.xml");
            try
            {
                myLogger.LogInfo(expected);
                myLogger.Save(output);
                Assert.True(File.Exists(output.FullName));

                var fromFile = XDocument.Load(output.FullName);
                Assert.True(XNode.DeepEquals(myLogger.OutputDocument, fromFile));
            }
            finally
            {
                output.Delete();
            }
        }

        [Fact]
        public void XLoggerSavesToFileTwiceAndTheFileIsValid()
        {
            var testInfo = "info";
            var testWarning = "info";
            var myLogger = new XLogger();
            var output = new FileInfo("test.xml");
            try
            {
                myLogger.LogInfo(testInfo);
                myLogger.Save(output);
                myLogger.LogWarning(testWarning);
                myLogger.Save(output);

                var fromFile = XDocument.Load(output.FullName);
                Assert.True(XNode.DeepEquals(myLogger.OutputDocument, fromFile));
            }
            finally
            {
                output.Delete();
            }
        }
    }
}
