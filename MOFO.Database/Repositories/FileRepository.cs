using System;
using System.Collections.Generic;
using System.Linq;
using MOFO.Models;
using System.Text;
using System.Threading.Tasks;
using MOFO.Database.Contracts;

namespace MOFO.Database.Repositories
{
    public class FileRepository : BaseRepository<File>, IFileRepository
    {
        public FileRepository(IDatabase database) : base(database)
        {
        }
    }
}
