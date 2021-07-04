using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int FlightId { get; set; }
        [Required]
        public string VisitorId { get; set; }
        [ForeignKey("VisitorId")]
        public ApplicationUser Visitor { get; set; }
        [ForeignKey("FlightId")]
        public Flight Flight { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
