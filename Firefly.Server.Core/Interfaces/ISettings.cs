using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Interfaces
{
    public interface ISettings
    {
        bool Validate(ref List<string> messages);
    }
}
