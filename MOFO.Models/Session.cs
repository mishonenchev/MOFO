using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Models
{
    public class Session
    {
        [Key]
        public int Id { get; set; }
        public ICollection<File> Files { get; set; }
        public Room Room { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeLastActive { get; set; }
    }
}
