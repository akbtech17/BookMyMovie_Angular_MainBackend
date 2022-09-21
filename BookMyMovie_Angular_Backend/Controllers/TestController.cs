using BookMyMovie_Angular_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace BookMyMovie_Angular_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        BookMyMovieContext db = new BookMyMovieContext();

        [HttpGet]
        [Route("set-database")]
        public IActionResult Get()
        {
            try
            {
                db.Database.ExecuteSqlInterpolated($"SetDB");
            }
            catch (Exception ex) {
                return BadRequest("Error " + ex.InnerException.Message);
            }
            return Ok("Anshul have set the Database\n" +
                "Keep Working\n" +
                "All the Best");
        }
    }
}
