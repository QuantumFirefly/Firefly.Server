using Firefly.Server.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Entitys;
    public class LogSettings : ISettings
{
    public bool Validate(ref List<string> messages)
    {
        bool validationPassed = true;


        return validationPassed;
    }
}
