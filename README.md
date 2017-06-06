Most of the programming tasks are reducible or somehow related to logging some information and tracing all the different implementations or handling concurency issues can be a nuisance. The LoggerLite is a .NET Core and .NET classic compatible solution, featuring one interface ILogger, handfull of implementations and a passive debouncer. The solution is a thin wrapper around .NET FileStreaming, XDocument, Console and other classes. Currently, the project contains following implementations:
- Console Logger
- Debug Trace Logger
- File Logger
- Xml Logger
- Yaml Logger
