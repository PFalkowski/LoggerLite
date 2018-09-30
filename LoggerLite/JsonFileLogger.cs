using System;
using System.Collections.Generic;
using System.Text;

namespace LoggerLite
{
    public class JsonFileLogger : FileLoggerBase
    {
        public JsonFileLogger(string path = null) : base(path)
        {
            Formatter =
                (level, message) =>
                    $"{{\"time\": \"{DateTime.Now}\", \"level\": \"{level}\", \"message\": \"{message}\"}}{Environment.NewLine}";
        }

        protected override string DefaultExtension => ".json";
    }
}
