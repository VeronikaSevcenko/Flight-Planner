using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FlighttPlanner.Models;
using FlighttPlanner.Storage;


namespace FlighttPlanner.Controllers
{
    [Route("admin-api")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        private static readonly object _a = new();

        [HttpGet]
        [Route("Flights/{id}")]
        [Authorize]
        public IActionResult GetFlights(int id)
        {
            lock(_a)
            {
                var flight = FlightStorage.GetFlight(id);
                if (flight == null)
                    return NotFound();
                return Ok(flight);
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("Flights/{id}")]
        public IActionResult DeleteFlights(int id)
        {
           lock(_a)
           {
               FlightStorage.DeleteFlight(id);
               return Ok();
           }
        }

        [HttpPut]
        [Route("Flights")]
        [Authorize]
        public IActionResult AddFlights(AddFlightRequest reguest)
        {
            lock(_a)
            {
                if (!FlightStorage.IsValid(reguest))
                {
                    return BadRequest();
                }

                if (FlightStorage.Exists(reguest))
                {
                    return Conflict();
                }
                return Created("", FlightStorage.AddFlight(reguest));
            }
        }
    }
}
