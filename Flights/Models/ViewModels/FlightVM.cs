using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Models.ViewModels
{
    public class FlightVM
    {
        public int? Id { get; set; }
        [Required]
        [Display(Name = "Destination from")]
        public Destination DestinationFrom { get; set; }
        [Required]
        [Display(Name = "Destination to")]
        public Destination DestinationTo { get; set; }
        [Required]
        [Display(Name = "Available seats")]
        public int AvailableSeats { get; set; }
        [Required]
        [Display(Name = "Number of connections")]
        public string NumberOfConnections { get; set; }
        [Display(Name = "Date")]
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime flightDate;
        public DateTime FlightDate {
            get
            {
                return (flightDate == DateTime.MinValue) ? DateTime.Now.Date  : flightDate;
            }
            set { flightDate = value; }
        }
    }
}
