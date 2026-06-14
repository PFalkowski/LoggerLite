using System.IO;

namespace LoggerLite;

public abstract class BufferedLoggerBase : LoggerBase
{
	public override bool FlushAuto => false;

	public virtual void Save(FileInfo outputFile)
	{
		using StreamWriter outputSteam = new StreamWriter(new FileStream(outputFile.FullName, FileMode.Create));
		Save(outputSteam);
	}

	public abstract void Save(TextWriter outputSteam);
}
