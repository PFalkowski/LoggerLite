using LoggerLite;
using System;
using System.Diagnostics;
using System.IO;
using static ConsoleUserInteractionHelper.ConsoleHelper;

namespace HtmlLoggerTestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello! This is html logger demo / test.");
            Console.Write("Please enter number of messages to show in logger: ");
            int validInteger = GetNaturalInt();
            if (validInteger > 100000)
                Console.Write(" This can take a while:)");


            var outputFile = new FileInfo(Path.ChangeExtension(Path.GetRandomFileName(), "html"));
            var logger = new HtmlLogger();
            var random = new Random(0);
            for (int i = 0; i < validInteger; ++i)
            {
                switch (random.Next(6))
                {
                    case 0:
                    case 1:
                    case 2:
                        logger.LogInfo("info");
                        break;
                    case 3:
                    case 4:
                        logger.LogWarning("warning");
                        break;
                    case 5:
                        logger.LogError("error, but not really:)");
                        break;
                    default:
                        break;
                }
            }
            logger.Save(outputFile);
            using (var process = Process.Start(new ProcessStartInfo { FileName = outputFile.FullName, UseShellExecute = true } ))
            { }
        }
    }
}
