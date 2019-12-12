using System;
using System.Collections.Generic;
using System.Linq;
using MOFO.Models;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using MOFO.Database.Contracts;
using System.Linq.Expressions;

namespace MOFO.Database.Repositories
{
    public class FileRepository : BaseRepository<File>, IFileRepository
    {
        public FileRepository(IDatabase database) : base(database)
        {
        }
        public IEnumerable<File> WhereIncludeAll(Expression<Func<File, bool>> where)
        {
            return _dbSet.Where(where).Include(x => x.User).AsEnumerable();
        }
    }
}
