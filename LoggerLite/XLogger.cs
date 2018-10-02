using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace LoggerLite
{
    public class XLogger : BufferedLoggerBase
    {
        public const string EntryElementName = "entry";
        public const string TimeElementName = "time";
        public const string DescriptionElementName = "description";
        public const string TypeElementName = "type";
        public const string RootElementName = "activity";

        public override int Requests { get; protected set; }
        public override int Sucesses { get; protected set; }
        public override int Failures { get; protected set; }

        public XDocument OutputDocument { get; } = new XDocument(
                new XDeclaration("1.0", "utf-8", "true"),
                new XElement(RootElementName));

        public override bool IsThreadSafe => false;

        // TODO: add xsd validation
        public override void Save(TextWriter outputSteam)
        {
            OutputDocument.Save(outputSteam);
        }

        public override void Log(string message, MessageSeverity severity)
        {
            ++Requests;
            try
            {
                OutputDocument.Root.Add(new XElement(EntryElementName,
                    new XElement(TimeElementName, DateTime.Now),
                    new XElement(DescriptionElementName, message),
                    new XElement(TypeElementName, severity.ToString())));
                ++Sucesses;
            }
            catch (Exception)
            {
                ++Failures;
            }
        }
    }
}
