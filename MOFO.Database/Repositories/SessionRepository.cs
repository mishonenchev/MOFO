using MOFO.Database.Contracts;
using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;
namespace MOFO.Database.Repositories
{
    public class SessionRepository : BaseRepository<Session>, ISessionRepository
    {
        public SessionRepository(IDatabase database) : base(database)
        {
        }
        public IEnumerable<Session> WhereIncludeAll(Expression<Func<Session, bool>> where)
        {
            return _dbSet.Where(where).Include(x => x.Messages.Select(y=>y.File)).Include(x=>x.Room).ToList();
        }
    }
}
