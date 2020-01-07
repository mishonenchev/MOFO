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
        public Session()
        {
            Messages = new List<Message>();
        }
        [Key]
        public int Id { get; set; }
        public ICollection<Message> Messages { get; set; }
        public Room Room { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeLastActive { get; set; }
        public bool IsActive { get; set; }
    }
}
