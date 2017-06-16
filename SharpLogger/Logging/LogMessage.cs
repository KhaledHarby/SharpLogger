using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.Logging
{
    public class LogMessage
    {
        public DateTime DateTime { get; set; }
        public SharpLogger.Level Level { get; set; }
        public string Message { get; set; }
        public string CallingClass { get; set; }
        public string CallingMethod { get; set; }
        public int LineNumber { get; set; }

        public LogMessage() { }

        public LogMessage(SharpLogger.Level level, string message, DateTime dateTime, string callingClass, string callingMethod, int lineNumber)
        {
            Level = level;
            Message = message;
            DateTime = dateTime;
            CallingClass = callingClass;
            CallingMethod = callingMethod;
            LineNumber = lineNumber;
        }

        public override string ToString()
        {
            return new Formatters.SharpLoggerFormatter().ApplyFormat(this);

        }
    }

}
