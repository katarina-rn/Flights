using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Models.ViewModels
{
    public class ReservationVM
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public string DestinationFrom { get; set; }
        public string DestinationTo { get; set; }
        public string PassangerId { get; set; }
        public ApplicationUser Passanger { get; set; }
        public Flight FlightR { get; set; }
        public string Status { get; set; }
    }
}
