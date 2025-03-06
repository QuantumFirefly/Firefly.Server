using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Entitys
{
    public class Config // TODO - Implement IConfig Interface.
    {

        public GeneralConfig General {  get; set; } = new GeneralConfig();
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
