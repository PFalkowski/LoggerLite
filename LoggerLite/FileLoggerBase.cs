using System.IO;

namespace LoggerLite
{
    public class FileLoggerBase : FormattedLoggerBase
    {
        private readonly object _syncRoot = new object();
        protected virtual string DefaultExtension => ".log";
        public virtual string DefaultFileName => $"{Path.GetRandomFileName()}{DefaultExtension}";
        public string PathToLog => OutputFile?.FullName;
        public FileInfo OutputFile { get; }
        public DirectoryInfo OutputDirectory => OutputFile.Directory;
        public override bool FlushAuto => true;
        public bool CreateDirIfNotExists { get; set; } = false;

        private string GetOutputPath(string untrusted)
        {
            var pathTemp = untrusted ?? DefaultFileName;
            var result = Path.HasExtension(pathTemp) ? pathTemp : Path.ChangeExtension(pathTemp, DefaultExtension);
            return result;
        }

        public FileLoggerBase(string filePath = null)
        {
            var path = GetOutputPath(filePath);
            OutputFile = new FileInfo(path);
        }

        protected internal sealed override void Log(string message)
        {
            lock (_syncRoot)
            {
                if (CreateDirIfNotExists && !OutputDirectory.Exists)
                    OutputDirectory.Create();
                var fileStream = new FileStream(OutputFile.FullName, FileMode.Append);
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(message);
                }
            }
        }
    }
}