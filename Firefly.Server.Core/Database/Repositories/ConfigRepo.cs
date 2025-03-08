using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firefly.Server.Core.Database.Repositories.Queries;
using Firefly.Server.Core.Entities.RemoteConfig;

namespace Firefly.Server.Core.Database.Repositories
{
    public class ConfigRepo(DbConnection Db)
    {
        private readonly IConfigQueries _queries = new QueryFactory(Db.DBMS).GetConfigQueries();

        public async Task<RemoteTopConfig> GetAll() {
            return await Db.JsonGet<RemoteTopConfig>(_queries.GetAll()) ?? throw new Exception("Null JSON returned from database when accessing RemoteConfig.");
        }

    }
}
