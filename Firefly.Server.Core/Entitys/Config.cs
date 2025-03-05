using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Entitys
{
    public class Config
    {

        public GeneralConfig General {  get; set; }
        public struct GeneralConfig() {
            public string ServerName { get; set; }
            public string ServerDomain { get; set; }
        }



        public IRCConfig IRC = new IRCConfig();
        public struct IRCConfig
        {
            public int Port { get; set; }
            public bool Enabled { get; set; }
        }
    }
}
