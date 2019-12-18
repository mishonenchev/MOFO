using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public DateTime RegisterDateTime { get; set; }
    }
}
