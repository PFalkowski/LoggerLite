using LoggerLite;
using System;

namespace ConsoleLoggerTestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello! This is console logger demo / test.");
            Console.WriteLine("Current console settings: "); 
            Console.WriteLine($"\tConsole.BackgroundColor: {Console.BackgroundColor}"); 
            Console.WriteLine($"\tConsole.ForegroundColor : {Console.ForegroundColor }"); 
            Console.WriteLine("Press any key to continue to test phase..."); 

            Console.ReadKey();
            Console.Clear();

            var logger = new ConsoleLogger();
            logger.LogInfo("info!");
            logger.LogWarning("warning");
            logger.LogError("error, but not really:)");
            Console.ReadKey();
        }
    }
}
