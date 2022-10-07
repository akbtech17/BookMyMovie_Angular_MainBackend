using BookMyMovie_Angular_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BookMyMovie_Angular_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        Db01Context db = new Db01Context();

        [HttpPost]
        [Route("signin")]
        public IActionResult ValidateSignIn(Credentials query) {
            try
            {
                Akbcustomer customerData = db.Akbcustomers
                    .Where(c => c.Email.ToLower().Equals(query.Email) && c.Password.Equals(query.Password))
                    .FirstOrDefault();
                if (customerData == null) return NotFound();
                return Ok(customerData);
            }
            catch (Exception ex) {
                return BadRequest(ex.InnerException.Message);
            }
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(Akbcustomer customerData)
        {
            try
            {
                customerData.CustomerId = null;
                db.Akbcustomers.Add(customerData);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }
    }

    public class Credentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
