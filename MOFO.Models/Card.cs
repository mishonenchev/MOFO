using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Models
{
    public class Card
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public UserRole Role { get; set; }
        public User User { get; set; }
        public School School { get; set; }
        public string QRCode { get; set; }
        public DateTime ValidBefore { get; set; }
    }
}
