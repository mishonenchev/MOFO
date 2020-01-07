using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Database.Contracts
{
    public interface ISessionRepository : IBaseRepository<Session>
    {
        IEnumerable<Session> WhereIncludeAll(System.Linq.Expressions.Expression<Func<Session, bool>> where);
    }
}
