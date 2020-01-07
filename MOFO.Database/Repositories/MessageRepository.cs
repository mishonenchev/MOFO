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
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(IDatabase database) : base(database)
        {
         
        }
        public IEnumerable<Message> WhereIncludeAll(Expression<Func<Message, bool>> where)
        {
            return _dbSet.Where(where).Include(x=>x.User).Include(x=>x.File).Include(x=>x.Session).AsEnumerable();
        }
    }
}
