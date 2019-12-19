using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Models
{
   public class Moderator
    {
        [Key]
        public int Id { get; set; }
        public string Auth { get; set; }
        public string AspUserId { get; set; }
        public School School { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public DateTime RegisteredDateTime { get; set; }
        public DateTime VerificationDateTime { get; set; }
        public bool IsVerified { get; set; }
    }
}
