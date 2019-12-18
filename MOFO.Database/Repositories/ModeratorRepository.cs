using MOFO.Database.Contracts;
using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Database.Repositories
{
    public class ModeratorRepository : BaseRepository<Moderator>, IModeratorRepository
    {
        public ModeratorRepository(IDatabase database) : base(database)
        {
        }
    }
}
