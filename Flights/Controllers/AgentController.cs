using Flights.Hubs;
using Flights.Models;
using Flights.Models.ViewModels;
using Flights.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Controllers
{
    public class AgentController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IFlightService _flightService;
        private readonly IReservationService _reservationService;
        private readonly IHubContext<FlightsHub> _hubContext;
        public AgentController(ApplicationDbContext db, IFlightService flightService, IReservationService reservationService, IHubContext<FlightsHub> hubContext)
        {
            _db = db;
            _flightService = flightService;
            _reservationService = reservationService;
            _hubContext = hubContext;
        }
        public IActionResult Index()
        {
            List<FlightVM> flights = _flightService.NextFlights();
            return View(flights);
        }

        public IActionResult Reservations()
        {
            return View();
        }

        public IActionResult CreateFlight()
        {
            FlightVM flightVM = new FlightVM
            {
                DestinationFrom = new Destination(),
                DestinationTo = new Destination()
            };
            List<SelectListItem> dest = _flightService.GetDestinationList().ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                };
            });
            ViewBag.Destinations = dest;
            return View(flightVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFlight(FlightVM obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.DestinationFrom.Id == obj.DestinationTo.Id)
                {
                    ModelState.AddModelError("", "Same destinations");
                    return View(obj);
                }
                Flight f = _flightService.AddFlight(obj);
                using (var transaction = _db.Database.BeginTransaction())
                {
                    _db.Database.ExecuteSqlRaw("set identity_insert Flights on");
                    _db.Flights.Add(f);
                    await _db.SaveChangesAsync();
                    _db.Database.ExecuteSqlRaw("set identity_insert Flights off");
                    transaction.Commit();
                   
                }
                await _hubContext.Clients.All.SendAsync("NewFlightAdded", f);
            }
            return RedirectToAction("Index");
        }
    }
}
