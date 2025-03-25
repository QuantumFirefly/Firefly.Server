using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firefly.Server.Core.Database.Repositories.Queries;
using Firefly.Server.Core.Entities;
using Firefly.Server.Core.Entities.RemoteConfig;

namespace Firefly.Server.Core.Database.Repositories
{
    public class UserRepo(IDbConnection Db) : IUserRepo
    {
        private readonly IUserQueries _queries = new QueryFactory(Db.DBMS).GetUserQueries();

        public async Task<User?> GetByUsername(string username) { 
            string query = _queries.GetByUsername();

            var parameters = new { username = username };

            return await Db.QuerySingleOrDefaultAsync<User?>(query, parameters);
        }

    }
}
