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
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public UserRole Role { get; set; }
        public string Auth { get; set; }
        public Session Session { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateTimeLastActive { get; set; }
    }

    public enum UserRole
    {
        Teacher,
        Student
    }
}
