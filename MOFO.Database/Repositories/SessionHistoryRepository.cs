using MOFO.Database.Contracts;
using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Database.Repositories
{
    public class SessionHistoryRepository : BaseRepository<SessionHistory>, ISessionHistoryRepository
    {
        public SessionHistoryRepository(IDatabase database) : base(database)
        {
        }
    }
}
