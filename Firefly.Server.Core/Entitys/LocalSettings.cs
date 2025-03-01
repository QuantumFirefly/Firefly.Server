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

        public bool Validate(ref List<string> messages)
        {
            bool validationPassed = true;

            if (!DbConnectionSettings.Validate(ref messages)) validationPassed = false;

            return validationPassed;
        }

    }
}
