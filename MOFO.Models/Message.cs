using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Models
{
   public class Message
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public File File { get; set; }
        public DateTime DateTimeUploaded { get; set; }
        public User User { get; set; }
        public MessageType Type { get; set; }
        public Session Session { get; set; }
    }
    public enum MessageType
    {
        file,
        text
    }
}
