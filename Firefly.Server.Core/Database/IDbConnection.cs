using Firefly.Server.Core.Entities.LocalConfig;
using Firefly.Server.Core.Enums;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Database
{
    public interface IDbConnection : IDisposable
    {
        public Task<T?> JsonGetAsync<T>(string query);
    }
}
