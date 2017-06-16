using Sharp.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharp.Configurations;
using System.Net.Mail;
using System.Net;
using System.Threading;

namespace Sharp.Loggers
{
    public class SharpEmailLogger : ISharpLoggerHandler
    {
        private EmailConfiguration _emailConfiguration;
        private static object locker = new object();
        private static List<string> _Recipient;


        /// <summary>
        /// Allow log handler to send messages/exceptions by EmailConfiguration 
        /// </summary>
        /// <param name="emailConfiguration"></param>
        public SharpEmailLogger(EmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
            _Recipient = new List<string>();
        }

        /// <summary>
        /// Allow log handler to send messages/exceptions by EmailConfiguration
        /// </summary>
        /// <param name="emailConfiguration">Set email configurations</param>
        /// <param name="Recipients">Message Recipients</param>
        public SharpEmailLogger(EmailConfiguration emailConfiguration, List<string> Recipients)
        {
            _emailConfiguration = emailConfiguration;
            _Recipient = Recipients;
        }


        public async void Publish(LogMessage logMessage)
        {
            lock (locker)
            {
                if (_Recipient != null && _Recipient.Count > 0)
                {
                    Task.WhenAll(SendEmail(logMessage));
                }
            }
        }

        private async Task SendEmail(LogMessage logMessage)
        {
            lock (locker)
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.Subject = string.Format("[Sharp Logger]  {0}  - {1}  {2}", logMessage.CallingClass, logMessage.CallingMethod, logMessage.Message);
                    mail.Body = "<h1>Sharp Logger</h1> <br/>";
                    mail.Body += string.Format("Calling Class : {0}", logMessage.CallingClass);
                    mail.Body += string.Format("Calling Method : {0}", logMessage.CallingMethod);
                    mail.Body += string.Format("Line Number : {0}", logMessage.LineNumber);
                    mail.Body += string.Format("Message : {0}", logMessage.Message);
                    mail.Body += string.Format("Entry Level : {0}", logMessage.Level);
                    mail.Body += string.Format("DateTime : {0}", logMessage.DateTime);
                    mail.IsBodyHtml = true;


                    mail.From = new MailAddress(_emailConfiguration.UserName);
                    for (int i = 0; i < SharpLogger.MailRecipients.Count; i++)
                    {
                        string recipient = SharpLogger.MailRecipients[i].ToString();
                        mail.To.Add(recipient);
                    }

                    using (SmtpClient smtp = new SmtpClient(_emailConfiguration.Host, _emailConfiguration.Port))
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = _emailConfiguration.UserName,
                            Password = _emailConfiguration.Password
                        };
                        smtp.Credentials = credential;
                        smtp.Host = _emailConfiguration.Host;
                        smtp.Port = _emailConfiguration.Port;
                        smtp.EnableSsl = _emailConfiguration.EnableSSL;
                        smtp.UseDefaultCredentials = _emailConfiguration.UseDefaultCredential;
                        smtp.SendAsync(mail, null);
                    }
                }
            }
        }

        /// <summary>
        /// Add new recipient 
        /// </summary>
        /// <param name="Recipient"></param>
        public void AddRecipient(string Recipient)
        {
            lock (locker)
            {
                if (!_Recipient.Contains(Recipient))
                    _Recipient.Add(Recipient);
            }
        }


        /// <summary>
        /// remove existing recipient
        /// </summary>
        /// <param name="Recipient"></param>
        public void RemoveRecipient(string Recipient)
        {
            lock (locker)
            {
                if (_Recipient.Contains(Recipient))
                    _Recipient.Remove(Recipient);
            }
        }

    }
}
