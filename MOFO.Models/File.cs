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
        public string FileName { get; set; }
        public string DownloadCode { get; set; }
        public string Size { get; set; }
        public DateTime DateTimeUploaded { get; set; }
    }
}
