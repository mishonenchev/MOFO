using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Database.Contracts
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        IEnumerable<Message> WhereIncludeAll(System.Linq.Expressions.Expression<Func<Message, bool>> where);
    }
}
