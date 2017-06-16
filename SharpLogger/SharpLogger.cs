using Sharp.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sharp
{
    /// <summary>
    /// Log your messages and exceptions
    /// </summary>
    public class SharpLogger
    {

        private static object locker = new object();
        private static readonly LogPublisher LogPublisher;
        private static readonly IList<string> Mailrecipients;

        public enum Level
        {
            None,
            Info,
            Warning,
            Error
        }

        static SharpLogger()
        {
            lock (locker)
            {
                LogPublisher = new LogPublisher();
                Mailrecipients = new List<string>();
            }
        }


        /// <summary>
        /// Register or remove logger handler 
        /// </summary>
        public static ISharpLoggerHandlerManager LoggerHandlerManager
        {
            get { return LogPublisher; }
        }

        /// <summary>
        /// Published Messages
        /// </summary>
        public static IEnumerable<LogMessage> Messages
        {
            get { return LogPublisher.Messages; }
        }
        

        /// <summary>
        /// Log Message into registered handlers
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            Log(Level.Info, message);
        }

        /// <summary>
        /// Log exception into registered handlers
        /// </summary>
        /// <param name="exception"></param>
        public static void Log(Exception exception)
        {
            Log(Level.Error, exception.Message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level">Entry Level </param>
        /// <param name="message">Message to log</param>
        public static void Log(Level level, string message)
        {
            var stackFrame = FindStackFrame();
            var methodBase = GetCallingMethodBase(stackFrame);
            var callingMethod = methodBase.Name;
            var callingClass = methodBase.ReflectedType.Name;
            var lineNumber = stackFrame.GetFileLineNumber();

            Log(level, message, callingClass, callingMethod, lineNumber);
        }

        public static void AddRecipient(string recipient)
        {
            lock (locker)
            {
                if (!Mailrecipients.Contains(recipient))
                    Mailrecipients.Add(recipient);
            }
        }

        public static void RemoveRecipient(string recipient)
        {
            lock (locker)
            {
                if (Mailrecipients.Contains(recipient))
                    Mailrecipients.Remove(recipient);
            }
        }

        public static  IReadOnlyList<string> MailRecipients
        {
            get { return Mailrecipients.ToList(); }
            private set { }
        }

        private static void Log(Level level, string message, string callingClass, string callingMethod, int lineNumber)
        {
            var currentDateTime = DateTime.Now;

            var logMessage = new LogMessage(level, message, currentDateTime, callingClass, callingMethod, lineNumber);
            LogPublisher.Publish(logMessage);
        }

        private static MethodBase GetCallingMethodBase(StackFrame stackFrame)
        {
            return stackFrame == null
                ? MethodBase.GetCurrentMethod() : stackFrame.GetMethod();
        }

        private static StackFrame FindStackFrame()
        {
            var stackTrace = new StackTrace();
            for (var i = 0; i < stackTrace.GetFrames().Count(); i++)
            {
                var methodBase = stackTrace.GetFrame(i).GetMethod();
                var name = MethodBase.GetCurrentMethod().Name;
                if (!methodBase.Name.Equals("Log") && !methodBase.Name.Equals(name))
                    return new StackFrame(i, true);
            }
            return null;
        }
    }
}
