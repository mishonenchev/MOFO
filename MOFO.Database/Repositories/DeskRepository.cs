using MOFO.Database.Contracts;
using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MOFO.Database.Repositories
{
    public class DeskRepository : BaseRepository<Desk>, IDeskRepository
    {
        public DeskRepository(IDatabase database) : base(database)
        {
        }
        public IEnumerable<Desk> WhereIncludeAll(Expression<Func<Desk, bool>> where)
        {
            return Where(where, x => x.User).AsEnumerable();
        }
    }
}
