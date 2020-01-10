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
    public class SessionHistoryRepository : BaseRepository<SessionHistory>, ISessionHistoryRepository
    {
        public SessionHistoryRepository(IDatabase database) : base(database)
        {
        }
        public IEnumerable<SessionHistory> WhereIncludeAll(Expression<Func<SessionHistory, bool>> where)
        {
            return _dbSet.Where(where).Include(x => x.Messages.Select(y=>y.File)).Include(x => x.Messages.Select(y => y.User)).Include(x=>x.Users).Include(x=>x.Room).ToList();
        }
    }
}
