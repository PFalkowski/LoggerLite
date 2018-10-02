﻿using LoggerLite;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace JsonFileLoggerTestHarness
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello in JSON logger test harness! " +
                "This is the load test. The file logger will be spammed with the number of messages (random GUIDs) you choose from multiple threads.");
            Console.Write("Please enter number of messages to spam with: ");
            var line = Console.ReadLine();
            int validInteger;
            while (!int.TryParse(line, out validInteger) && !(validInteger > 0))
            {
                Console.WriteLine($"There was a problem with your input: {line} is not a valid integer in this context. Enter any natural number greater than 0.");
                Console.Write("Number of messages: ");
                line = Console.ReadLine();
            }
            Console.Write($"You chose {validInteger}.");
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

            Console.WriteLine("Delete file? y/n");
            var key = Console.ReadKey();
            if (key.KeyChar == 'y')
            {
                fileLogger.OutputFile.Delete();
            }
        }
    }
}