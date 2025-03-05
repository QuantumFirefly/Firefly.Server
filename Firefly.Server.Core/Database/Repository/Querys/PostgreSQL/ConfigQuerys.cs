using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Database.Repository.Querys.PostgreSQL
{
    internal class ConfigQuerys
    {
        public string GetAll = "SELECT UpperKey, JsonData FROM Config";
    }
}
