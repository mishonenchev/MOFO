using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Models
{
    public class SessionHistory
    {
        public SessionHistory()
        {
            Users = new List<User>();
            Files = new List<File>();
        }
        [Key]
        public int Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime FinishDateTime { get; set; }
        public IEnumerable<User> Users { get; set; }
        public Room Room { get; set; }
        public IEnumerable<File> Files { get; set; }
    }
}
