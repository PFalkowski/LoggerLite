Most of the programming tasks are reducible or somehow related to logging information. Tracing all the different implementations or handling concurency issues can be a nuisance. While there are many great, full-featured solutions, they are either not portable, bloated or badly written. If you need to understand and see the code you are using, this is a library for you. Unit tests cover most of the codeline, there are no external dependencies and all relevant cade takes around 15 KB - around 350 lines of code. The LoggerLite is a .NET Core and .NET classic compatible solution, featuring one interface ILogger, handfull of implementations and a passive debouncer. The solution is a thin wrapper around .NET FileStreaming, XDocument, Console and other classes. Currently, the project contains following implementations:
- Console Logger
- Debug Trace Logger
- File Logger
- Xml Logger
- Yaml Logger

Contributions are welcomed


The example for console logger:
```c#
using System;
using LoggerLite;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new ConsoleLogger();
            logger.LogInfo("info!");
            logger.LogWarning("warning");
            logger.LogError("error :(");
            Console.ReadKey();
        }
    }
}
```
The example for yaml logger or any file logger based on FileLoggerBase:
```c#
using System;
using LoggerLite;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new YamlFileLogger("yamlLog.yaml");
            logger.LogInfo("info");
            logger.LogWarning("warning");
            logger.LogError("error");//the logger automatically flushes messages to the yamlLog.yaml file, no need to call any save
            Console.ReadKey();
        }
    }
}
```
