﻿using System;
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
            Desks = new List<Desk>();
        }
        [Key]
        public int Id { get; set; }
        public string Name{ get; set; }
        public ICollection<Desk> Desks { get; set; }
    }
}
