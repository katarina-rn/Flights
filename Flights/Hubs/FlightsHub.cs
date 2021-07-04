using Flights.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Hubs
{
    public class FlightsHub : Hub
    {
        public async Task NewFlightAdded(Flight flight)
        {
            await Clients.All.SendAsync("NewFlightAdded", flight);
        }
    }
}
