using System.IO;
using System.Xml.Linq;
using Xunit;

namespace LoggerLite.xTest
{
    public class HtmlLoggerTest
    {
        private const string Xslt =
@"<?xml version='1.0' encoding='utf-8'?>
<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'
    xmlns:msxsl='urn:schemas-microsoft-com:xslt' exclude-result-prefixes='msxsl'
>
    <xsl:output method='xml' indent='yes'/>

    <xsl:template match='@* | node()'>
        <xsl:copy>
            <xsl:apply-templates select='@* | node()'/>
        </xsl:copy>
    </xsl:template>
</xsl:stylesheet>";

        [Fact]
        public void FlushAutoAlwaysReturnsFalse()
        {
            var tested1 = new HtmlLogger();
            Assert.False(tested1.FlushAuto);
        }

        [Fact]
        public void IsThreadSafeAlwaysReturnsTrue()
        {
            var tested1 = new HtmlLogger();
            Assert.False(tested1.IsThreadSafe);
        }

        [Fact]
        public void HtmlLoggerLogs()
        {
            const string error = "test error";
            const string info = "test info";
            const string warning = "test warning";
            const string outputFile = "htmlLogTest2.html";
            var file = new FileInfo(outputFile);

            var xDock = XDocument.Parse(Xslt);
            var tested = new HtmlLogger(xDock);

            tested.LogError(error);
            tested.LogInfo(info);
            tested.LogWarning(warning);

            var writer = new StringWriter();

            tested.OutputDocument.Save(writer);

            var actual = writer.ToString();

            Assert.Contains(error, actual);
            Assert.Contains(info, actual);
            Assert.Contains(warning, actual);
        }

        [Fact]
        public void HtmlLoggerSavesToWriter()
        {
            const string error = "test error";
            const string info = "test info";
            const string warning = "test warning";
            const string outputFile = "htmlLogTest2.html";
            var file = new FileInfo(outputFile);

            var xDock = XDocument.Parse(Xslt);
            var tested = new HtmlLogger(xDock);

            tested.LogError(error);
            tested.LogInfo(info);
            tested.LogWarning(warning);

            var writer = new StringWriter();

            tested.Save(writer);

            var actual = writer.ToString();

            Assert.Contains(error, actual);
            Assert.Contains(info, actual);
            Assert.Contains(warning, actual);
        }

        [Fact]
        public void HtmlLoggerSavesToDisk()
        {
            const string error = "test error";
            const string info = "test info";
            const string warning = "test warning";
            const string outputFile = "htmlLogTest.html";
            var file = new FileInfo(outputFile);
            var tested = new HtmlLogger();

            tested.LogInfo(info);
            tested.LogInfo(info);
            tested.LogInfo(info);
            tested.LogInfo(info);
            tested.LogInfo(info);
            tested.LogInfo(info);
            tested.LogWarning(warning);
            tested.LogError(error);
            try
            {
                tested.Save(file);
                Assert.True(File.Exists(outputFile));
            }
            finally
            {
                file.Delete();
            }
        }
    }
}
