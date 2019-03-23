using LoggerLite;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using static ConsoleUserInteractionHelper.ConsoleHelper;

namespace JsonFileLoggerTestHarness
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello in JSON logger test harness! " +
                "This is the load test. The file logger will be spammed with the number of messages (random GUIDs) you choose from multiple threads.");
            Console.Write("Please enter number of messages to spam with: ");
            var validInteger = GetNaturalInt();
            if (validInteger > 100000)
                Console.Write(" This can take a while:)");
            Console.WriteLine();
            var file = new FileInfo(Path.ChangeExtension(Path.GetRandomFileName(), "json"));
            Console.WriteLine($"The contents will be written to {file.FullName}.");

            var fileLogger = new JsonFileLogger(file.FullName);
            var watch = Stopwatch.StartNew();

            Parallel.For(0, validInteger, x =>
            {
                fileLogger.LogInfo(Guid.NewGuid().ToString());
            });
            watch.Stop();
            Console.WriteLine($"Finished in {watch.Elapsed}");// with {wrapper.FailedDequeues} failures, after {wrapper.LogRequests} log requests");

            Console.WriteLine("Open file? y/n");
            var key = Console.ReadKey();
            if (key.KeyChar == 'y')
            {
                using (var process = Process.Start(new ProcessStartInfo { FileName = fileLogger.OutputFile.FullName, UseShellExecute = true }))
                { }
            }
            Console.WriteLine();
            Console.WriteLine("Delete file? y/n");
            key = Console.ReadKey();
            if (key.KeyChar == 'y')
            {
                fileLogger.OutputFile.Delete();
            }
        }
    }
}
