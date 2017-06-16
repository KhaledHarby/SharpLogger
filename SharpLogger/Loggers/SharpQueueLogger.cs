using Sharp.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.Loggers
{
    public class SharpQueueLogger : ISharpLoggerHandler
    {
        private readonly static object locker = new object();
        Queue _Queue;

        public SharpQueueLogger()
        {
            lock (locker)
            {
                _Queue = new Queue();
            }
        }


        public void Publish(LogMessage logMessage)
        {
            lock (locker)
            {
                _Queue.Enqueue(logMessage);
            }
        }
    }
}
