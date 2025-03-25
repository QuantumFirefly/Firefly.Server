using Firefly.Server.Core.Enums;

namespace Firefly.Server.Core.Database.Repositories.Queries
{
    public class QueryFactory(EnumDBMS DBMS)
    {
        public IConfigQueries GetConfigQueries() {
            return DBMS switch {
                EnumDBMS.PostgreSQL => new PostgreSQL.ConfigQueries(),
                _ => throw new NotSupportedException("Unsupported DBMS type")
            };
        }

        public IUserQueries GetUserQueries() {
            return DBMS switch {
                EnumDBMS.PostgreSQL => new PostgreSQL.UserQueries(),
                _ => throw new NotSupportedException("Unsupported DBMS type")
            };
        }
    }
}
