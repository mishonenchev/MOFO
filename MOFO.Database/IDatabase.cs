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
        DbSet<Card> Cards { get; set; }
        DbSet<File> Files { get; set; }
        DbSet<Room> Rooms { get; set; }
        DbSet<Session> Sessions { get; set; }
        DbSet<SessionHistory> SessionHistories { get; set; }
        DbSet<Message> Messages { get; set; }
        DbSet<School> Schools { get; set; }
        DbSet<Teacher> Teachers { get; set; }
        DbSet<Moderator> Moderators { get; set; }
        DbSet<T> Set<T>() where T : class;
        void SaveChanges();
    }
}
