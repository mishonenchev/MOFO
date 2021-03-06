﻿using MOFO.Database.Contracts;
using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Database.Repositories
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        public RoomRepository(IDatabase database) : base(database)
        {
        }
        public IEnumerable<Room> WhereIncludeAll(Expression<Func<Room, bool>> where)
        {
            return _dbSet.Where(where).Include(x=>x.Desks.Select(y=>y.User)).ToList();
        }
    }
}
