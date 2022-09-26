using BookMyMovie_Angular_Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BookMyMovie_Angular_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        BookMyMovieContext db = new BookMyMovieContext();

        [HttpPost]
        [Route("signin")]
        public IActionResult validateSignIn(Credentials query) {
            try
            {
                var data = db.Customers.Where(c => c.Email.ToLower().Equals(query.email) && c.Password.Equals(query.password)).FirstOrDefault();
                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);
            }
            catch (Exception ex) {
                return BadRequest(ex.InnerException.Message);
            }
        }
    }

    public class Credentials
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
