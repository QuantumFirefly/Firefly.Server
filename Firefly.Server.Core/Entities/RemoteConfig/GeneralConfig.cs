using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Entities.RemoteConfig
{
    public class GeneralConfig: IConfig
    {
        public required string ServerName { get; set; }
        public required string ServerDomain { get; set; }

        public bool Validate(ref List<string> messages) {
            bool validationPassed = true;

            if (ServerName.Length < 3) {
                messages.Add($"Server Name should be 3 or more characters.");
                validationPassed = false;
            }

            if (ServerDomain.Length < 3) {
                messages.Add($"Server Domain should be 3 or more characters.");
                validationPassed = false;
            }

            return validationPassed;
        }
    }
}
