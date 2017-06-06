using System;

namespace LoggerLite
{
    public class YamlFileLogger : FileLoggerBase
    {
        public YamlFileLogger(string path = null) : base(path)
        {
            Formatter =
                (level, message) =>
                    $"---{Environment.NewLine}time: {DateTime.Now}{Environment.NewLine}{level}: {message}{Environment.NewLine}";
        }

        protected override string DefaultExtension => ".yaml";
    }
}