using MOFO.Database.Contracts;
using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Database.Repositories
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(IDatabase database) : base(database)
        {
        }
    }
}
