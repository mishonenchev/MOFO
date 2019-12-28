using MOFO.Database.Contracts;
using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;

namespace MOFO.Database.Repositories
{
    public class SchoolRepository : BaseRepository<School>, ISchoolRepository
    {
        public SchoolRepository(IDatabase database) : base(database)
        {
        }
        public IEnumerable<School> SearchSchool(string name)
        {
            IQueryable<School> query = _dbSet;
            if (!string.IsNullOrEmpty(name))
            {
                var keywords = name.Split(' ');
                foreach (var item in keywords)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        query = query.Where(x => x.Name.ToLower().Contains(item.ToLower()));
                    }
                }
            }
            return query.Include(x=>x.City).ToList();
        }
        public IEnumerable<School> WhereIncludeAll(Expression<Func<School, bool>> where)
        {
            return _dbSet.Where(where).Include(x => x.City);

        }
    }
}
