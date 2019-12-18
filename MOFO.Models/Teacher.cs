using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }
        public School School { get; set; }
        public bool IsVerified { get; set; }
        public User User { get; set; }
        public DateTime RegisteredDateTime { get; set; }
        public DateTime VerificationDateTime { get; set; }
    }
}
