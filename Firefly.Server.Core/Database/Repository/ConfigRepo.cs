using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firefly.Server.Core.Database.Repository.Queries;
using Firefly.Server.Core.Entitys;

namespace Firefly.Server.Core.Database.Repository
{
    public class ConfigRepo
    {
        private readonly DbConnection _db;
        private readonly IConfigQueries _queries;
        public ConfigRepo(DbConnection db) {
            _db = db;

            var queryFactory = new QueryFactory(_db.DBMS);
            _queries = queryFactory.GetConfigQueries();
        }

        public Config GetAll() {
            
            // TODO - Implement Dapper to drop this into Config entity?
            // Call DB. Get Query from Query Area
        }

        // TODO - GetAll(), Get(), SetAll(), Set()
        //  Deserialises specific fields.
    }
}
