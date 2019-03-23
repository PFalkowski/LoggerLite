using LoggerLite;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using static ConsoleUserInteractionHelper.ConsoleHelper;

namespace ActiveDebouncerTestHarness
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello in Active Debouncer test harness! " +
       "This is the load test. The file logger will be spammed with the number of messages (random GUIDs) you choose from multiple threads." +
       " The debouncer role is to keep the messaged queued in memory and not flush immidiatly. How many messages would you like to send from the thread pool?");
            Console.Write("Number of messages: ");
            int validInteger = GetNaturalInt();
            if (validInteger > 100000)
                Console.Write(" This can take a while:)");
            Console.WriteLine();
            var file = new FileInfo(Path.ChangeExtension(Path.GetRandomFileName(), "txt"));
            Console.WriteLine($"The contents will be written to {file.FullName}.");

            var watch = Stopwatch.StartNew();
            var fileLogger = new FileLoggerBase(file.FullName);

            using (var debouncer = new ActiveDebouncer())
            using (var wrapper = new QueuedLoggerWrapper(fileLogger, debouncer))
            {

                Parallel.For(0, validInteger, x =>
                {
                    wrapper.LogInfo(Guid.NewGuid().ToString());
                });
                watch.Stop();
                Console.WriteLine($"Finished in {watch.Elapsed} with {wrapper.Failures} failures, after {wrapper.Requests} log requests");
            }
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
