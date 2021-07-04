using Flights.Models;
using Flights.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Services
{
    public interface IFlightService
    {
        public List<Destination> GetDestinationList();
        public Flight AddFlight(FlightVM model);
        public List<FlightVM> AllFlights();
        public List<FlightVM> NextFlights();
        public List<FlightVM> SearchFLights(string destinationFrom, string destinationTo, string date, bool connections);
        public FlightVM FlightById(int id);
        public Task<int> DeleteById(int id);
    }
}
