using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb.Settings
{
    /// <summary>
    /// Properties to match appsettings.json
    /// </summary>
    public class EmailSettings
    {
        public bool DefaultCredentials { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string SMTPServer { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string ExTo { get; set; }
        public string ExFrom { get; set; }
        public bool ExEnabled { get; set; }
    }
}
