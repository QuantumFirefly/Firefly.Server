using Firefly.Server.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Entitys
{
    public class LocalSettings : ISettings
    {
        public LocalSettings()
        {
            DbConnectionSettings = new DbConnectionSettings();
            LogSettings = new LogSettings();
        }
        public ISettings DbConnectionSettings { get; set; }
        public ISettings LogSettings { get; set; }

        public bool Validate(ref List<string> messages)
        {
            bool validationPassed = true;

            if (!DbConnectionSettings.Validate(ref messages)) validationPassed = false;
            if (!LogSettings.Validate(ref messages)) validationPassed = false;

            return validationPassed;
        }

    }
}
