using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firefly.Server.Core.Entitys;

namespace Firefly.Server.Core.Database.Repository
{
    public class ConfigRepo(DbConnection db)
    {
        private readonly DbConnection _db;

        public Config GetAll() {
            
            // Call DB. Get Query from Query Area
        }

        // TODO - GetAll(), Get(), SetAll(), Set()
        //  Deserialises specific fields.
    }
}
