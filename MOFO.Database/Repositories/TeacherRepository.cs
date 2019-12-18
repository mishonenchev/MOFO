using MOFO.Database.Contracts;
using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Database.Repositories
{
    public class TeacherRepository : BaseRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(IDatabase database) : base(database)
        {
        }
    }
}
