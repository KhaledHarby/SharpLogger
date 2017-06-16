using Sharp.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.Formatters
{
    public interface ISharpLoggerFormatter
    {
        string ApplyFormat(LogMessage logMessage);
    }
}
