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
    public class CardRepository : BaseRepository<Card>, ICardRepository
    {
        public CardRepository(IDatabase database) : base(database)
        {
        }
        public IEnumerable<Card> WhereIncludeAll(Expression<Func<Card, bool>> where)
        {
            return _dbSet.Where(where).Include(x => x.Room).Include(x => x.School.City).Include(x => x.User);
        }
    }
}
