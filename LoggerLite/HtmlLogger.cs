using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace LoggerLite
{
    public class HtmlLogger : XLogger
    {
        private XslCompiledTransform Transform { get; }

        public override bool FlushAuto => false;

        public HtmlLogger() : this(XDocument.Parse(Properties.Resources.XLoggerStylesheet)) { }

        public HtmlLogger(XNode xDocument) : this(xDocument.CreateNavigator()) { }

        public HtmlLogger(FileInfo stylesheet)
        {
            Transform = new XslCompiledTransform(false);
            Transform.Load(stylesheet.FullName);
        }

        public HtmlLogger(IXPathNavigable xsltNavigator)
        {
            Transform = new XslCompiledTransform(false);
            Transform.Load(xsltNavigator);
        }

        // TODO: add Save() overload accepting XmlWriter, so that not only file save is possible
        public override void Save(FileInfo outputFile)
        {
            var outputFileName = Path.ChangeExtension(outputFile.FullName, "html");
            using (var output = XmlWriter.Create(outputFileName, new XmlWriterSettings() { Indent = true, ConformanceLevel = ConformanceLevel.Auto }))
            {
                Transform.Transform(OutputDocument.CreateNavigator(), output);
            }
        }
    }
}
