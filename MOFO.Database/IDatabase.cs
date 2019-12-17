using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Database
{
    public interface IDatabase
    {
        DbSet<User> Users { get; set; }
        DbSet<Desk> Desks { get; set; }
        DbSet<File> Files { get; set; }
        DbSet<Room> Rooms { get; set; }
        DbSet<Session> Sessions { get; set; }
        DbSet<T> Set<T>() where T : class;
        void SaveChanges();
    }
}
