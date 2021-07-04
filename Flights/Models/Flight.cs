using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Models
{
    public class Flight
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Destination to")]
        public int DestinationToId { get; set; }
        [Required]
        [Display(Name = "Destination from")]
        public int DestinationFromId { get; set; }
        [Required]
        [Display(Name = "Available seats")]
        public int AvailableSeats { get; set; }
        [Required]
        [Display(Name = "Number of connections")]
        public string NumberOfConnections { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date")]
        public DateTime FlightDate { get; set; }
    }
}
