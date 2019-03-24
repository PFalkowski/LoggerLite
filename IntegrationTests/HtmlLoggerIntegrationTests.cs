using LoggerLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace IntegrationTests
{
    public class HtmlLoggerIntegrationTests
    {

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
