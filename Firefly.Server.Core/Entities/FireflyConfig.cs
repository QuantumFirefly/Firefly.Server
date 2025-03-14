using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firefly.Server.Core.Entities.LocalConfig;
using Firefly.Server.Core.Entities.RemoteConfig;

namespace Firefly.Server.Core.Entities
{
    public class FireflyConfig : IFireflyConfig
    {
        public RemoteTopConfig? Remote { get; set; }
        public LocalTopConfig? Local { get; set; }

        public bool Validate(ref List<string> messages) {
            bool validationPassed = true;

            if (Local != null && !Local.Validate(ref messages)) validationPassed = false;
            if (Remote != null && !Remote.Validate(ref messages)) validationPassed = false;

            if (Local == null) {
                validationPassed = false;
                messages.Add("Local Settings Missing from FireflyConfig.");
            }

            if (Remote == null) {
                validationPassed = false;
                messages.Add("Remote Settings Missing from FireflyConfig.");
            }

            return validationPassed;
        }
    }
}
