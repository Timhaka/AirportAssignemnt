using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AirportSystemAssign.Models
{
    public class Airplane
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Size { get; set; }
        public int MaxNrOfPassengers { get; set; }
        public int? AirportId { get; set; }
        public Airport Airport { get; set; }

        public int? PilotId { get; set; }
        public Pilot Pilot { get; set; }
        public int? CoPilotId { get; set; }
        public Pilot CoPilot { get; set; }

        public int? AirplaneTypeId { get; set; }
        public AirplaneType AirplaneType { get; set; }
    }
}