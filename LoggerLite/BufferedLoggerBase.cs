using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LoggerLite
{
    public abstract class BufferedLoggerBase : LoggerBase
    {
        public virtual void Save(FileInfo outputFile)
        {
            var fileStream = new FileStream(outputFile.FullName, FileMode.Create);
            using (var writer = new StreamWriter(fileStream))
            {
                Save(writer);
            }
        }

        public abstract void Save(TextWriter outputSteam);
    }
}
