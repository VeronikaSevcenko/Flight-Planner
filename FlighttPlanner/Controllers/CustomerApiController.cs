using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using FlighttPlanner.Storage;
using FlighttPlanner.Models;


namespace FlighttPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        private static readonly object _a = new();

        [HttpGet]
        [Route("Airports")]

        public IActionResult SearchAirports(string search)
        {
            lock(_a)
            {
                var airports = FlightStorage.FindAirports(search);
                return Ok(airports);
            }
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights(SearchFlightRequest request)
        {
           if (!FlightStorage.IsValidFlight(request))
           {
              return BadRequest();
           }

            return Ok( FlightStorage.SearchFlight(request));
        }

        [HttpGet]
        [Route("flights/{id}")]

        public IActionResult SearchFlights(int id)
        {
            var flight = FlightStorage.GetFlight(id);
            if (flight == null)
            {
                return NotFound();
            }
                return Ok(flight);
        }
    }
}
