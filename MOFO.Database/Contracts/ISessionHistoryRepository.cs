using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Database.Contracts
{
    public interface ISessionHistoryRepository : IBaseRepository<SessionHistory>
    {
        IEnumerable<SessionHistory> WhereIncludeAll(System.Linq.Expressions.Expression<Func<SessionHistory, bool>> where);
    }
}
