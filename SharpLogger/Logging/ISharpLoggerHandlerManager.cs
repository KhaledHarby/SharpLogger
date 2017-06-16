using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.Logging
{
    public interface ISharpLoggerHandlerManager
    {
        /// <summary>
        /// Register Logger provider for your messages  
        /// </summary>
        /// <param name="loggerHandler">Logger Type</param>
        /// <returns></returns>
        ISharpLoggerHandlerManager RegisterLogger(ISharpLoggerHandler loggerHandler);




        /// <summary>
        /// Remove Registered Logger provider   
        /// </summary>
        /// <param name="loggerHandler">Logger Type</param>
        /// <returns></returns>
        bool RemoveLogger(ISharpLoggerHandler loggerHandler);
    }
}
