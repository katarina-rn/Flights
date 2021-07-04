
using Flights.Models;
using Flights.Models.ViewModels;
using Flights.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Flights.Controllers
{
    [ApiController]
    [Route("/api/reservations")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IFlightService _flightService;
        private readonly UserManager<ApplicationUser> _userManager;
        public ReservationController(IReservationService reservationService, IFlightService flightService, UserManager<ApplicationUser> userManager)
        {
            _reservationService = reservationService;
            _flightService = flightService;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("getReservations")]
        public IActionResult GetReservations()
        {
            var response = _reservationService.ReservationsForUpcomingFlights();
            return Ok(response);
        }

        [HttpGet]
        [Route("visitorReservations/{id}")]
        public IActionResult VisitorReservations(string id)
        {
            var response = _reservationService.VisitorReservations(id);
            return Ok(response);
        }
        
        [HttpPost]
        [Route("addReservation/{id}")]
        public IActionResult AddReservation(int id)
        {
            var flight = _flightService.FlightById(id);
            if(flight.FlightDate <= DateTime.Now.AddDays(3))
            {
                ModelState.AddModelError("", "Cant add reservation if flight is in less than 3 days");
                return Json(new { status = "error", message = "Ne mozete rezervisati mesto na letu koji je za manje od 3 dana" });
            }
            ReservationVM obj = new ReservationVM
            {
                FlightR = new Flight
                {
                    Id = id
                },
                Passanger = new ApplicationUser
                {
                    Id = _userManager.GetUserId(User)
                },
                Status = "Pending"
            };
            var response = _reservationService.AddReservation(obj);
            return Json(JsonSerializer.Serialize(response));
        }

        [HttpPatch]
        [Route("editReservations/{id}")]
        public IActionResult EditReservations(int id)
        {
            var response = _reservationService.EditReservation(id);
            
            return Json(JsonSerializer.Serialize(response));
        }

        [HttpDelete]
        [Route("deleteReservations/{id}")]
        public IActionResult DeleteReservations(int id)
        {
            var response = _reservationService.DeleteById(id);
            return Json(JsonSerializer.Serialize(response));
        }
    }
}
