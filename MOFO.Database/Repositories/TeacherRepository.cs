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
    public class TeacherRepository : BaseRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(IDatabase database) : base(database)
        {
        }
        public IEnumerable<Teacher> WhereIncludeAll(Expression<Func<Teacher, bool>> where)
        {
            return _dbSet.Where(where).Include(x => x.User).Include(x=>x.School).ToList();
        }
    }
}
