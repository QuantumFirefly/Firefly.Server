using Firefly.Server.Core.Entities.LocalConfig;
using Firefly.Server.Core.Entities.RemoteConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Entities
{
    public  interface IFireflyConfig : IConfig
    {
        public RemoteTopConfig? Remote { get; set; }
        public LocalTopConfig? Local { get; set; }
    }
}
