using Flights.Models;
using Flights.Models.ViewModels;
using Flights.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Controllers
{
    [ApiController]
    [Route("api/flights")]
    public class FlightsController : Controller
    {
        private readonly IFlightService _flightService;
        public FlightsController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet]
        [Route("getFlights")]
        public IActionResult GetFlights()
        {
            List<FlightVM> flights = _flightService.AllFlights();
            return Ok(flights);
        }

        [HttpGet]
        [Route("searchFlights/{destinationFrom}/{destinationTo}/{date}/{connections}")]
        public IActionResult SearchFlights(string destinationFrom, string destinationTo, string date, string connections)
        {
            List<FlightVM> flights = _flightService.SearchFLights(destinationFrom, destinationTo, date, Convert.ToBoolean(connections));
            return Ok(flights);
        }

        [HttpGet]
        [Route("getFlightById/{id}")]
        public IActionResult GetFlightById(int id)
        {
            FlightVM flight = _flightService.FlightById(id);
            return Ok(flight);
        }

        [HttpGet]
        [Route("cancelFlightById/{id}")]
        public async Task<IActionResult> CancelFlightById(int id)
        {
            var response = await _flightService.DeleteById(id);
            return Ok(response);
        }
    }
}
