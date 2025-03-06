using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firefly.Server.Core.Database.Repository.Queries;

namespace Firefly.Server.Core.Database.Repository.Queries.PostgreSQL
{
    internal class ConfigQueries : IConfigQueries
    {
        public string GetAll() => "SELECT UpperKey, JsonData FROM Config";
    }
}
