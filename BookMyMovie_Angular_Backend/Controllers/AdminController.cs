using BookMyMovie_Angular_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BookMyMovie_Angular_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        Db01Context db = new Db01Context();

        [HttpPost]
        [Route("signin")]
        public IActionResult ValidateSignIn(Credentials query)
        {
            try
            {
                Akbadmin adminData = db.Akbadmins
                    .Where(c => c.Email.ToLower().Equals(query.email) && c.Password
                    .Equals(query.password))
                    .FirstOrDefault();
                if (adminData == null) return NotFound();
                return Ok(adminData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }
    }
}
