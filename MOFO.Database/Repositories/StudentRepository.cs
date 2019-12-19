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
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(IDatabase database) : base(database)
        {
        }
        public IEnumerable<Student> WhereIncludeAll(Expression<Func<Student, bool>> where)
        {
            return _dbSet.Where(where).Include(x=>x.User).ToList();
        }
    }
}
