using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Entities
{
    public interface IConfig
    {
        bool Validate(ref List<string> messages);
    }
}
