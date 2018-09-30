using System.IO;

namespace LoggerLite
{
    public class FileLoggerBase : FormattedLoggerBase
    {
        private readonly object _syncRoot = new object();

        protected virtual string DefaultExtension => ".log";
        public virtual string DefaultFileName => $"{Path.GetRandomFileName()}{DefaultExtension}";

        private string GetOutputPath(string untrusted)
        {
            var pathTemp = untrusted ?? DefaultFileName;
            var result = Path.HasExtension(pathTemp) ? pathTemp : pathTemp + DefaultExtension;
            return result;
        }

        public FileLoggerBase(string filePath = null)
        {
            PathToLog = GetOutputPath(filePath);
            OutputFile = new FileInfo(PathToLog);
        }

        public string PathToLog { get; }

        public FileInfo OutputFile { get; }

        public override bool FlushAuto => true;

        protected internal sealed override void Log(string message)
        {
            lock (_syncRoot)
            {
                using (var streamWriter = new StreamWriter(OutputFile.FullName, true))
                {
                    streamWriter.Write(message);
                }
            }
        }
    }
}