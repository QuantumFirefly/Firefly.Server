using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Database.Repositories
{
    public class DbContext(IConfigRepo configRepo, IUserRepo userRepo) : IDbContext
    {
        public IConfigRepo Config { get; } = configRepo;
        public IUserRepo User { get;  } = userRepo;
    }
}
