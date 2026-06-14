using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;
using LoggerLite.Properties;

namespace LoggerLite;

public class HtmlLogger : XLogger
{
	private XslCompiledTransform Transform { get; }

	public HtmlLogger()
		: this(XDocument.Parse(Resources.XLoggerStylesheet))
	{
	}

	public HtmlLogger(XNode xsltTransformStylesheet)
		: this(xsltTransformStylesheet.CreateNavigator())
	{
	}

	public HtmlLogger(FileInfo xsltTransformStylesheet)
	{
		Transform = new XslCompiledTransform(enableDebug: false);
		Transform.Load(xsltTransformStylesheet.FullName);
	}

	public HtmlLogger(IXPathNavigable xsltTransformStylesheet)
	{
		Transform = new XslCompiledTransform(enableDebug: false);
		Transform.Load(xsltTransformStylesheet);
	}

	public override void Save(FileInfo outputFile)
	{
		using XmlWriter results = XmlWriter.Create(Path.ChangeExtension(outputFile.FullName, "html"), new XmlWriterSettings
		{
			Indent = true,
			ConformanceLevel = ConformanceLevel.Auto
		});
		Transform.Transform(base.OutputDocument.CreateNavigator(), results);
	}
}
