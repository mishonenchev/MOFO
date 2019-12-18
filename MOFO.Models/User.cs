using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Models
{
    public class User
    {
        public User()
        {
            SessionHistories = new List<SessionHistory>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public UserRole Role { get; set; }
        public string Auth { get; set; }
        public string AspUserId { get; set; }
        public Session Session { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateTimeLastActive { get; set; }
        public DateTime DateTimeRegistered { get; set; }
        public IEnumerable<SessionHistory> SessionHistories { get; set; }
    }

    public enum UserRole
    {
        Teacher,
        Student
    }
}
