using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firefly.Server.Core.Database.Repository.Queries;
using Firefly.Server.Core.Entities.RemoteConfig;

namespace Firefly.Server.Core.Database.Repository
{
    public class ConfigRepo(DbConnection Db)
    {
        private readonly IConfigQueries _queries = new QueryFactory(Db.DBMS).GetConfigQueries();

        public async Task<RemoteConfig> GetAll() {
            return await Db.JsonGet<RemoteConfig>(_queries.GetAll());
        }

    }
}
