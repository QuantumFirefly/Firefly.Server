using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Entities.RemoteConfig
{
    public class RemoteTopConfig : IConfig
    {

        public GeneralConfig? General { get; set; }
        
        public IRCConfig? IRC { get; set; }

        public bool Validate(ref List<string> messages) {
            bool validationPassed = true;

            if (General != null && !General.Validate(ref messages)) validationPassed = false;
            if (IRC != null && !IRC.Validate(ref messages)) validationPassed = false;

            return validationPassed;
        }

    }
}
