using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firefly.Server.Core.Entities.LocalConfig;
using Firefly.Server.Core.Entities.RemoteConfig;

namespace Firefly.Server.Core.Entities
{
    public class FireflyConfig : IConfig
    {
        public RemoteTopConfig Remote { get; set; }
        public LocalTopConfig Local { get; set; }

        public bool Validate(ref List<string> messages) {
            bool validationPassed = true;

            if (!Local.Validate(ref messages)) validationPassed = false;
            if (!Remote.Validate(ref messages)) validationPassed = false;
            
            return validationPassed;
        }
    }
}
