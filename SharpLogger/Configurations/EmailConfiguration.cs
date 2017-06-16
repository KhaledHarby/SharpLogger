using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.Configurations
{
    public class EmailConfiguration
    {
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string Host { get; private set; }
        public int Port { get; private set; }
        public bool EnableSSL { get; private set; }
        public bool UseDefaultCredential { get; private set; }

        public EmailConfiguration(string userName, string password, string host, int port, bool EnableSsl, bool UseDefaultCredential)
        {
            this.UserName = userName;
            this.Password = password;
            this.Host = host;
            this.Port = port;
            this.EnableSSL = EnableSsl;
            this.UseDefaultCredential = UseDefaultCredential;
        }
    }
}
