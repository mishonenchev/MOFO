using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Models
{
    public class Room
    {
        public Room()
        {
            Cards = new List<Card>();
        }
        [Key]
        public int Id { get; set; }
        public string Name{ get; set; }
        public ICollection<Card> Cards { get; set; }
        public School School { get; set; }
        public string Description { get; set; }
    }
}
