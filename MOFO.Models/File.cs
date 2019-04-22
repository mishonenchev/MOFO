using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Models
{
    public class File
    {
        [Key]
        public int Id { get; set; }
        public Type Type { get; set; }
        public string FileName { get; set; }
        public string DownloadCode { get; set; }
        public string Message { get; set; }
        public User User { get; set; }
        public DateTime DateTimeUploaded { get; set; }
    }
    public enum Type
    {
        File,
        Text
    }
}
