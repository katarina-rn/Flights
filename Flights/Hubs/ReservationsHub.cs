using Flights.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Hubs
{
    public class ReservationsHub : Hub
    {
        public async Task AddReservation(Reservation reservation)
        {
            await Clients.All.SendAsync("AddReservation", reservation);
        }

        public async Task EditReservation(Reservation reservation)
        {
            await Clients.All.SendAsync("EditReservation", reservation);
        }

        public async Task DeleteReservation(Reservation reservation)
        {
            await Clients.All.SendAsync("DeleteReservation", reservation);
        }
    }
}
