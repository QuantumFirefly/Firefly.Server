using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firefly.Server.Core.Database.Repositories.Queries;

namespace Firefly.Server.Core.Database.Repositories.Queries.PostgreSQL
{
    internal class UserQueries : IUserQueries
    {
        public string GetByUsername() => "SELECT * FROM Users WHERE Username = @username";
    }
}
