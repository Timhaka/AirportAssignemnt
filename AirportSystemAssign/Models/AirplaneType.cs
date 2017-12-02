using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AirportSystemAssign.Models
{
    public class AirplaneType
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual List<Airplane> Airplane { get; set; }
        public virtual List<Pilot> Pilot { get; set; }
    }
}