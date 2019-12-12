using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Database.Contracts
{
    public interface IFileRepository: IBaseRepository<File>
    {
        IEnumerable<File> WhereIncludeAll(Expression<Func<File, bool>> where);
    }
}
