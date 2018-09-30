using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LoggerLite
{
    public class FileLoggerBase : FormattedLoggerBase, IDisposable
    {
        private readonly SemaphoreSlim _syncRoot = new SemaphoreSlim(1, 1);

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
            StreamWriter streamWriter = null;
            try
            {
                streamWriter = new StreamWriter(OutputFile.FullName, true);
                _syncRoot.Wait();
                streamWriter.Write(message);
            }
            finally
            {
                _syncRoot.Release();
                streamWriter?.Dispose();
            }
        }
        protected internal sealed override async Task LogAsync(string message)
        {
            StreamWriter streamWriter = null;
            try
            {
                streamWriter = new StreamWriter(OutputFile.FullName, true);
                await _syncRoot.WaitAsync().ConfigureAwait(false);
                await streamWriter.WriteAsync(message).ConfigureAwait(false);
            }
            finally
            {
                _syncRoot.Release();
                streamWriter?.Dispose();
            }
        }

        public void Dispose()
        {
            _syncRoot.Dispose();
        }
    }
}