using MOFO.Database.Contracts;
using MOFO.Database.Repositories;
using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Database
{
    public class CityRepository : BaseRepository<City>, ICityRepository
    {
        public CityRepository(IDatabase database) : base(database)
        {
        }
       
    }
}
