using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Database.Repositories
{
    public interface IDbContext
    {
        public IConfigRepo Config { get; }
        public IUserRepo User { get; }
    }
}
