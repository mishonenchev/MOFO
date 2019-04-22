using MOFO.Database.Contracts;
using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Database.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IDatabase database) : base(database)
        {
        }
        public IEnumerable<User> WhereIncludeAll(Expression<Func<User, bool>> where)
        {
            return Where(where, x=>x.Session).AsEnumerable();
        }
    }
}
