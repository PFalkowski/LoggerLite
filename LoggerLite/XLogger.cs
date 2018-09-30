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

        public override bool FlushAuto => false;
        public XDocument OutputDocument { get; } = new XDocument(
                new XDeclaration("1.0", "utf-8", "true"),
                new XElement(RootElementName));

        // TODO: add xsd validation

        public override void Save(TextWriter outputSteam)
        {
            OutputDocument.Save(outputSteam);
        }

        public override void LogInfo(string message)
        {
            OutputDocument.Root.Add(new XElement(EntryElementName,
                new XElement(TimeElementName, DateTime.Now),
                new XElement(DescriptionElementName, message),
                new XElement(TypeElementName, InfoName)));
        }

        public override void LogWarning(string warning)
        {
            OutputDocument.Root.Add(new XElement(EntryElementName,
                new XElement(TimeElementName, DateTime.Now),
                new XElement(DescriptionElementName, warning),
                new XElement(TypeElementName, WarningName)));
        }

        public override void LogError(Exception exception)
        {
            OutputDocument.Root.Add(new XElement(EntryElementName,
                new XElement(TimeElementName, DateTime.Now),
                new XElement(DescriptionElementName, exception),
                new XElement(TypeElementName, ErrorName)));
        }

        public override void LogError(string error)
        {
            OutputDocument.Root.Add(new XElement(EntryElementName,
                new XElement(TimeElementName, DateTime.Now),
                new XElement(DescriptionElementName, error),
                new XElement(TypeElementName, ErrorName)));
        }
    }
}
