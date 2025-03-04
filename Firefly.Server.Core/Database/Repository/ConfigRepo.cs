using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Database.Repository
{
    internal class ConfigRepo
    {
        private DbConnection _db;
        public ConfigRepo(DbConnection db) {
            _db = db;
        }


        // TODO - GetAll(), Get(), SetAll(), Set()
        //  Deserialises specific fields.
    }
}
