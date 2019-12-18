using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Database
{
    public class Database : DbContext, IDatabase
    {
        public Database() : base("DefaultConnection")
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionHistory> SessionHistories { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Moderator> Moderators { get; set; }
        public void SaveChanges()
        {
            base.SaveChanges();
        }
    }
}