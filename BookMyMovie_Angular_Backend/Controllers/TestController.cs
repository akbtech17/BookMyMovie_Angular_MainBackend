using Microsoft.AspNetCore.Mvc;

namespace BookMyMovie_Angular_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        // GET: api/<TestController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hi Anshul Here!");
        }
    }
}
