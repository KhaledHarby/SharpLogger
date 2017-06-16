using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.Logging
{
    public class LogPublisher : ISharpLoggerHandlerManager
    {
        private readonly IList<ISharpLoggerHandler> _loggerHandlers;
        private readonly IList<LogMessage> _messages;

        public LogPublisher()
        {
            _loggerHandlers = new List<ISharpLoggerHandler>();
            _messages = new List<LogMessage>();
           
        }

        public void Publish(LogMessage logMessage)
        {
            foreach (var loggerHandler in _loggerHandlers)
                loggerHandler.Publish(logMessage);
        }

        /// <summary>
        /// Register Logger provider for your messages  
        /// </summary>
        /// <param name="loggerHandler">Logger Type</param>
        /// <returns></returns>
        public ISharpLoggerHandlerManager RegisterLogger(ISharpLoggerHandler loggerHandler)
        {
            if (loggerHandler != null)
                _loggerHandlers.Add(loggerHandler);
            return this;
        }

        /// <summary>
        /// Remove Registered Logger provider   
        /// </summary>
        /// <param name="loggerHandler">Logger Type</param>
        /// <returns></returns>
        public bool RemoveLogger(ISharpLoggerHandler loggerHandler)
        {
            return _loggerHandlers.Remove(loggerHandler);
        }

        public IEnumerable<LogMessage> Messages
        {
            get { return _messages; }
        }
    }
}
