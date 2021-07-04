using Flights.Hubs;
using Flights.Models;
using Flights.Models.ViewModels;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHubContext<ReservationsHub> _hubContext;
        public ReservationService(ApplicationDbContext db, IHubContext<ReservationsHub> hubContext)
        {
            _db = db;
            _hubContext = hubContext;
        }

        public async Task<int> AddReservation(ReservationVM model)
        {
            if(model != null)
            {
                Reservation reservation = new Reservation()
                {
                    FlightId = model.FlightR.Id,
                    VisitorId = model.Passanger.Id,
                    Status = model.Status
                };
                _db.Reservations.Add(reservation);
                await _db.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("AddReservation", reservation);
                return 1;
            }
            return 0;
        }

        public List<ReservationVM> ReservationsForUpcomingFlights()
        {
            var reservations = (from r in _db.Reservations
                                join f in _db.Flights on r.Flight.Id equals f.Id
                                join d1 in _db.Destinations on f.DestinationFromId equals d1.Id
                                join d2 in _db.Destinations on f.DestinationToId equals d2.Id
                                join u in _db.Users on r.Visitor.Id equals u.Id
                                select new ReservationVM
                                {
                                    Id = r.Id,
                                    DestinationFrom = d1.Name,
                                    DestinationTo = d2.Name,
                                    FlightR = new Flight
                                    {
                                        Id = f.Id,
                                        AvailableSeats = f.AvailableSeats,
                                        NumberOfConnections = f.NumberOfConnections,
                                        FlightDate = f.FlightDate
                                    },
                                    Passanger = new ApplicationUser
                                    {
                                        Id = u.Id,
                                        Name = u.Name
                                    },
                                    Status = r.Status
                                }).ToList();
            return reservations;
        }

        public async Task<int> DeleteById(int id)
        {
            var reservation = _db.Reservations.FirstOrDefault(res => res.Id == id);
            if (reservation != null)
            {
                _db.Reservations.Remove(reservation);
                await _db.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("DeleteReservation", reservation);
                return 1;
            }
            return 0;
        }

        public async Task<int> EditReservation(int id)
        {
            var reservation = _db.Reservations.FirstOrDefault(r => r.Id == id);
            if(reservation != null)
            {
                reservation.Status = "Aproved";
                await _db.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("EditReservation", reservation);
                return 1;
            }
            return 0;
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

        public ReservationVM ReservationById(int id)
        {
            throw new NotImplementedException();
        }

        public List<ReservationVM> ReservationsByStatus(string status)
        {
            throw new NotImplementedException();
        }

        public List<ReservationVM> VisitorReservations(string id)
        {
            var reservations = (from r in _db.Reservations.Where(res => res.VisitorId == id).OrderBy(res => res.Flight.FlightDate)
                                join f in _db.Flights on r.Flight.Id equals f.Id
                                join d1 in _db.Destinations on f.DestinationFromId equals d1.Id
                                join d2 in _db.Destinations on f.DestinationToId equals d2.Id
                                join u in _db.Users on r.Visitor.Id equals u.Id
                                select new ReservationVM
                                {
                                    Id = r.Id,
                                    DestinationFrom = d1.Name,
                                    DestinationTo = d2.Name,
                                    FlightR = new Flight
                                    {
                                        Id = f.Id,
                                        AvailableSeats = f.AvailableSeats,
                                        NumberOfConnections = f.NumberOfConnections,
                                        FlightDate = f.FlightDate
                                    },
                                    Passanger = new ApplicationUser
                                    {
                                        Id = u.Id,
                                        Name = u.Name
                                    },
                                    Status = r.Status
                                }).ToList();
            return reservations;
        }
    }
}
