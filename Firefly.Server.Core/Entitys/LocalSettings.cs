using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Entitys
{
    public class LocalSettings
    {
        public LocalSettings()
        {
            DbConnectionSettings = new DbConnectionSettings();
        }
        public DbConnectionSettings DbConnectionSettings { get; set; }


    }
}
