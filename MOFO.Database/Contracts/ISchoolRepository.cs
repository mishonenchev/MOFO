using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Database.Contracts
{
    public interface ISchoolRepository : IBaseRepository<School>
    {
        IEnumerable<School> SearchSchool(string name);
        IEnumerable<School> WhereIncludeAll(System.Linq.Expressions.Expression<Func<School, bool>> where);
    }
}
