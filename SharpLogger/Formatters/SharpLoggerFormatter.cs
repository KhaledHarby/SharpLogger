using Sharp.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.Formatters
{
    public class SharpLoggerFormatter : ISharpLoggerFormatter
    {
        public SharpLoggerFormatter()
        {

        }

        public string ApplyFormat(LogMessage logMessage)
        {
            return string.Format("{0:dd.MM.yyyy HH:mm:ss}: {1} [line: {2} {3} -> {4}()]: {5}",
                            logMessage.DateTime, logMessage.Level, logMessage.LineNumber, logMessage.CallingClass,
                            logMessage.CallingMethod, logMessage.Message);
        }
    }
}
