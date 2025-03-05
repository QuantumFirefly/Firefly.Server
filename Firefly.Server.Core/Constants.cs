using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core
{
    public static class Constants
    {
        public const string DB_ENVIRONMENT_TYPE = "Production"; // Can use some env flags later to swap between Production and dev


        public const string LOCAL_SETTINGS_INI_FILE = "LocalConfig.ini";
        public const int LOWEST_PORT_ALLOWED = 1;
        public const int HIGHEST_PORT_ALLOWED = 65535;
        public const int MB_TO_BYTES = 1048576;
        
    }
    

}
