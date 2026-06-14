using System;
using System.IO;
using System.Xml.Linq;

namespace LoggerLite;

public class XLogger : BufferedLoggerBase
{
	public const string EntryElementName = "entry";

	public const string TimeElementName = "time";

	public const string DescriptionElementName = "description";

	public const string TypeElementName = "type";

	public const string RootElementName = "activity";

	public XDocument OutputDocument { get; } = new XDocument(new XDeclaration("1.0", "utf-8", "true"), new XElement("activity"));

	public override bool IsThreadSafe => false;

	public override void Save(TextWriter outputSteam)
	{
		OutputDocument.Save(outputSteam);
	}

	public override void Log(string message, MessageSeverity severity)
	{
		OutputDocument.Root.Add(new XElement("entry", new XElement("time", DateTime.Now), new XElement("description", message), new XElement("type", severity.ToString())));
	}
}
