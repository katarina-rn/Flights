using Flights.Models;
using Flights.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Services
{
    public class FlightService : IFlightService
    {
        private readonly ApplicationDbContext _db;
        public FlightService(ApplicationDbContext db)
        {
            _db = db;
        }

        public Flight AddFlight(FlightVM model)
        {
            if(model != null)
            {
                int id;
                if (!_db.Flights.Any())
                {
                    id = 1;
                }
                else
                {
                    id = _db.Flights.Max(f => f.Id);
                    id++;
                }
                Flight flight = new Flight()
                {
                    Id = id,
                    DestinationFromId = model.DestinationFrom.Id,
                    DestinationToId = model.DestinationTo.Id,
                    AvailableSeats = model.AvailableSeats,
                    NumberOfConnections = model.NumberOfConnections,
                    FlightDate = model.FlightDate
                };
                return flight;
            }
            return null;
            
        }

        public List<FlightVM> AllFlights()
        {
            var flights = (from f in _db.Flights
                           join d in _db.Destinations on f.DestinationFromId equals d.Id
                           join d2 in _db.Destinations on f.DestinationToId equals d2.Id
                           select new FlightVM 
                           { 
                               Id = f.Id,
                               DestinationFrom = new Destination
                               {
                                   Id = f.DestinationFromId,
                                   Name = d.Name
                               },
                               DestinationTo = new Destination
                               {
                                   Id = f.DestinationToId,
                                   Name = d2.Name
                               },
                               AvailableSeats = f.AvailableSeats,
                               NumberOfConnections = f.NumberOfConnections,
                               FlightDate = f.FlightDate
                           }).ToList();
            return flights;
        }

        public async Task<int> DeleteById(int id)
        {
            var flight = _db.Flights.FirstOrDefault(fl => fl.Id == id);
            if(flight != null)
            {
                _db.Flights.Remove(flight);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public FlightVM FlightById(int id)
        {
            var flight = (from f in _db.Flights.Where(fl => fl.Id == id)
                          join d in _db.Destinations on f.DestinationFromId equals d.Id
                          join d2 in _db.Destinations on f.DestinationToId equals d2.Id
                          select new FlightVM
                          {
                              Id = f.Id,
                              DestinationFrom = new Destination
                              {
                                  Id = f.DestinationFromId,
                                  Name = d.Name
                              },
                              DestinationTo = new Destination
                              {
                                  Id = f.DestinationToId,
                                  Name = d2.Name
                              },
                              AvailableSeats = f.AvailableSeats,
                              NumberOfConnections = f.NumberOfConnections,
                              FlightDate = f.FlightDate
                          }).FirstOrDefault();
            return flight;
        }

        public List<Destination> GetDestinationList()
        {
            if (!_db.Destinations.Any())
            {
                _db.Destinations.Add(new Destination { Name = "Beograd" });
                _db.Destinations.Add(new Destination { Name = "Nis" });
                _db.Destinations.Add(new Destination { Name = "Novi Sad" });
                _db.Destinations.Add(new Destination { Name = "Pristina" });
                _db.Destinations.Add(new Destination { Name = "Kraljevo" });
                _db.SaveChanges();
            }
            var destinations = (from d in _db.Destinations select new Destination { Id = d.Id, Name = d.Name }).ToList();
            return destinations;
        }

        public List<FlightVM> NextFlights()
        {
            var flights = (from f in _db.Flights.Where(fl => fl.FlightDate > DateTime.Now).OrderBy(fl=>fl.FlightDate)
                           join d in _db.Destinations on f.DestinationFromId equals d.Id
                           join d2 in _db.Destinations on f.DestinationToId equals d2.Id
                           select new FlightVM
                           {
                               Id = f.Id,
                               DestinationFrom = new Destination
                               {
                                   Id = f.DestinationFromId,
                                   Name = d.Name
                               },
                               DestinationTo = new Destination
                               {
                                   Id = f.DestinationToId,
                                   Name = d2.Name
                               },
                               AvailableSeats = f.AvailableSeats,
                               NumberOfConnections = f.NumberOfConnections,
                               FlightDate = f.FlightDate
                           }).ToList();
            return flights;
        }

        public List<FlightVM> SearchFLights(string destinationFrom, string destinationTo, string date, bool connections)
        {
            var flights = new List<FlightVM>();
            if (connections)
            {
                flights = (from f in _db.Flights.Where(fl => fl.FlightDate.Date == DateTime.ParseExact(date, "yyyy-MM-dd", null) && fl.AvailableSeats > 0 &&Convert.ToInt32(fl.NumberOfConnections) == 0)
                           join d in _db.Destinations.Where(des => des.Name == destinationFrom) on f.DestinationFromId equals d.Id
                           join d2 in _db.Destinations.Where(des2 => des2.Name == destinationTo) on f.DestinationToId equals d2.Id
                           select new FlightVM
                           {
                               Id = f.Id,
                               DestinationFrom = new Destination
                               {
                                   Id = f.DestinationFromId,
                                   Name = d.Name
                               },
                               DestinationTo = new Destination
                               {
                                   Id = f.DestinationToId,
                                   Name = d2.Name
                               },
                               AvailableSeats = f.AvailableSeats,
                               NumberOfConnections = f.NumberOfConnections,
                               FlightDate = f.FlightDate
                           }).ToList();
            }
            else
            {
                flights = (from f in _db.Flights.Where(fl => fl.FlightDate.Date == DateTime.ParseExact(date, "yyyy-MM-dd", null) && fl.AvailableSeats > 0)
                               join d in _db.Destinations.Where(des => des.Name == destinationFrom) on f.DestinationFromId equals d.Id
                               join d2 in _db.Destinations.Where(des2 => des2.Name == destinationTo) on f.DestinationToId equals d2.Id
                               select new FlightVM
                               {
                                   Id = f.Id,
                                   DestinationFrom = new Destination
                                   {
                                       Id = f.DestinationFromId,
                                       Name = d.Name
                                   },
                                   DestinationTo = new Destination
                                   {
                                       Id = f.DestinationToId,
                                       Name = d2.Name
                                   },
                                   AvailableSeats = f.AvailableSeats,
                                   NumberOfConnections = f.NumberOfConnections,
                                   FlightDate = f.FlightDate
                               }).ToList();
            }
            
            return flights;
        }

    }
}
