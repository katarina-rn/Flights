using Flights.Models;
using Flights.Models.ViewModels;
using Flights.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Controllers
{
    public class VisitorController : Controller
    {
        public readonly IReservationService _reservationService;
        UserManager<ApplicationUser> _userManager;
        public VisitorController(IReservationService reservationService, UserManager<ApplicationUser> userManager)
        {
            _reservationService = reservationService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            ReservationVM reservation = new ReservationVM
            {
                Passanger = new ApplicationUser(),
                FlightR = new Flight()
            };
            List<SelectListItem> dest = _reservationService.GetDestinationList().ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.Name,
                    Value = a.Name
                };
            });
            ViewBag.Destinations = dest;
            return View(reservation);
        }
        public IActionResult Reservations()
        {
            //List<ReservationVM> reservations = _reservationService.VisitorReservations(_userManager.GetUserId(User));
            return View();
        }
    }
}
