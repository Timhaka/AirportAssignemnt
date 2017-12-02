using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AirportSystemAssign.Models
{
    public class Pilot
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual List<Airplane> Airplane { get; set; }
       
        public virtual List<AirplaneType> AirplaneTypes { get; set; }
    }
}