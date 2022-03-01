using Microsoft.AspNetCore.Mvc;
using FlighttPlanner.Storage;

namespace FlighttPlanner.Controllers
{
    [Route("testing-api")]
    [ApiController]
    public class TestingApiController : ControllerBase
    {
        [HttpPost]
        [Route("clear")]
        public IActionResult Clear()
        {
            FlightStorage.Clear();

            return Ok();
        }
    }
}
