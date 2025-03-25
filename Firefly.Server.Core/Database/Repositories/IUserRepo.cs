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
    public interface IUserRepo
    {
        public Task<User?> GetByUsername(string username);

    }
}
