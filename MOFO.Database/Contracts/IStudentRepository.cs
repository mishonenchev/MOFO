using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Database.Contracts
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        IEnumerable<Student> WhereIncludeAll(Expression<Func<Student, bool>> where);
    }
}
