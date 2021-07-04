using Flights.Models;
using Flights.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Services
{
    public interface IReservationService
    {
        public List<Destination> GetDestinationList();
        public Task<int> AddReservation(ReservationVM model);
        public List<ReservationVM> ReservationsForUpcomingFlights();
        public List<ReservationVM> ReservationsByStatus(string status);
        public List<ReservationVM> VisitorReservations(string id);
        public Task<int> EditReservation(int id);
        public ReservationVM ReservationById(int id);
        public Task<int> DeleteById(int id);
    }
}
