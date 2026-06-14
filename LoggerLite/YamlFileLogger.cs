using System;

namespace LoggerLite;

public class YamlFileLogger : FileLoggerBase
{
	protected override string DefaultExtension => ".yaml";

	public YamlFileLogger(string path = null)
		: base(path)
	{
		base.Formatter = (string level, string message) => $"---{Environment.NewLine}time: {DateTime.Now}{Environment.NewLine}{level}: {message}{Environment.NewLine}";
	}
}
