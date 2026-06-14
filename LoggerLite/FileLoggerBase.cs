using System.IO;

namespace LoggerLite;

public class FileLoggerBase : FormattedLoggerBase
{
	protected readonly object _syncRoot = new object();

	protected virtual string DefaultExtension => ".log";

	public virtual string DefaultFileName => Path.GetRandomFileName() + DefaultExtension;

	public string PathToLog => OutputFile?.FullName;

	public FileInfo OutputFile { get; }

	public DirectoryInfo OutputDirectory => OutputFile.Directory;

	public override bool FlushAuto => true;

	public override bool IsThreadSafe => true;

	public bool CreateDirIfNotExists { get; set; }

	private string GetOutputPath(string untrusted)
	{
		string text = untrusted ?? DefaultFileName;
		if (!Path.HasExtension(text))
		{
			return Path.ChangeExtension(text, DefaultExtension);
		}
		return text;
	}

	public FileLoggerBase(string filePath = null)
	{
		string outputPath = GetOutputPath(filePath);
		OutputFile = new FileInfo(outputPath);
	}

	protected internal sealed override void Log(string message)
	{
		lock (_syncRoot)
		{
			if (CreateDirIfNotExists && !OutputDirectory.Exists)
			{
				OutputDirectory.Create();
			}
			using StreamWriter streamWriter = new StreamWriter(OutputFile.FullName, append: true);
			streamWriter.Write(message);
		}
	}
}
