using MOFO.Database.Contracts;
using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Database.Repositories
{
    public class SchoolRepository : BaseRepository<School>, ISchoolRepository
    {
        public SchoolRepository(IDatabase database) : base(database)
        {
        }
    }
}
