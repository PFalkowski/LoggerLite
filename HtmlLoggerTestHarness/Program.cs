using LoggerLite;
using System;
using System.Diagnostics;
using System.IO;

namespace HtmlLoggerTestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello! This is html logger demo / test.");
            Console.WriteLine("Press any key to continue to test phase...");

            Console.ReadKey();
            Console.Clear();

            var outputFile = new FileInfo(Path.ChangeExtension(Path.GetRandomFileName(), "html"));
            var logger = new HtmlLogger();
            logger.LogInfo("info1");
            logger.LogInfo("info2");
            logger.LogInfo("info3");
            logger.LogInfo("info4");
            logger.LogInfo("info5");
            logger.LogWarning("warning1");
            logger.LogError("error, but not really:)");
            logger.Save(outputFile);
            using (var process = Process.Start(new ProcessStartInfo { FileName = outputFile.FullName, UseShellExecute = true } ))
            { }
        }
    }
}
