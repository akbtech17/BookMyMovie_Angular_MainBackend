using BookMyMovie_Angular_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookMyMovie_Angular_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        BookMyMovieContext db = new BookMyMovieContext();

        [HttpPost]
        [Route("signin")]
        public IActionResult validateSignIn(Credentials query)
        {
            try
            {
                var data = db.Admins.Where(c => c.Email.ToLower().Equals(query.email) && c.Password.Equals(query.password)).FirstOrDefault();
                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }
    }
}
