using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Database.Contracts
{
    public interface ITeacherRepository : IBaseRepository<Teacher>
    {
        IEnumerable<Teacher> WhereIncludeAll(Expression<Func<Teacher, bool>> where);
    }
}
